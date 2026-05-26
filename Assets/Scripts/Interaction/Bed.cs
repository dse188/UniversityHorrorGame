using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private AlarmClockManager alarmClockManager;

    public string GetPromptText()
    {
        if (alarmClockManager == null) return "Bed";
        return alarmClockManager.IsAlarmSet 
            ? "Press E to sleep" 
            : "Sleep your alarm first";
    }

    public void Interact()
    {
        if (alarmClockManager == null) return;
        if (!alarmClockManager.IsAlarmSet) return;
        alarmClockManager.BeginSleep();
    }

}
