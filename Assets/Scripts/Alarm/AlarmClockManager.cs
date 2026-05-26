using System;
using UnityEngine;

public enum AlarmChoice
{
    Unset,
    Early,
    Normal,
    Late
}

public class AlarmClockManager : MonoBehaviour
{
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private WakeUpButtonMashMiniGame minigame;

    [Header("Slot bonus by choice")]
    [SerializeField] private int earlyBonusSlots = 2;
    [SerializeField] private int normalBonusSlots = 1;
    [SerializeField] private int lateBonusSlots = 0;

    [Header("Oversleep penalty")]
    [SerializeField] private int stressPerSnooze = 5;
    [SerializeField] private int oversleepCap = 20;

    private AlarmChoice choice = AlarmChoice.Unset;
    private int snoozesThisWake;
    private int pendingOversleepPenalty;

    public AlarmChoice Choice => choice;
    public bool IsAlarmSet => choice != AlarmChoice.Unset;
    public int SnoozesThisWake => snoozesThisWake;
    public int PendingOversleepPenalty => pendingOversleepPenalty;

    public event Action<AlarmChoice> OnAlarmSet;
    public event Action OnAlarmFired;
    public event Action OnSnoozed;
    public event Action OnWokeUp;

    public void SetAlarm(AlarmChoice newChoice)
    {
        if (newChoice == AlarmChoice.Unset) return;
        choice = newChoice;
        OnAlarmSet?.Invoke(choice);
    }

    public void BeginSleep()
    {
        if (!IsAlarmSet) return;
        FireAlarm();
    }

    public void Snooze()
    {
        snoozesThisWake++;
        pendingOversleepPenalty = Mathf.Min(pendingOversleepPenalty + stressPerSnooze, oversleepCap);
        OnSnoozed?.Invoke();
        FireAlarm();
    }

    public void WakeUp()
    {
        OnWokeUp?.Invoke();
        if (daySystem != null)
        {
            daySystem.AdvanceDay();
        }
    }

    public int GetBonusSlotsForToday()
    {
        return choice switch
        {
            AlarmChoice.Early => earlyBonusSlots,
            AlarmChoice.Normal => normalBonusSlots,
            AlarmChoice.Late => lateBonusSlots,
            _ => 0
        };
    }

    public int GetOverSleepPenaltyFromPreviousDay()
    {
        return pendingOversleepPenalty;
    }

    private void FireAlarm()
    {
        OnAlarmFired?.Invoke();
        if (minigame != null)
        {
            minigame.Begin();
        }
    }

    private void OnEnable()
    {
        if (daySystem != null)
        {
            daySystem.OnDayAdvanced += HandleDayAdvanced;
        }

        if (minigame != null)
        {
            minigame.OnSucceeded += HandleMinigameSucceeded;
            minigame.OnFailed += HandleMinigameFailed;
        }
    }

    private void OnDisable()
    {
        if (daySystem != null)
        {
            daySystem.OnDayAdvanced -= HandleDayAdvanced;
        }

        if (minigame != null)
        {
            minigame.OnSucceeded -= HandleMinigameSucceeded;
            minigame.OnFailed -= HandleMinigameFailed;
        }
    }

    // Cleared HERE (not in WakeUp) so that DaySystem.EnterDay can still read
    // the penalty bonus during its before-OnDayAdvanced query window.
    private void HandleDayAdvanced(DayDataSO _)
    {
        choice = AlarmChoice.Unset;
        snoozesThisWake = 0;
        pendingOversleepPenalty = 0;
    }

    private void HandleMinigameSucceeded() => WakeUp();
    private void HandleMinigameFailed() => Snooze();
}
