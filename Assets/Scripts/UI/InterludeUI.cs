using TMPro;
using UnityEngine;

public class InterludeUI : MonoBehaviour
{
    BattleManager battleManager;

    [SerializeField] GameObject interludePanel;
    [SerializeField] TextMeshProUGUI interludeText;

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void OnEnable()
    {
        battleManager.OnEnemyTurnStart += HidePanel;
        battleManager.OnPlayerTurnStart += HidePanel;

        battleManager.OnEnemyTurnEnd += ShowPanelPlayers;
        battleManager.OnPlayerTurnEnd += ShowPanelBoss;
    }

    private void OnDisable()
    {
        battleManager.OnEnemyTurnStart -= HidePanel;
        battleManager.OnPlayerTurnStart -= HidePanel;

        battleManager.OnEnemyTurnEnd -= ShowPanelPlayers;
        battleManager.OnPlayerTurnEnd -= ShowPanelBoss;
    }

    // -------------------------------------------------------------- //

    void ShowPanelBoss()
    {
        interludePanel.SetActive(true);
        interludeText.text = "TURNO DO INIMIGO";
    }

    void ShowPanelPlayers()
    {
        interludePanel.SetActive(true);
        interludeText.text = "O INIMIGO ATACOU"; // depois colocar o feedback definitivo
    }

    void HidePanel()
    {
        interludePanel.SetActive(false);
    }  
}
