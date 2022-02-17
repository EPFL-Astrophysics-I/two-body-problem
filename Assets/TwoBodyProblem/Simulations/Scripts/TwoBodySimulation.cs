using UnityEngine;

public class TwoBodySimulation : Simulation
{
    private TwoBodyPrefabs prefabs;

    [Header("Simulation Properties")]
    [SerializeField, Min(0)] private float newtonG = 1f;
    [SerializeField, Min(1)] private int numSubsteps = 20;
    [SerializeField, Min(0)] private float mass1 = 1f;
    [SerializeField, Min(0)] private float mass2 = 1f;

    [Header("Initial Conditions")]
    [SerializeField] private Vector3 initPosition1 = Vector3.left;
    [SerializeField] private Vector3 initPosition2 = Vector3.right;
    [SerializeField] private Vector3 initVelocity1 = Vector3.up;
    [SerializeField] private Vector3 initVelocity2 = Vector3.down;

    [Header("Single-Body Equivalent")]
    [SerializeField] private OneBodySimulation oneBodySim;
    [SerializeField] private Vector3 oneBodyOffset;

    // References to the actual transforms held in TwoBodyPrefabs
    private Transform body1;
    private Transform body2;

    // Initial center of mass quantities
    private Vector3 initPositionCM;
    private Vector3 initVelocityCM;

    // Quantities of motion
    private float time;
    private float resetTimer;
    private float totalMass;
    private float reducedMass;
    [HideInInspector] public Vector3 r;  // r1 - r2
    [HideInInspector] public Vector3 v;  // time derivative of r
    [HideInInspector] public float theta;  // angular coordinate in the orbital plane

    // Conserved quantites (evaluated in CM frame)
    private float energy;
    private float period;
    private Vector3 angularMomentum;
    private float magnitudeL;
    private float semiMajorAxis;
    private float eccentricity;
    private float initTheta;

    // Properties
    public float M => totalMass;
    public float Mu => reducedMass;
    public float Energy => energy;
    public float Period => period;
    public float L => magnitudeL;
    public float SemiMajorAxis => semiMajorAxis;
    public float Eccentricity => eccentricity;

    // Coordinate system with angular momentum along the z-axis and x, y in the orbital plane
    private Vector3 xHat = Vector3.right;
    private Vector3 yHat = Vector3.up;
    private Vector3 zHat = Vector3.forward;

    private void Awake()
    {
        if (!TryGetComponent(out prefabs))
        {
            Debug.LogWarning("No TwoBodyPrefabs component found.");
            Pause();
            return;
        }

        // Place the simulation at the center of mass
        totalMass = mass1 + mass2;
        initPositionCM = (mass1 * initPosition1 + mass2 * initPosition2) / totalMass;
        initVelocityCM = (mass1 * initVelocity1 + mass2 * initVelocity2) / totalMass;
        transform.position = initPositionCM;

        // Create all objects
        prefabs.InstantiateAllPrefabs();

        body1 = prefabs.body1;
        body1.localScale = 2 * Mathf.Pow(3f * mass1 / 4f / Mathf.PI, 0.333f) * Vector3.one;
        body1.position = initPosition1;

        body2 = prefabs.body2;
        body2.localScale = 2 * Mathf.Pow(3f * mass2 / 4f / Mathf.PI, 0.333f) * Vector3.one;
        body2.position = initPosition2;

        if (prefabs.centerOfMass)
        {
            prefabs.centerOfMass.localScale = 0.5f * Vector3.one;
            prefabs.UpdateCenterOfMass(CenterOfMassPosition());
        }

        if (oneBodySim)
        {
            oneBodySim.originPosition = oneBodyOffset;
        }

        Reset();

        // Orbital plane coordinate system
        zHat = angularMomentum.normalized;
        //xHat = Quaternion.AngleAxis(-initTheta, zHat) * r.normalized;
        xHat = -r.normalized;
        yHat = Vector3.Cross(zHat, xHat);
    }

    private void Start()
    {
        Vector3 positionCM = transform.position;

        if (prefabs.angularMomentumVector)
        {
            prefabs.angularMomentumVector.SetPositions(positionCM, positionCM + 3.5f * Vector3.back);
            prefabs.angularMomentumVector.Redraw();
        }
    }

