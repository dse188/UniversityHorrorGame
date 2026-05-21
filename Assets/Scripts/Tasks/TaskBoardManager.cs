using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskBoardManager : MonoBehaviour
{
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private StressManager stressManager;

    private DayDataSO currentDay;
    private List<bool> completion = new();

    public event Action<int> OnTasksCompleted;

    public IReadOnlyList<TaskDef> CurrentTasks =>
        currentDay != null ? currentDay.RequiredTasks : Array.Empty<TaskDef>();

    public int TotalTasks => currentDay != null ? currentDay.RequiredTasks.Count : 0;

    public int CompletedCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < completion.Count; i++)
                if (completion[i]) count++;
            return count;
        }
    }

    public bool IsComplete(int index)
    {
        return index >= 0 && index < completion.Count && completion[index];
    }

    public void CompleteTask(int index)
    {
        if (currentDay == null) return;
        if (index < 0 || index >= completion.Count) return;
        if (completion[index]) return;

        completion[index] = true;

        TaskDef def = currentDay.RequiredTasks[index];
        if (stressManager != null)
        {
            stressManager.RelieveStress(def.stressRelief);
        }

        OnTasksCompleted?.Invoke(CompletedCount);
    }

    public int GetIncompleteRolloverFor(DayDataSO previousDay)
    {
        // Defensive: only valid during the day-advance query window, when our
        // currentDay still equals the day being asked about.
        if (previousDay == null || currentDay != previousDay) return 0;

        int total = 0;
        for (int i = 0; i < currentDay.RequiredTasks.Count; i++)
        {
            if (!completion[i])
            {
                total += currentDay.RequiredTasks[i].incompleteRollover;
            }
        }
        return total;
    }

    private void OnEnable()
    {
        if (daySystem != null) daySystem.OnDayAdvanced += HandleDayAdvanced;
    }

    private void OnDisable()
    {
        if (daySystem != null) daySystem.OnDayAdvanced -= HandleDayAdvanced;
    }

    private void HandleDayAdvanced(DayDataSO newDay)
    {
        currentDay = newDay;
        completion.Clear();
        if (newDay == null) return;
        for (int i = 0; i < newDay.RequiredTasks.Count; i++)
            completion.Add(false);
    }
}
