using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;

public class CardDeckController : MonoBehaviour
{
    BattleManager battleManager;

    public int energy = 5;
    public bool isEnergyMachineBroken = false;
    [SerializeField] TextMeshProUGUI energyText;

    public Card[] cards;
    [HideInInspector] public Card selectedCard = null;

    [SerializeField] GameObject overlayPanel; // sprite
    [SerializeField] GameObject overlyText; // UI

    Color[] originalCardColors = new Color[3];

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void OnEnable()
    {
        battleManager.OnPlayerTurnEnd += HideOverlay;
    }

    private void OnDisable()
    {
        battleManager.OnPlayerTurnEnd -= HideOverlay;
    }

    // --------------------------------------------------------------------------- //

    public void SetSelectedCardFeedback(Card card)
    {
        if (card.isSelected)
        {
            overlayPanel.SetActive(true);
            overlyText.SetActive(true);

            ResetCardsFeedback();
            card.isSelected = true;
            card.gameObject.GetComponent<Image>().color = Color.green;

            selectedCard = card;
        }

        else
        {
            HideOverlay();
            ResetCardsFeedback();

            selectedCard = null;
        }
    }

    public void SetCardTarget()
    {
        if (selectedCard != null)
        {
            GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
            PlayerCharacter playerCharacter = clickedObject.GetComponent<PlayerCharacter>();

            if (!playerCharacter.isAlive) return;

            PhotonView targetView = playerCharacter.GetComponent<PhotonView>();

            int targetViewID = targetView.ViewID;

            battleManager.ApplyCardEffect(targetViewID, selectedCard.cardType, selectedCard.damageBuff, selectedCard.resistenceBuff, selectedCard.heal);

            ResetCardsFeedback();
            HideOverlay();

            UpdateEnergy(-selectedCard.energyCost);
            selectedCard = null;
        }
    }

    // --------------------------------------------------------------------------- //

    public void UpdateEnergy(int amount) // futuramente para as cartas de energia e debuffs de ataques tambem
    {
        energy += amount;
        if (energy < 0) energy = 0;
        if (energy > 5) energy = 5;
        energyText.text = energy.ToString();
    }

    public void SetCards(Card[] cardsInHand)
    {
        for (int i = 0; i < 3; i++)
        {
            cards[i] = cardsInHand[i];
        }

        SetOriginalColors();
    }

    void ResetCardsFeedback()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.GetComponent<Image>().color = originalCardColors[i];
            cards[i].isSelected = false;
        }
    }

    void HideOverlay() // esconder a forca ao passar de turno
    {
        overlayPanel.SetActive(false);
        overlyText.SetActive(false);
    }

    void SetOriginalColors()
    {      
        for (int i = 0; i < 3 ; i++)
        {
            originalCardColors[i] = cards[i].gameObject.GetComponent<Image>().color;
        }
    }
}
