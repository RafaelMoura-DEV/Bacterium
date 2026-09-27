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
        battleManager.OnEnemyTurnStart += HidePanel;

        battleManager.OnPlayerTurnEnd += ShowPanel;
        battleManager.OnPlayerTurnEnd += ShowPanel;
    }

    private void OnDisable()
    {
        battleManager.OnEnemyTurnStart -= HidePanel;
        battleManager.OnEnemyTurnStart = HidePanel;

        battleManager.OnPlayerTurnEnd -= ShowPanel;
        battleManager.OnPlayerTurnEnd -= ShowPanel;
    }

    // -------------------------------------------------------------- //


    // N SEI PQ MAS N TA CHAMANDO QUANDO O BOSS PASSA O TURNO. E TA CHAMANDO 2 VEZES

    void ShowPanel()
    {
        //Debug.LogWarning("SHOW");
        interludePanel.SetActive(true);
    }

    void HidePanel()
    {
        //Debug.LogWarning("HIDE");
        interludePanel.SetActive(false);
    }
}
