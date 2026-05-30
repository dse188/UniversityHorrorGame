using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private StudyModeController studyMode;
    [SerializeField] private DaySystem daySystem;

    public string GetPromptText()
    {
        if (studyMode == null) return "Computer";
        if (studyMode.IsTodayStudyDone) return "Study notes — done";
        if (!studyMode.HasStudyTaskToday) return "Nothing to do here today";
        if (daySystem != null && daySystem.IsExhausted) return "Too tired to focus";
        return "Press E to study";
    }

    public void Interact()
    {
        if (studyMode == null) return;
        if (studyMode.IsTodayStudyDone) return;
        if (!studyMode.HasStudyTaskToday) return;
        if (daySystem != null && daySystem.IsExhausted) return;
        studyMode.Begin();
    }
}
