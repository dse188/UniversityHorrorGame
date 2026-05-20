using System;
using System.Collections.Generic;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    [SerializeField] private List<DayDataSO> days = new();
    [SerializeField] private StressManager stressManager;

    public event Action<DayDataSO> OnDayAdvanced;
    public event Action<int> OnSlotConsumed;
    public event Action OnSlotsExhausted;
    public event Action OnAllDaysComplete;

    private int currentDayIndex = -1;
    private int slotsRemaining;

    public DayDataSO CurrentDay => 
        (currentDayIndex >= 0 && currentDayIndex < days.Count) ? days[currentDayIndex] : null;

/**
    * public int SlotsRemaining() {
        get { return slotsRemaining; }
    }
*/
    public int SlotsRemaining => slotsRemaining;
    public int DayIndex => currentDayIndex;
    public bool IsExhausted => slotsRemaining <= 0;

    private void Start()
    {
        EnterDay(0);
    }

    public void ConsumeSlot()
    {
        if (slotsRemaining <= 0) return;
        slotsRemaining--;
        OnSlotConsumed?.Invoke(slotsRemaining);
        if (slotsRemaining == 0) OnSlotsExhausted?.Invoke();
    }

    public void AdvanceDay()
    {
        if (currentDayIndex + 1 >= days.Count)
        {
            OnAllDaysComplete?.Invoke();
            return;
        }
        EnterDay(currentDayIndex + 1);
    }

    private void EnterDay(int index)
    {
        // Capture BEFORE we change currentDayIndex - null on Day 1.
        DayDataSO previousDay = CurrentDay;

        currentDayIndex = index;
        slotsRemaining = days[index].SlotBudget;

        // Stress query must run BEFORE OnDayAdvanced so contributors (TaskBoardManager,
        // AlarmClockManager) still hold previous-day state when queried.
        ApplyDayStartStress(days[index], previousDay);

        OnDayAdvanced?.Invoke(days[index]);
    }

    private void ApplyDayStartStress(DayDataSO newDay, DayDataSO previousDay)
    {
        int startingStress = newDay.StressFloor 
                           + GetIncompleteRolloverFromPreviousDay(previousDay)
                           + GetOversleepingPenaltyFromPreviousDay();

        // TODO: replace with stressManager.SetBaseline(startingStress) once StressManger exists.
        Debug.Log($"[DaySystem] Day {newDay.DayNumber} start stress = {startingStress}");
    }

    // TODO: wired when TaskBoardManager exists.
    private int GetIncompleteRolloverFromPreviousDay(DayDataSO previousDay)
    {
        if (previousDay == null) return 0;
        return 0;
    }

    // TODO: wired when AlarmClockManager exists.
    private int GetOversleepingPenaltyFromPreviousDay()
    {
        return 0;
    }
}
