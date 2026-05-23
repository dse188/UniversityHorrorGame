using UnityEngine;

public class Chore : MonoBehaviour, IInteractable
{
    [SerializeField] private TaskBoardManager taskBoardManager;
    [SerializeField] private DaySystem daySystem;

    [Tooltip("Must match exactly one task's title in the current day's RequiredTasks.")]
    [SerializeField] private string taskTitle;

    public string GetPromptText()
    {
        int index = FindTaskIndex();
        if (index < 0) return "Nothing to do here today";
        if (taskBoardManager.IsComplete(index)) return $"{taskTitle} — done";
        return $"Press E to: {taskTitle}";
    }

    public void Interact()
    {
        int index = FindTaskIndex();
        if (index < 0) return;
        if (taskBoardManager.IsComplete(index)) return;

        taskBoardManager.CompleteTask(index);
        if (daySystem != null) daySystem.ConsumeSlot();
    }

    private int FindTaskIndex()
    {
        if (taskBoardManager == null) return -1;
        var tasks = taskBoardManager.CurrentTasks;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].title == taskTitle) return i;
        }
        return -1;
    }
}
