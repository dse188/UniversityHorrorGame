using UnityEngine;
using System.Text;

public class Whiteboard : MonoBehaviour, IInteractable
{
 [SerializeField] private TaskBoardManager taskBoardManager;
 [SerializeField] private DaySystem daySystem;
 [SerializeField] private WhiteboardPanelUI panelUI;

 public string GetPromptText()
    {
        return panelUI != null && panelUI.IsOpen
            ? "Press E to close"
            : "Press E to view tasks";
    }

    public void Interact()
    {
        if (panelUI == null) return;

        if (panelUI.IsOpen) panelUI.Hide();
        else RefreshPanel();
    }

    private void RefreshPanel()
    {
        if (panelUI == null || daySystem == null || taskBoardManager == null) return;
        if (daySystem.CurrentDay == null) return;

        var day = daySystem.CurrentDay;
        string title = $"Day {day.DayNumber} — {day.DayName}";

        var sb = new StringBuilder();
        sb.AppendLine($"Slots: {daySystem.SlotsRemaining} / {day.SlotBudget}");
        sb.AppendLine();

        var tasks = taskBoardManager.CurrentTasks;
        for (int i = 0; i < tasks.Count; i++)
        {
            string mark = taskBoardManager.IsComplete(i) ? "x" : " ";
            sb.AppendLine($"[{mark}] {tasks[i].title}");
        }

        panelUI.Show(title, sb.ToString());
    }

    private void OnEnable()
    {
        if (taskBoardManager != null) taskBoardManager.OnTasksCompleted += HandleTasksCompleted;
        if (daySystem != null) daySystem.OnDayAdvanced += HandleDayAdvanced;
    }

    private void OnDisable()
    {
        if (taskBoardManager != null) taskBoardManager.OnTasksCompleted -= HandleTasksCompleted;
        if (daySystem != null) daySystem.OnDayAdvanced -= HandleDayAdvanced;
    }

    private void HandleTasksCompleted(int _)
    {
        if (panelUI != null && panelUI.IsOpen) RefreshPanel();
    }

    private void HandleDayAdvanced(DayDataSO _)
    {
        if (panelUI != null && panelUI.IsOpen) RefreshPanel();
    }
}
