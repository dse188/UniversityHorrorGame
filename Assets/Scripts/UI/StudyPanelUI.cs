using TMPro;
using UnityEngine;

public class StudyPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI bodyLabel;
    [SerializeField] private TextMeshProUGUI pageCounterLabel;
    [SerializeField] private TextMeshProUGUI hintLabel;

    public void Show()
    {
        if (root != null) root.SetActive(true);
        if (hintLabel != null) hintLabel.text = "E — next page    Q/Esc — leave";
    }

    public void Hide()
    {
        if (root != null) root.SetActive(false);
    }

    public void RenderPage(string body, int pageNumber, int totalPages)
    {
        if (bodyLabel != null) bodyLabel.text = body;
        if (pageCounterLabel != null) pageCounterLabel.text = $"Page {pageNumber}/{totalPages}";
    }
}
