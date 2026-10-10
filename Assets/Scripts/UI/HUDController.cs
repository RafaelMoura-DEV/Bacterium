using UnityEngine;
using System.Collections;
using TMPro;

public class HUDController : MonoBehaviour
{
    private BattleManager battleManager;

    [SerializeField] RectTransform infoPanelTransform;
    private Coroutine currentMovementCoroutine;
    private TextMeshProUGUI[] infoPanelTexts;

    [SerializeField] GameObject cardsHolder;

    [SerializeField] TextMeshProUGUI firstPlayerName;
    [SerializeField] TextMeshProUGUI secondPlayerName;
    [SerializeField] TextMeshProUGUI localFeedbackText; // msg de morte, game over, debuffs, etc.

    [SerializeField] GameObject buttonFirstPlayer; // referencia do behaviour first player
    [SerializeField] GameObject buttonSecondPlayer; // referencia do behaviour second player

    [SerializeField] GameObject quitGamePanel;

    private bool isClientOnGame = true; // pra saber se o player esta morto ou impedido de jogar

    // ------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void Start()
    {
        infoPanelTexts = new TextMeshProUGUI[infoPanelTransform.childCount];

        for (int i = 0; i < infoPanelTransform.childCount; i++)
        {
            infoPanelTexts[i] = infoPanelTransform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    private void OnEnable()
    {
        battleManager.OnPlayerTurnStart += ShowCardsHolder;
        battleManager.OnPlayerTurnEnd += HidePanelOnEndTurn;
        battleManager.OnPlayerTurnEnd += HideCardsHolder;
    }

    private void OnDisable()
    {
        battleManager.OnPlayerTurnStart -= ShowCardsHolder;
        battleManager.OnPlayerTurnEnd -= HidePanelOnEndTurn;
        battleManager.OnPlayerTurnEnd -= HideCardsHolder;
    }

    // ------------------------------------------------------------------------------------------------- //

    public void SetPlayersNames(string firstName, string secondName)
    {
        firstPlayerName.text = firstName;
        secondPlayerName.text = secondName;

        Character character1 = buttonFirstPlayer.GetComponent<Character>();
        if (character1 != null ) character1.characterName = firstName;

        Character character2 = buttonSecondPlayer.GetComponent<Character>();
        if (character2 != null) character2.characterName = secondName;
    }

    public void ToggleInfoPanel(float speed)
    {
        if (battleManager.CurrentGameState != BattleManager.GameState.PlayerTurn) return;

        // Interrompe qualquer movimento anterior
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        currentMovementCoroutine = StartCoroutine(MovePanelCoroutine(speed));
    }

    private IEnumerator MovePanelCoroutine(float speed)
    {
        float targetX;

        if (speed > 0)
        {
            targetX = 165f;
            //UpdatePanelInfo();
        }
        else if (speed < 0)
        {
            targetX = -165f;
        }
        else
        {
            yield break;
        }

        while (Mathf.Abs(infoPanelTransform.anchoredPosition.x - targetX) > 0.01f)
        {
            Vector2 position = infoPanelTransform.anchoredPosition;

            position.x = Mathf.MoveTowards(position.x, targetX, Mathf.Abs(speed) * Time.deltaTime);

            infoPanelTransform.anchoredPosition = position;

            yield return null;
        }

        // Garante que termina exatamente no destino
        Vector2 finalPosition = infoPanelTransform.anchoredPosition;
        finalPosition.x = targetX;
        infoPanelTransform.anchoredPosition = finalPosition;

        currentMovementCoroutine = null;
    }

    public void UpdatePanelInfo(Character character)
    {
        infoPanelTexts[0].text = character.characterName;
        infoPanelTexts[1].text = "VIDA: " + character.currentHealth.ToString() + "/" + character.maxHealth.ToString();
        infoPanelTexts[2].text = "FORÇA: " + character.damage.ToString();
        infoPanelTexts[3].text = "RESISTÊNCIA: " + character.resistence + "%";

        switch (character.currentEffect)
        {
            case Character.CharacterEffect.None:
                infoPanelTexts[4].text = "";
                break;

            case Character.CharacterEffect.CantAttack:
                infoPanelTexts[4].text = "Esse jogador não pode atacar durante esse turno.";
                break;

            case Character.CharacterEffect.CantDeffend:
                infoPanelTexts[4].text = "Esse jogador não pode defender durante esse turno.";
                break;

            case Character.CharacterEffect.CantUseCards:
                infoPanelTexts[4].text = "Esse jogador não pode usar cartas durante esse turno.";
                break;
        }
    }

    public void DisplayDieFeedback()
    {
        isClientOnGame = false;
        localFeedbackText.text = "VOCÊ FOI MORTO...";
        HideCardsHolder();
    }

    void HidePanelOnEndTurn()
    {
        StopAllCoroutines();
        infoPanelTransform.anchoredPosition = new Vector2(-165.0f, 0);
    }

    void HideCardsHolder()
    {
        if (!isClientOnGame) return;
        cardsHolder.SetActive(false);
    }

    void ShowCardsHolder()
    {
        if (!isClientOnGame) return;
        cardsHolder.SetActive(true);
    }

    public void ToggleQuitPanel(bool openBool)
    {
        quitGamePanel.SetActive(openBool);  
    }

    public void QuitGame()
    {
        GameManager.Instance.LoadSceneLocal("Menu");
        PhotonConnection.Instance.LeaveRoom();
    }
}
