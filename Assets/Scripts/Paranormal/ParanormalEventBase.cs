using System;
using UnityEngine;

public abstract class ParanormalEventBase : MonoBehaviour
{
    [SerializeField, Min(1)] private int tier = 1;
    [SerializeField, Min(0)] private float cooldownSeconds = 30f;

    private float lastFiredTime = float.NegativeInfinity;

    public int Tier => tier;
    public bool IsAvailable => Time.time - lastFiredTime >= cooldownSeconds;
    public event Action OnFinished;

    public void Fire()
    {
        if (!IsAvailable) return;

        lastFiredTime = Time.time;
        //Debug.Log($"Firing paranormal event {name} of tier {tier}");
        OnFireRequested();
    }

    protected abstract void OnFireRequested();
    protected void RaiseFinished() => OnFinished?.Invoke();
}
