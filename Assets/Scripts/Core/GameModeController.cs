using System;
using UnityEngine;

public enum GameMode
{
    FreeRoam,
    ModalUI
}

public class GameModeController : MonoBehaviour
{
    private GameMode current = GameMode.FreeRoam;

    public GameMode Current => current;
    public event Action<GameMode> OnModeChanged;

    public void Set(GameMode mode)
    {
        if (mode == current) return;
        current = mode;
        OnModeChanged?.Invoke(current);
    }
}
