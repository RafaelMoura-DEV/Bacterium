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

    public void UpdateDisplay(int informationIndex) // Attack = 0; Defense = 1; Charge = 2; Reroll = 3; Freeze = 4; CartaDano = 5; CartaResistencia = 6; CartaCura = 7;
    {
        switch (informationIndex)
        {
            case 0:
                displayerText.text = "Cause dano com base nos seus pontos de forca.";
                break;

            case 1:
                displayerText.text = "Chame a atencao do inimigo e resista parte do dano.";
                break;

            case 2:
                displayerText.text = "Recarregue 2 cargas de sua energia.";
                break;

            case 3:
                displayerText.text = "Troque cartas da sua mao por novas. 1 uso por inimigo.";
                break;

            case 4:
                displayerText.text = "Recarregue a arma de gelo gastando sua energia.";
                break;

            case 5:
                displayerText.text = "Escolha um jogador para aumentar sua forca nesse turno.";
                break;

            case 6:
                displayerText.text = "Escolha um jogador para aumentar sua resistencia nesse turno.";
                break;

            case 7:
                displayerText.text = "Escolha um jogador e cure sua vida. Nao adiciona vida extra.";
                break;
        }
    }

    public void ClearDisplay()
    {
        displayerText.text = "";
    }

    public void SelectButton(int buttonIndex) // Attack = 0; Defense = 1; Charge = 2;
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
