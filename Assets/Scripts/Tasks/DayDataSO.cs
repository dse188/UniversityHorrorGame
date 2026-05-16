using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct TaskDef
{
    public string title;

    [Tooltip("Stress added if this task is still incomplete at end of day.")]
    public int stressPenalty;
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
}
