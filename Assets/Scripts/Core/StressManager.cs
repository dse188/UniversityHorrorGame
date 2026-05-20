using System;
using UnityEngine;

public class StressManager : MonoBehaviour
{
    private int currentStress;

    public int CurrentStress => currentStress;

    public event Action<int> OnStressChanged;

    public void AddStress(int amount)         => Apply(currentStress + amount);
    public void RelieveStress(int amount)     => Apply(currentStress - amount);
    public void SetBaselineStress(int amount) => Apply(amount);

    private void Apply(int proposedValue)
    {
        int clamped = Mathf.Clamp(proposedValue, 0, 100);
        if (clamped == currentStress) return;

        currentStress = clamped;
        Debug.Log($"Stress changed to {currentStress}");
        OnStressChanged?.Invoke(currentStress);
    }
}
