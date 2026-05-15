using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;

    private void Awake()
    {
        Hide();
    }

    public void Show(string text)
    {
        promptText.text = text;
        promptRoot.SetActive(true);
    }

    public void Hide()
    {
        promptRoot.SetActive(false);
    }
}
