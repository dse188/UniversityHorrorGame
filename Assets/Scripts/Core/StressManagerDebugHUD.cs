using UnityEngine;
using UnityEngine.InputSystem;

public class StressManagerDebugHUD : MonoBehaviour
{
    [SerializeField] private StressManager stressManager;

    private void Update()
    {
        if (stressManager == null) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.f3Key.wasPressedThisFrame)
        {
            stressManager.AddStress(10);
        }
        if (kb.f4Key.wasPressedThisFrame)
        {
            stressManager.RelieveStress(10);
        }
    }

    private void OnGUI()
    {
        if (stressManager == null) return;

        int stress = stressManager.CurrentStress;
        GUI.Label(new Rect(10, 230, 400, 20), $"Stress: {stress} / 100");
        GUI.Label(new Rect(10, 250, 400, 20), $"Tier: {GetTierName(stress)}");

        GUI.Box(new Rect(10, 320, 280, 75), "Controls");
        GUI.Label(new Rect(20, 345, 260, 20), "[F3]  Add 10 stress");
        GUI.Label(new Rect(20, 365, 260, 20), "[F4]  Relieve 10 stress");
    }

    private string GetTierName(int stress)
    {
        if (stress < 25) return "1 - Subtle";
        if (stress < 50) return "2 - Moderate";
        if (stress < 75) return "3 - Active";
        return "4 - Confrontational";
    }
}
