using UnityEngine;
using UnityEngine.UI;

public class FreezePanel : MonoBehaviour
{
    [SerializeField] GameObject freezePanel;
    [SerializeField] GameObject descriptionObj;
    [SerializeField] GameObject chargeObj;

    [SerializeField] Image descriptionButtonImage;
    [SerializeField] Image chargeButtonImage;

    private Color buttonsOriginalColor;

    private void Start()
    {
        buttonsOriginalColor = descriptionButtonImage.color;
    }

    public void ToggleFreezePanel()
    {
        freezePanel.SetActive(!freezePanel.activeSelf);
    }

    public void ToggleSecondaryPanel(bool charge) // trocar entre descricao e recarga
    {
        chargeObj.SetActive(charge);
        descriptionObj.SetActive(!charge);

        if (charge)
        {
            descriptionButtonImage.color = new Color(descriptionButtonImage.color.r, descriptionButtonImage.color.g, descriptionButtonImage.color.b, 0.6f);
            chargeButtonImage.color = buttonsOriginalColor;
        }

        else
        {
            descriptionButtonImage.color = buttonsOriginalColor;
            chargeButtonImage.color = new Color(descriptionButtonImage.color.r, descriptionButtonImage.color.g, descriptionButtonImage.color.b, 0.6f);
        }
    }
}
