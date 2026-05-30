using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StudyModeController : MonoBehaviour
{
    [SerializeField] private TaskBoardManager taskBoardManager;
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private GameModeController gameMode;
    [SerializeField] private StudyPanelUI panel;

    [Tooltip("Must match the title of the Study task in DayDataSO.")]
    [SerializeField] private string studyTaskTitle = "Study notes";

    [SerializeField, Min(1)] private int totalPages = 3;

    [SerializeField, TextArea(3, 8)]
    private string[] pageTexts =
    {
        "Notes — Chapter 1\n\nThe central tension of cognitive load theory is...",
        "Notes — Chapter 2\n\nBaddeley's working-memory model splits the central executive...",
        "Notes — Chapter 3\n\nTo retain material across spaced repetition windows...",
    };

    public event Action OnSessionStarted;
    public event Action<int> OnPageAdvanced;
    public event Action OnSessionCompleted;
    public event Action OnSessionAbandoned;

    private bool isActive;
    private int pageIndex;

    public bool IsActive => isActive;

    public bool HasStudyTaskToday => FindTaskIndex() >= 0;

    public bool IsTodayStudyDone
    {
        get
        {
            if (taskBoardManager == null) return false;
            int i = FindTaskIndex();
            return i >= 0 && taskBoardManager.IsComplete(i);
        }
    }

    public void Begin()
    {
        if (isActive) return;
        if (!HasStudyTaskToday) return;
        if (IsTodayStudyDone) return;

        isActive = true;
        pageIndex = 0;

        if (daySystem != null) daySystem.ConsumeSlot();
        if (gameMode != null) gameMode.Set(GameMode.ModalUI);
        if (panel != null)
        {
            panel.Show();
            panel.RenderPage(GetPageText(pageIndex), pageIndex + 1, totalPages);
        }

        OnSessionStarted?.Invoke();
    }

    private void Update()
    {
        if (!isActive) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.qKey.wasPressedThisFrame)
        {
            Abandon();
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            AdvancePage();
        }
    }

    private void AdvancePage()
    {
        pageIndex++;
        OnPageAdvanced?.Invoke(pageIndex);

        if (pageIndex >= totalPages)
        {
            Complete();
            return;
        }

        if (panel != null) panel.RenderPage(GetPageText(pageIndex), pageIndex + 1, totalPages);
    }

    private void Complete()
    {
        int index = FindTaskIndex();
        if (index >= 0 && taskBoardManager != null) taskBoardManager.CompleteTask(index);
        End();
        OnSessionCompleted?.Invoke();
    }

    private void Abandon()
    {
        End();
        OnSessionAbandoned?.Invoke();
    }

    // Slot is consumed in Begin; End only tears down modal state.
    private void End()
    {
        isActive = false;
        if (panel != null) panel.Hide();
        if (gameMode != null) gameMode.Set(GameMode.FreeRoam);
    }

    private string GetPageText(int i)
    {
        if (pageTexts == null || pageTexts.Length == 0) return $"Page {i + 1}";
        return pageTexts[Mathf.Clamp(i, 0, pageTexts.Length - 1)];
    }

    private int FindTaskIndex()
    {
        if (taskBoardManager == null) return -1;
        var tasks = taskBoardManager.CurrentTasks;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].title == studyTaskTitle) return i;
        }
        return -1;
    }
}
