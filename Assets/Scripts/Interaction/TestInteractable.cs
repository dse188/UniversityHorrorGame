using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText = "Press E to interact";

    public string GetPromptText() => promptText;

    public void Interact()
    {
        Debug.Log($"Interacted with {name}");
    }
}
