using UnityEngine;

public class OneBodySimulation : Simulation
{
    private OneBodyPrefabs prefabs;

    [HideInInspector] public Vector3 originPosition;

    private void Awake()
    {
        if (!TryGetComponent(out prefabs))
        {
            Debug.LogWarning("No TwoBodyPrefabs component found.");
            Pause();
            return;
        }

        prefabs.InstantiateAllPrefabs();
    }

    private void Start()
    {
        // originPosition should have been set by TwoBodySimulation in Awake()
        if (prefabs.coordinateOrigin)
        {
            prefabs.coordinateOrigin.position = originPosition;
        }

        if (prefabs.centralPotential)
        {
            prefabs.centralPotential.position = originPosition;
        }

        if (prefabs.angularMomentumVector)
        {
            prefabs.angularMomentumVector.SetPositions(originPosition, originPosition + 3.5f * Vector3.back);
            prefabs.angularMomentumVector.Redraw();
        }

        if (prefabs.orbitalPlane)
        {
            prefabs.orbitalPlane.position = 3.8f * Vector3.right + 0.05f * Vector3.down;
        }
    }

    public override void Reset()
    {

    }

    public void SetPosition(Vector3 position)
    {
        if (!prefabs)
        {
            return;
        }

        prefabs.body.position = position;
        prefabs.UpdateVectors(originPosition);
    }
}
