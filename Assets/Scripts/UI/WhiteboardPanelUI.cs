using UnityEngine;
using TMPro;

public class WhiteboardPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    public bool IsOpen => root != null && root.activeSelf;

    public void Show(string title, string body)
    {
        titleText.text = title;
        bodyText.text = body;
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}
