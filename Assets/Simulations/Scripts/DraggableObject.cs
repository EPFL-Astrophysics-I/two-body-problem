using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    public enum DragPlane { XY }
    public DragPlane dragPlane = DragPlane.XY;

    public bool draggable = true;

    private Camera mainCamera;
    private bool clickedOnUIElement;
    private new Collider collider;
    private Vector3 startPosition;
    private Vector2 viewportClickedPosition;
    private Vector2 visibleWorldXY;
    private bool dragging;

    private void Awake()
    {
        // Get reference to the camera for raycasting
        mainCamera = Camera.main;

        TryGetComponent(out collider);
    }

    private void Update()
    {
        if (!draggable || !collider)
        {
            return;
        }

        // Initial mouse click on the moon
        if (Input.GetMouseButtonDown(0))
        {
            clickedOnUIElement = EventSystem.current.IsPointerOverGameObject();

            if (clickedOnUIElement)
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (collider.Raycast(ray, out _, 1000f))
            {
                startPosition = transform.position;
                viewportClickedPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                float height = Mathf.Tan(mainCamera.fieldOfView * Mathf.Deg2Rad) * Mathf.Abs(mainCamera.transform.position.z);
                float width = height * mainCamera.aspect;
                visibleWorldXY = new Vector2(width, height);
                dragging = true;
            }
        }

        // Hitting while dragging
        if (Input.GetMouseButton(0) && dragging && !clickedOnUIElement)
        {
            // Convert Viewport distance to distance along the world space x- and y-axis
            Vector2 viewportPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            Vector2 worldDelta = (viewportPosition - viewportClickedPosition) * visibleWorldXY;
            Vector2 newPosition = startPosition + new Vector3(worldDelta.x, worldDelta.y, 0);
            transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            clickedOnUIElement = false;
            dragging = false;
        }
    }
}
