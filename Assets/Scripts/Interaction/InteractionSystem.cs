using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private InteractionPromptUI promptUI;
    [SerializeField] private GameModeController gameMode;

    private IInteractable currentTarget;
    

    private void Update()
    {
        UpdateTarget();
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        currentTarget?.Interact();
    }

    private void UpdateTarget()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        IInteractable detected = null;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            hit.collider.TryGetComponent(out detected);
        }

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, detected != null ? Color.red : Color.blue);

        // State-change check - only touch the UI when the target actually changes.
        if (detected == currentTarget) return;

        currentTarget = detected;
        if (currentTarget != null)
        {
            promptUI.Show(currentTarget.GetPromptText());
        }
        else
        {
            promptUI.Hide();
        }
    }

    private void OnEnable()
    {
        if (gameMode != null) gameMode.OnModeChanged += HandleModeChanged;
    }

    private void OnDisable()
    {
        if (gameMode != null) gameMode.OnModeChanged -= HandleModeChanged;
    }

    private void HandleModeChanged(GameMode mode)
    {
        bool shouldRun = mode == GameMode.FreeRoam;
        if (!shouldRun && promptUI != null) promptUI.Hide();
        currentTarget = null;
        enabled = shouldRun;
    }



}
