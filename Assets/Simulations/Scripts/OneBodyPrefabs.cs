using UnityEngine;

public class OneBodyPrefabs : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject positionVectorWhitePrefab;
    [SerializeField] private GameObject positionVectorBlackPrefab;
    [SerializeField] private GameObject coordinateOriginPrefab;
    [SerializeField] private GameObject centralPotentialPrefab;
    [SerializeField] private GameObject angularMomentumVectorPrefab;
    [SerializeField] private GameObject orbitalPlanePrefab;

    [HideInInspector] public Transform body;
    [HideInInspector] public Vector positionVectorWhite;
    [HideInInspector] public Vector positionVectorBlack;
    [HideInInspector] public Transform coordinateOrigin;
    [HideInInspector] public Transform centralPotential;
    [HideInInspector] public Vector angularMomentumVector;
    [HideInInspector] public Transform orbitalPlane;

    public void SetPositionVectorWhiteVisibility(bool visible)
    {
        if (positionVectorWhite)
        {
            positionVectorWhite.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVectorBlackVisibility(bool visible)
    {
        if (positionVectorBlack)
        {
            positionVectorBlack.gameObject.SetActive(visible);
        }
    }

    public void SetCoordinateOriginVisibility(bool visible)
    {
        if (coordinateOrigin)
        {
            coordinateOrigin.gameObject.SetActive(visible);
        }
    }

    public void SetCentralPotentialVisibility(bool visible)
    {
        if (centralPotential)
        {
            centralPotential.gameObject.SetActive(visible);
        }
    }

    public void SetAngularMomentumVectorVisibility(bool visible)
    {
        if (angularMomentumVector)
        {
            angularMomentumVector.gameObject.SetActive(visible);
        }
    }

    public void SetBodyLabelVisibility(bool visible)
    {
        Transform label = body.Find("Label");
        if (label)
        {
            label.gameObject.SetActive(visible);
        }
    }

    public void SetOrbitalPlaneVisibility(bool visible)
    {
        if (orbitalPlane)
        {
            orbitalPlane.gameObject.SetActive(visible);
        }
    }

    public void InstantiateAllPrefabs()
    {
        if (bodyPrefab)
        {
            body = Instantiate(bodyPrefab, transform).transform;
            body.name = "Single Body";
        }

        if (positionVectorWhitePrefab)
        {
            positionVectorWhite = Instantiate(positionVectorWhitePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVectorWhite.SetPositions(Vector3.zero, Vector3.zero);
            positionVectorWhite.name = "Single Body Position Vector";
        }

        if (positionVectorBlackPrefab)
        {
            positionVectorBlack = Instantiate(positionVectorBlackPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVectorBlack.SetPositions(Vector3.zero, Vector3.zero);
            positionVectorBlack.name = "Single Body Position Vector";
        }

        if (coordinateOriginPrefab)
        {
            coordinateOrigin = Instantiate(coordinateOriginPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            coordinateOrigin.name = "Single Body Coordinate Origin";
        }

        if (centralPotentialPrefab)
        {
            centralPotential = Instantiate(centralPotentialPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            centralPotential.name = "Single Body Central Potential";
        }

        if (angularMomentumVectorPrefab)
        {
            angularMomentumVector = Instantiate(angularMomentumVectorPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            angularMomentumVector.SetPositions(Vector3.zero, Vector3.zero);
            angularMomentumVector.name = "Single Body Angular Momentum";
        }

        if (orbitalPlanePrefab)
        {
            orbitalPlane = Instantiate(orbitalPlanePrefab, Vector3.zero, Quaternion.identity, transform).transform;
            orbitalPlane.name = "Orbital Plane";
        }
    }

    public void UpdateVectors(Vector3 originPosition)
    {
        if (positionVectorWhite)
        {
            positionVectorWhite.SetPositions(originPosition, body.position);
            positionVectorWhite.Redraw();
        }

        if (positionVectorBlack)
        {
            positionVectorBlack.SetPositions(originPosition, body.position);
            positionVectorBlack.Redraw();
        }
    }
}