    private void FixedUpdate()
    {
        if (paused)
        {
            return;
        }

        if (resetTimer >= Period)
        {
            resetTimer = 0;
            r = initPosition1 - initPosition2;
            v = initVelocity1 - initVelocity2;
            body1.localPosition = mass2 / M * r;
            body2.localPosition = -mass1 / M * r;

            theta = initTheta;
        }

        time += Time.fixedDeltaTime;
        resetTimer += Time.fixedDeltaTime;

        // Move the simulation to the new CM position
        transform.position = CenterOfMassPosition(time);

        // Bound orbits
        if (energy < 0)
        {
            // Solve the equation of motion for theta
            float substep = Time.fixedDeltaTime / numSubsteps;
            for (int i = 0; i < numSubsteps; i++)
            {
                StepForwardThetaR(substep);
            }
        }
        else
        {
            // Solve the equation of motion for r
            float substep = Time.fixedDeltaTime / numSubsteps;
            for (int i = 1; i <= numSubsteps; i++)
            {
                StepForward(substep);
            }
        }

        // Update each body's position in the CM frame
        body1.localPosition = mass2 / M * r;
        body2.localPosition = -mass1 / M * r;

        // Let TwoBodyPrefabs know how to update its vectors
        prefabs.UpdateVectors();

        // Update the equivalent single body if assigned
        if (oneBodySim)
        {
            oneBodySim.SetPosition(oneBodyOffset + r);
        }
    }

    private void StepForward(float deltaTime)
    {
        // Solve the equation of motion for the difference vector r in the CM frame
        // (1) dr/dt = v
        // (2) dv/dt = -(G * M / r^2) * rhat

        // Change in velocity during time deltaTime
        Vector3 specificForce = -(newtonG * M / r.sqrMagnitude) * r.normalized;
        Vector3 deltaV = specificForce * deltaTime;
        r += v * deltaTime;
        v += deltaV;
    }

    private void StepForwardThetaR(float deltaTime)
    {
        float angularSpeed = -L / Mu / r.sqrMagnitude;
        theta += angularSpeed * deltaTime;
        float a = SemiMajorAxis;
        float e = Eccentricity;
        float rMagnitude = a * (1f - e * e) / (1f + e * Mathf.Cos(theta));
        r = rMagnitude * (Mathf.Cos(theta) * xHat + Mathf.Sin(theta) * yHat);
    }

    public override void Reset()
    {
        time = 0;
        resetTimer = 0;

        r = initPosition1 - initPosition2;
        v = initVelocity1 - initVelocity2;

        // Compute conserved quantities (in CM frame)
        reducedMass = mass1 * mass2 / totalMass;
        energy = 0.5f * reducedMass * v.sqrMagnitude - newtonG * reducedMass * totalMass / r.magnitude;
        angularMomentum = reducedMass * Vector3.Cross(r, v);
        magnitudeL = angularMomentum.magnitude;

        // Compute orbital properties
        semiMajorAxis = -0.5f * newtonG * reducedMass * totalMass / energy;
        eccentricity = Mathf.Sqrt(1f + 2f * energy * Mathf.Pow(magnitudeL / newtonG / reducedMass / totalMass, 2) / reducedMass);
        // Period and initial theta
        if (energy >= 0)
        {
            // Unbound orbit
            period = float.PositiveInfinity;
        }
        else
        {
            // Bound orbit
            float a = semiMajorAxis;
            period = 2 * Mathf.PI * Mathf.Sqrt(a * a * a / newtonG / totalMass);

            float e = eccentricity;
            // Avoid NANs by making sure the argument of arccos is strictly between -1 and 1
            float arg = Mathf.Clamp(((a * (1f - e * e) / r.magnitude) - 1f) / e, -1f, 1f);
            initTheta = Mathf.Acos(arg);
            theta = initTheta;
        }

        //Debug.Log("Period is " + Period + " s");
        //Debug.Log("CM is at " + initPositionCM);
        //Debug.Log("CM v is " + initVelocityCM);
    }

    // Center of mass position at any time
    public Vector3 CenterOfMassPosition(float time)
    {
        return initPositionCM + initVelocityCM * time;
    }

    // Center of mass position at the current time
    public Vector3 CenterOfMassPosition()
    {
        return CenterOfMassPosition(time);
    }
}
