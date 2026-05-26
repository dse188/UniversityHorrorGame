using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class WakeUpButtonMashMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField, Min(1)] private int requiredPresses = 8;
    [SerializeField, Min(0.5f)] private float timeWindow = 3f;

    public event Action OnSucceeded;
    public event Action OnFailed;

    private bool isActive;
    private int pressesLeft;
    private float timeLeft;

    public void Begin()
    {
        isActive = true;
        pressesLeft = requiredPresses;
        timeLeft = timeWindow;
        if (panel != null) panel.SetActive(true);
        UpdateLabel();
    }

    private void Update()
    {
        if (!isActive) return;

        timeLeft -= Time.deltaTime;

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            pressesLeft--;
            if (pressesLeft <= 0)
            {
                End();
                OnSucceeded?.Invoke();
                return;
            }
        }

        if (timeLeft <= 0f)
        {
            End();
            OnFailed?.Invoke();
            return;
        }

        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (label != null) label.text = $"WAKE UP - mash E x{pressesLeft}({timeLeft:0.0}s)";
    }

    private void End()
    {
        isActive = false;
        if (panel != null) panel.SetActive(false);
    }
}
