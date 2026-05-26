using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] private AlarmClockManager alarmClockManager;

    public string GetPromptText()
    {
        if (alarmClockManager == null) return "Alarm clock";
        AlarmChoice next = NextChoice(alarmClockManager.Choice);
        string current = alarmClockManager.IsAlarmSet
            ? $" (now {alarmClockManager.Choice})"
            : "";
        return $"Press E to set alarm: {next}{current}";
    }

    public void Interact()
    {
        if (alarmClockManager == null) return;
        alarmClockManager.SetAlarm(NextChoice(alarmClockManager.Choice));
    }

    private static AlarmChoice NextChoice(AlarmChoice current) => current switch
    {
        AlarmChoice.Early => AlarmChoice.Normal,
        AlarmChoice.Normal => AlarmChoice.Late,
        AlarmChoice.Late => AlarmChoice.Early,
        _ => AlarmChoice.Early
    };
}
