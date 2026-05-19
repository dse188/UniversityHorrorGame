using UnityEngine;

public class StressManager : MonoBehaviour
{
    private int currentStress;

    public int CurrentStress => currentStress;

    public int AddStress(int amount)
    {
        currentStress += amount;
        Debug.Log($"Added {amount} stress. Current stress: {currentStress}");
        return currentStress;
    }

    public int RelieveStress(int amount)
    {
        currentStress = MathF.Max(0, currentStress - amount); // avoid negative stress
        Debug.Log($"Relieved {amount} stress. Current stress: {currentStress}");
        return currentStress;
    }

    public int SetBaselineStress(int amount)
    {
        currentStress = amount;
        Debug.Log($"Baseline stress set to {currentStress}");
        return currentStress;
    }
}
