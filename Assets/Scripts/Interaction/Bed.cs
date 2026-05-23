using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private string promptText = "Press E to sleep";

    public string GetPromptText() => promptText;

    public void Interact()
    {
        if (daySystem == null) return;
        daySystem.AdvanceDay();
    }

}
