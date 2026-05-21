using UnityEngine;
using UnityEngine.InputSystem;

public class TaskBoardManagerDebugHUD : MonoBehaviour
{
   [SerializeField] private TaskBoardManager taskBoardManager;

    private void Update()
    {
        if (taskBoardManager == null) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.f5Key.wasPressedThisFrame)
            CompleteFirstIncomplete();
    }

    private void CompleteFirstIncomplete()
    {
        for (int i = 0; i < taskBoardManager.TotalTasks; i++)
        {
            if (!taskBoardManager.IsComplete(i))
            {
                taskBoardManager.CompleteTask(i);
                return;
            }
        }
    }

    private void OnGUI()
    {
        if (taskBoardManager == null) return;

        int total = taskBoardManager.TotalTasks;
        int done = taskBoardManager.CompletedCount;

        GUI.Label(new Rect(10, 410, 400, 20), $"Tasks: {done} / {total} complete");

        var tasks = taskBoardManager.CurrentTasks;
        int y = 430;
        for (int i = 0; i < tasks.Count; i++)
        {
            string mark = taskBoardManager.IsComplete(i) ? "[x]" : "[ ]";
            GUI.Label(new Rect(20, y, 380, 20), $"{mark} {tasks[i].title}");
            y += 20;
        }

        int boxY = y + 10;
        GUI.Box(new Rect(10, boxY, 280, 55), "Controls");
        GUI.Label(new Rect(20, boxY + 25, 260, 20), "[F5]  Complete next task");
    }
}
