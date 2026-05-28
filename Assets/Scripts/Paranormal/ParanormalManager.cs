using System.Collections.Generic;
using UnityEngine;

public class ParanormalManager : MonoBehaviour
{
    [SerializeField] private StressManager stressManager;
    [SerializeField] private List<ParanormalEventBase> paranormalEvents = new();

    [Header("Director")]
    [SerializeField, Min(0)] private int minStressToFire = 5;
    [SerializeField, Min(0f)] private float globalCooldownSeconds = 20f;

    private float lastGlobalFireTime = float.NegativeInfinity;

    private void OnEnable()
    {
        if (stressManager != null) stressManager.OnStressChanged += HandleStressChanged;
    }

    private void OnDisable()
    {
        if (stressManager != null) stressManager.OnStressChanged -= HandleStressChanged;
    }

    private void HandleStressChanged(int newStress)
    {
        if (newStress < minStressToFire) return;
        if (Time.time - lastGlobalFireTime < globalCooldownSeconds) return;

        int currentTier = TierFromStress(newStress);
        ParanormalEventBase picked = PickEligible(currentTier);
        if (picked == null) return;

        lastGlobalFireTime = Time.time;
        picked.Fire();
    }

    private ParanormalEventBase PickEligible(int currentTier)
    {
        List<ParanormalEventBase> pool = new();
        foreach (ParanormalEventBase evt in paranormalEvents)
        {
            if (evt == null) continue;
            if (evt.Tier > currentTier) continue;
            if (!evt.IsAvailable) continue;
            pool.Add(evt);
        }
        if (pool.Count == 0) return null;
        return pool[Random.Range(0, pool.Count)];
    }

    private static int TierFromStress(int stress)
    {
        if (stress < 25) return 1;
        if (stress < 50) return 2;
        if (stress < 75) return 3;
        return 4;
    }
}
