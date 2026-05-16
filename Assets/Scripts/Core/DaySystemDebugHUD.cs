using UnityEngine;
using UnityEngine.InputSystem;

public class DaySystemDebugHUD : MonoBehaviour
{
    [SerializeField] private DaySystem daySystem;

    private void Update()
    {
        if (daySystem == null) return;
        
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.f1Key.wasPressedThisFrame)
        {
            daySystem.ConsumeSlot();
        }
        if (kb.f2Key.wasPressedThisFrame)
        {
            daySystem.AdvanceDay();
        }

    }

    private void OnGUI()
    {
        if (daySystem == null || daySystem.CurrentDay == null) return;

        var day = daySystem.CurrentDay;
        GUI.Label(new Rect(10, 10, 400, 20), $"Day {day.DayNumber}: {day.DayName}");
        GUI.Label(new Rect(10, 30, 400, 20), $"Slots Remaining: {daySystem.SlotsRemaining} / {day.SlotBudget}");
        GUI.Label(new Rect(10, 50, 400, 20), $"Stress floor: {day.StressFloor}");
        GUI.Label(new Rect(10, 70, 400, 20), $"Tasks: {day.RequiredTasks.Count}");

        GUI.Box(new Rect(10, 140, 280, 75), "Controls");
        GUI.Label(new Rect(20, 165, 260, 20), "[F1]  Consume slot");
        GUI.Label(new Rect(20, 185, 260, 20), "[F2]  Advance day");
    }
}
