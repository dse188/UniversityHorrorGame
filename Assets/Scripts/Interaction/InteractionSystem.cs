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
    private bool isGated;

    private void Update()
    {
        if (isGated) return;
        UpdateTarget();
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        if (isGated) return;
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

        if (detected == currentTarget) return;

        currentTarget = detected;
        if (currentTarget != null) promptUI.Show(currentTarget.GetPromptText());
        else                       promptUI.Hide();
    }

    private void OnEnable()
    {
        if (gameMode != null) gameMode.OnModeChanged += HandleModeChanged;
    }

    private void OnDisable()
    {
        if (gameMode != null) gameMode.OnModeChanged -= HandleModeChanged;
    }

    // WHY isGated instead of `this.enabled`: toggling `enabled` fires
    // OnDisable, which unsubscribes us from OnModeChanged — so the
    // FreeRoam event after a modal session would never reach us and
    // we'd stay locked out for the rest of play mode.
    private void HandleModeChanged(GameMode mode)
    {
        isGated = mode != GameMode.FreeRoam;
        if (isGated && promptUI != null) promptUI.Hide();
        currentTarget = null;
    }
}
