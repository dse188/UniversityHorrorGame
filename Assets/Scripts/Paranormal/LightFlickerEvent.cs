using System.Collections;
using UnityEngine;

public class LightFlickerEvent : ParanormalEventBase
{
    [SerializeField] private Light targetLight;
    [SerializeField, Min(1)] private int flickerCount = 4;
    [SerializeField, Min(0.02f)] private float flickerInterval = 0.08f;

    protected override void OnFireRequested()
    {
        if (targetLight == null)
        {
            RaiseFinished();
            return;
        }
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        // Preserve the player's prior light state so the flicker doesn't
        // permanently flip a switch they intentionally toggled off.
        bool originalState = targetLight.enabled;
        for (int i = 0; i < flickerCount; i++)
        {
            targetLight.enabled = !targetLight.enabled;
            yield return new WaitForSeconds(flickerInterval);
        }
        targetLight.enabled = originalState;
        RaiseFinished();
    }
}
