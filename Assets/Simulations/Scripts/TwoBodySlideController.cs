using UnityEngine;

public class TwoBodySlideController : SimulationSlideController
{
    [Header("Coordinate Origin")]
    [SerializeField] private bool originIsDraggable;
    [SerializeField] private Vector3 originPosition = Vector3.zero;
    [SerializeField] private float originMoveTime = 1;

    [Header("Object Visibility")]
    [SerializeField] private bool centerOfMass;
    [SerializeField] private bool coordinateOrigin;
    [SerializeField] private bool positionVector1;
    [SerializeField] private bool positionVector2;
    [SerializeField] private bool positionVector3;
    [SerializeField] private bool positionVectorCOM;
    [SerializeField] private bool trail1;
    [SerializeField] private bool trail2;
    [SerializeField] private bool forceVector1;
    [SerializeField] private bool forceVector2;
    [SerializeField] private bool bodyLabel1;
    [SerializeField] private bool bodyLabel2;

    private TwoBodyPrefabs prefabs;

    private void Awake()
    {
        simulation = (TwoBodySimulation)simulation;
        if (!simulation.TryGetComponent(out prefabs))
        {
            Debug.LogWarning("Did not find a TwoBodyPrefabs component");
        }
    }

    public override void ShowAndHideUIElements()
    {
        if (prefabs == null)
        {
            return;
        }

        prefabs.SetCoordinateOriginVisibility(coordinateOrigin);
        prefabs.SetCenterOfMassVisibility(centerOfMass);
        prefabs.SetPositionVector1Visibility(positionVector1);
        prefabs.SetPositionVector2Visibility(positionVector2);
        prefabs.SetPositionVector3Visibility(positionVector3);
        prefabs.SetPositionVectorCOMVisibility(positionVectorCOM);
        prefabs.SetTrail1Visibility(trail1);
        prefabs.SetTrail2Visibility(trail2);
        prefabs.SetForceVector1Visibility(forceVector1);
        prefabs.SetForceVector2Visibility(forceVector2);
        prefabs.SetBodyLabel1Visibility(bodyLabel1);
        prefabs.SetBodyLabel2Visibility(bodyLabel2);

        // Move coordinate origin into position
        prefabs.LerpOriginToPosition(originPosition, originMoveTime, originIsDraggable);
    }
}
