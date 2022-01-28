using UnityEngine;

public class OneBodySlideController : SimulationSlideController
{
    [Header("Object Visibility")]
    [SerializeField] private bool coordinateOrigin;
    [SerializeField] private bool positionVectorWhite;
    [SerializeField] private bool positionVectorBlack;
    [SerializeField] private bool centralPotential;
    [SerializeField] private bool angularMomentumVector;
    [SerializeField] private bool bodyLabel;
    [SerializeField] private bool orbitalPlane;

    private OneBodyPrefabs prefabs;

    private void Awake()
    {
        simulation = (OneBodySimulation)simulation;
        if (!simulation.TryGetComponent(out prefabs))
        {
            Debug.LogWarning("Did not find a OneBodyPrefabs component");
        }
    }

    public override void ShowAndHideUIElements()
    {
        if (prefabs == null)
        {
            return;
        }

        prefabs.SetPositionVectorWhiteVisibility(positionVectorWhite);
        prefabs.SetPositionVectorBlackVisibility(positionVectorBlack);
        prefabs.SetCoordinateOriginVisibility(coordinateOrigin);
        prefabs.SetCentralPotentialVisibility(centralPotential);
        prefabs.SetAngularMomentumVectorVisibility(angularMomentumVector);
        prefabs.SetBodyLabelVisibility(bodyLabel);
        prefabs.SetOrbitalPlaneVisibility(orbitalPlane);
    }
}
