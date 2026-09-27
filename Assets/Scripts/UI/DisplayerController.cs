using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayerController : MonoBehaviour
{
    BattleManager battleManager;

    [SerializeField] TextMeshProUGUI displayerText;
    [SerializeField] GameObject[] actionButtons;

    // -------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void OnEnable()
    {
        battleManager.OnPlayerTurnStart += DeactivateButtons;
    }

    private void OnDisable()
    {
        battleManager.OnPlayerTurnStart -= DeactivateButtons;
    }

    // -------------------------------------------------------------------------------------------------- //

    public void UpdateDisplay(int informationIndex) // Attack = 0; Defense = 1; Fix = 2; Reroll = 3
    {
        switch (informationIndex)
        {
            case 0:
                displayerText.text = "Cause dano com base nos seus pontos de força.";
                break;

            case 1:
                displayerText.text = "Chame a atenção do inimigo e resista parte do dano.";
                break;

            case 2:
                displayerText.text = "...";
                break;

            case 3:
                displayerText.text = "Troque cartas da sua mão por novas. 1 uso por inimigo.";
                break;
        }
    }

    public void ClearDisplay()
    {
        displayerText.text = "";
    }

    public void SelectButton(int buttonIndex) // Attack = 0; Defense = 1; Fix = 2;
    {
        DeactivateButtons();

        actionButtons[buttonIndex].GetComponent<Button>().enabled = false;
        actionButtons[buttonIndex].GetComponent<Image>().color = Color.gray;
    }

    void DeactivateButtons()
    {
        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].GetComponent<Button>().enabled = true;
            actionButtons[i].GetComponent<Image>().color = Color.white;
        }
    }
}
