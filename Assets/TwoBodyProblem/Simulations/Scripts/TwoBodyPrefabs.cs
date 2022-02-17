using System.Collections;
using UnityEngine;

public class TwoBodyPrefabs : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject body1Prefab;
    [SerializeField] private GameObject body2Prefab;
    [SerializeField] private GameObject coordinateOriginPrefab;
    [SerializeField] private GameObject centerOfMassPrefab;
    [SerializeField] private GameObject positionVector1Prefab;
    [SerializeField] private GameObject positionVector2Prefab;
    [SerializeField] private GameObject positionVector3WhitePrefab;
    [SerializeField] private GameObject positionVector3BlackPrefab;
    [SerializeField] private GameObject positionVectorCOMPrefab;
    [SerializeField] private GameObject trail1Prefab;
    [SerializeField] private GameObject trail2Prefab;
    [SerializeField] private GameObject forceVector1Prefab;
    [SerializeField] private GameObject forceVector2Prefab;
    [SerializeField] private GameObject angularMomentumVectorPrefab;

    [HideInInspector] public Transform body1;
    [HideInInspector] public Transform body2;
    [HideInInspector] public Transform coordinateOrigin;
    [HideInInspector] public Transform centerOfMass;
    [HideInInspector] public Vector positionVector1;
    [HideInInspector] public Vector positionVector2;
    [HideInInspector] public Vector positionVector3White;
    [HideInInspector] public Vector positionVector3Black;
    [HideInInspector] public Vector positionVectorCOM;
    [HideInInspector] public LineRenderer trail1;
    [HideInInspector] public LineRenderer trail2;
    [HideInInspector] public Vector forceVector1;
    [HideInInspector] public Vector forceVector2;
    [HideInInspector] public Vector angularMomentumVector;

    public void SetCenterOfMassVisibility(bool visible)
    {
        if (centerOfMass)
        {
            centerOfMass.gameObject.SetActive(visible);
        }
    }

    public void SetCoordinateOriginVisibility(bool visible)
    {
        if (coordinateOrigin)
        {
            coordinateOrigin.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVector1Visibility(bool visible)
    {
        if (positionVector1)
        {
            positionVector1.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVector2Visibility(bool visible)
    {
        if (positionVector2)
        {
            positionVector2.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVector3WhiteVisibility(bool visible)
    {
        if (positionVector3White)
        {
            positionVector3White.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVector3BlackVisibility(bool visible)
    {
        if (positionVector3Black)
        {
            positionVector3Black.gameObject.SetActive(visible);
        }
    }

    public void SetPositionVectorCOMVisibility(bool visible)
    {
        if (positionVectorCOM)
        {
            positionVectorCOM.gameObject.SetActive(visible);
        }
    }

    public void SetTrail1Visibility(bool visible)
    {
        if (trail1)
        {
            trail1.gameObject.SetActive(visible);
        }
    }

    public void SetTrail2Visibility(bool visible)
    {
        if (trail2)
        {
            trail2.gameObject.SetActive(visible);
        }
    }

    public void SetForceVector1Visibility(bool visible)
    {
        if (forceVector1)
        {
            forceVector1.gameObject.SetActive(visible);
        }
    }

    public void SetForceVector2Visibility(bool visible)
    {
        if (forceVector2)
        {
            forceVector2.gameObject.SetActive(visible);
        }
    }

    public void SetBodyLabel1Visibility(bool visible)
    {
        Transform label = body1.Find("Label");
        if (label)
        {
            label.gameObject.SetActive(visible);
        }
    }

    public void SetBodyLabel2Visibility(bool visible)
    {
        Transform label = body2.Find("Label");
        if (label)
        {
            label.gameObject.SetActive(visible);
        }
    }

    public void SetAngularMomentumVectorVisibility(bool visible)
    {
        if (angularMomentumVector)
        {
            angularMomentumVector.gameObject.SetActive(visible);
        }
    }

    public void InstantiateAllPrefabs()
    {
        if (body1Prefab)
        {
            body1 = Instantiate(body1Prefab, transform).transform;
            body1.name = "Body 1";
        }

        if (body2Prefab)
        {
            body2 = Instantiate(body2Prefab, transform).transform;
            body2.name = "Body 2";
        }

        if (centerOfMassPrefab)
        {
            centerOfMass = Instantiate(centerOfMassPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            centerOfMass.name = "Center of Mass";
        }

        if (coordinateOriginPrefab)
        {
            coordinateOrigin = Instantiate(coordinateOriginPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            coordinateOrigin.name = "Coordinate Origin";
        }

        if (positionVector1Prefab)
        {
            positionVector1 = Instantiate(positionVector1Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVector1.SetPositions(Vector3.zero, Vector3.zero);
            positionVector1.name = "Position Vector 1";
        }

        if (positionVector2Prefab)
        {
            positionVector2 = Instantiate(positionVector2Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVector2.SetPositions(Vector3.zero, Vector3.zero);
            positionVector2.name = "Position Vector 2";
        }

        if (positionVector3WhitePrefab)
        {
            positionVector3White = Instantiate(positionVector3WhitePrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVector3White.SetPositions(Vector3.zero, Vector3.zero);
            positionVector3White.name = "Position Vector 3 White";
        }

        if (positionVector3BlackPrefab)
        {
            positionVector3Black = Instantiate(positionVector3BlackPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVector3Black.SetPositions(Vector3.zero, Vector3.zero);
            positionVector3Black.name = "Position Vector 3 Black";
        }

        if (positionVectorCOMPrefab)
        {
            positionVectorCOM = Instantiate(positionVectorCOMPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            positionVectorCOM.SetPositions(Vector3.zero, Vector3.zero);
            positionVectorCOM.name = "Position Vector COM";
        }

        if (trail1Prefab)
        {
            trail1 = Instantiate(trail1Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<LineRenderer>();
            trail1.positionCount = 0;
            trail1.name = "Trail 1";
        }

        if (trail2Prefab)
        {
            trail2 = Instantiate(trail2Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<LineRenderer>();
            trail2.positionCount = 0;
            trail2.name = "Trail 2";
        }

        if (forceVector1Prefab)
        {
            forceVector1 = Instantiate(forceVector1Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            forceVector1.SetPositions(Vector3.zero, Vector3.zero);
            forceVector1.name = "Force Vector 1";
        }

        if (forceVector2Prefab)
        {
            forceVector2 = Instantiate(forceVector2Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            forceVector2.SetPositions(Vector3.zero, Vector3.zero);
            forceVector2.name = "Force Vector 2";
        }

        if (angularMomentumVectorPrefab)
        {
            angularMomentumVector = Instantiate(angularMomentumVectorPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<Vector>();
            angularMomentumVector.SetPositions(Vector3.zero, Vector3.zero);
            angularMomentumVector.name = "Angular Momentum Vector";
        }
    }

    public void UpdateCenterOfMass(Vector3 position)
    {
        if (centerOfMass)
        {
            centerOfMass.position = position;
        }
    }

    public void UpdateVectors()
    {
        if (positionVector1)
        {
            positionVector1.SetPositions(coordinateOrigin.position, body1.position);
            positionVector1.Redraw();
        }

        if (positionVector2)
        {
            positionVector2.SetPositions(coordinateOrigin.position, body2.position);
            positionVector2.Redraw();
        }

        if (positionVector3White)
        {
            positionVector3White.SetPositions(body2.position, body1.position);
            positionVector3White.Redraw();
        }

        if (positionVector3Black)
        {
            positionVector3Black.SetPositions(body2.position, body1.position);
            positionVector3Black.Redraw();
        }

        if (positionVectorCOM)
        {
            positionVectorCOM.SetPositions(coordinateOrigin.position, centerOfMass.position);
            positionVectorCOM.Redraw();
            positionVectorCOM.SetLabelVisibility(positionVectorCOM.Displacement.magnitude > 1.5f);
        }

        Vector3 r = body2.position - body1.position;
        Vector3 force = 3 * r / r.sqrMagnitude;

        if (forceVector1)
        {
            forceVector1.SetPositions(body1.position, body1.position + force);
            forceVector1.Redraw();
        }

        if (forceVector2)
        {
            forceVector2.SetPositions(body2.position, body2.position - force);
            forceVector2.Redraw();
        }
    }

    public void LerpOriginToPosition(Vector3 position, float moveTime, bool draggable = false)
    {
        if (!coordinateOrigin)
        {
            return;
        }

        //StopAllCoroutines();
        StartCoroutine(MoveCoordinateOriginToPosition(position, moveTime, draggable));
    }

    private IEnumerator MoveCoordinateOriginToPosition(Vector3 targetPosition, float moveTime, bool draggable)
    {
        float time = 0;
        Vector3 startPosition = coordinateOrigin.position;

        if (coordinateOrigin.TryGetComponent(out DraggableObject draggableObject))
        {
            draggableObject.draggable = false;
        }

        while (time < moveTime)
        {
            time += Time.deltaTime;
            coordinateOrigin.position = Vector3.Lerp(startPosition, targetPosition, time / moveTime);
            yield return null;
        }

        coordinateOrigin.position = targetPosition;

        if (draggableObject)
        {
            draggableObject.draggable = draggable;
        }        
    }
}
