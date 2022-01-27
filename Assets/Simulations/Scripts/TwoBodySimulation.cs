using UnityEngine;

public class TwoBodySimulation : Simulation
{
    //[SerializeField] private GameObject body1Prefab;
    //[SerializeField] private GameObject body2Prefab;
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

    // Conserved quantites
    private float energy;
    private float period;
    [HideInInspector] public float angularMomentum;

    // Properties
    public float M => totalMass;
    public float Mu => reducedMass;
    public float Energy => energy;
    public float Period => period;

    private void Awake()
    {
        if (!TryGetComponent(out prefabs))
        {
            Debug.LogWarning("No TwoBodyPrefabs component found.");
            Pause();
            return;
        }

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
        }

        Reset();
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
            body1.position = CenterOfMassPosition() + (mass2 / M * r);
            body2.position = CenterOfMassPosition() - (mass1 / M * r);
        }

        // Compute the new center of mass position
        time += Time.fixedDeltaTime;
        resetTimer += Time.fixedDeltaTime;
        Vector3 R = CenterOfMassPosition();

        // Solve the equation of motion for r
        float substep = Time.fixedDeltaTime / numSubsteps;
        for (int i = 1; i <= numSubsteps; i++)
        {
            StepForward(substep);
        }

        // Update each body's position
        body1.position = R + (mass1 / M * r);
        body2.position = R - (mass2 / M * r);

        // Let TwoBodyPrefabs know to update its vectors
        prefabs.UpdateVectors();
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

    public override void Reset()
    {
        time = 0;
        resetTimer = 0;

        r = initPosition1 - initPosition2;
        v = initVelocity1 - initVelocity2;

        // Compute conserved quantities
        totalMass = mass1 + mass2;
        reducedMass = mass1 * mass2 / totalMass;
        energy = 0.5f * reducedMass * v.sqrMagnitude - newtonG * mass1 * mass2 / r.magnitude;

        // Save the center of mass starting position and velocity
        initPositionCM = (mass1 * initPosition1 + mass2 * initPosition2) / totalMass;
        initVelocityCM = (mass1 * initVelocity1 + mass2 * initVelocity2) / totalMass;

        // Compute the orbital period
        if (energy >= 0)
        {
            // Unbound orbit
            period = float.PositiveInfinity;
        }
        else
        {
            // Bound orbit
            float a = -0.5f * newtonG * mass1 * mass2 / energy;
            period = 2 * Mathf.PI * Mathf.Sqrt(a * a * a / newtonG / totalMass);
        }

        Debug.Log("Period is " + Period + " s");
        Debug.Log("CM is " + initPositionCM);
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
