using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class WakeUpButtonMashMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameModeController gameMode;
    [SerializeField, Min(1)] private int requiredPresses = 8;
    [SerializeField, Min(0.5f)] private float timeWindow = 3f;
    

    public event Action OnSucceeded;
    public event Action OnFailed;

    private bool isActive;
    private int pressesLeft;
    private float timeLeft;
    private int beginFrame = -1;

    public void Begin()
    {
        isActive = true;
        pressesLeft = requiredPresses;
        timeLeft = timeWindow;
        beginFrame = Time.frameCount;
        if (panel != null) panel.SetActive(true);
        if (gameMode != null) gameMode.Set(GameMode.ModalUI);
        UpdateLabel();
    }

    private void Update()
    {
        if (!isActive) return;

        timeLeft -= Time.deltaTime;

        bool inputAllowed = Time.frameCount != beginFrame;
        if (inputAllowed && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
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
        if (gameMode != null) gameMode.Set(GameMode.FreeRoam);
    }
}
