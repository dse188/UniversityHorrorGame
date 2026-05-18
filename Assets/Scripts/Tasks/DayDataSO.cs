using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct TaskDef
{
    public string title;

    [Tooltip("Stress added to the next day if this task is still incomplete at end of day.")]
    public int incompleteRollover;

    [Tooltip("Stress relieved after completing this task.")]
    public int stressRelief;
}


[CreateAssetMenu(fileName = "DayDataSO", menuName = "Scriptable Objects/DayDataSO")]
public class DayDataSO : ScriptableObject
{
    [Header("Identity")] 
    [SerializeField] private int dayNumber;
    [SerializeField] private string dayName;

    [Tooltip("Marks this day for distinct calendar display (e.g. Exam Day).")]
    [SerializeField] private bool isSpecialDay;

    // Only shown on the Calendar when isSpecialDay is true.
    [SerializeField] private string specialDayLabel;

    [Header("Day Budget")]
    [SerializeField, Min(1)] private int slotBudget = 6;

    [Tooltip("StressManager floor for this day. Stress is clamped UP to this value on day advance.")]
    [SerializeField, Range(0, 100)] private int stressFloor;

    [Header("Tasks")] 
    [SerializeField] private List<TaskDef> requiredTasks = new();

    public int DayNumber => dayNumber;
    public string DayName => dayName;
    public bool IsSpecialDay => isSpecialDay;
    public string SpecialDayLabel => specialDayLabel;
    public int SlotBudget => slotBudget;
    public int StressFloor => stressFloor;
    public IReadOnlyList<TaskDef> RequiredTasks => requiredTasks;

    //TODO: Need to add some kind of calculation for the rollover of stress to the next day based on incomplete tasks. 
    // Maybe we can just have a field in TaskDef for how much stress is added to the next day if it's incomplete, 
    // and then we can calculate the total stress added to the next day based on which tasks are incomplete at the end of the day. 
    // Eventually, we will also need to add the stress penalty for missing the alarm and oversleeping, but for now we assume player always wakes up on time.
    public int StressRolloverFromTasks(List<TaskDef> incompleteTasks)
    {
        int totalRollover = 0;
        foreach (var task in incompleteTasks)
        {
            totalRolloever += task.incompleteRollover;
        }
    }
}
