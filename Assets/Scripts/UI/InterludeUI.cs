using TMPro;
using UnityEngine;

public class InterludeUI : MonoBehaviour
{
    [SerializeField] GameObject interludePanel;
    [SerializeField] TextMeshProUGUI interludeText;

    public void ToggleInterludePanel(bool showPanel, string message, int fontSize)
    {
        if (!showPanel) interludePanel.SetActive(false);

        else
        {
            interludePanel.SetActive(true);
            interludeText.text = message;
            interludeText.fontSize = fontSize;
        }
    }
}
