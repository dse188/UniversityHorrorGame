using System;
using System.Collections.Generic;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    [SerializeField] private List<DayDataSO> days = new();

    public event Action<DayDataSO> OnDayAdvanced;
    public event Action<int> OnSlotConsumed;
    public event Action OnSlotsExhausted;
    public event Action OnAllDaysComplete;

    private int currentDayIndex = -1;
    private int slotsRemaining;

    public DayDataSO CurrentDay => 
        (currentDayIndex >= 0 && currentDayIndex < days.Count) ? days[currentDayIndex] : null;

    public int SlotsRemaining => slotsRemaining;
    public int DayIndex => currentDayIndex;
    public bool isExhausted => slotsRemaining <= 0;

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
        currentDayIndex = index;
        slotsRemaining = days[index].SlotBudget;
        OnDayAdvanced?.Invoke(days[index]);
    }
}
