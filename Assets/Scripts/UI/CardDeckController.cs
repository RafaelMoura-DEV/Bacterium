using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDeckController : MonoBehaviour
{
    BattleManager battleManager;

    public int energy = 5;

    public Card[] cards; // futuramente, toda a logica do deck de cartas estara aqui
    public Card selectedCard = null;

    [SerializeField] GameObject overlayPanel; // sprite
    [SerializeField] GameObject overlyText; // UI

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

            switch (selectedCard.cardType)
            {
                case Card.CardType.DamageBuff:

                    int totalDamage = playerCharacter.damage;
                    totalDamage = Mathf.RoundToInt(totalDamage * (1f + selectedCard.damageBuff / 100f));

                    playerCharacter.damage = totalDamage;
                    break;

                case Card.CardType.ResistenceBuff:
                    playerCharacter.resistence += selectedCard.resistenceBuff;
                    break;

                case Card.CardType.Heal:
                    playerCharacter.currentHealth += selectedCard.heal;
                    break;
            }

            ResetCardsFeedback();
            HideOverlay();
        }
    }

    // --------------------------------------------------------------------------- //

    void ResetCardsFeedback()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].gameObject.GetComponent<Image>().color = Color.white;
            cards[i].isSelected = false;
        }
    }

    void HideOverlay() // esconder a forca ao passar de turno
    {
        overlayPanel.SetActive(false);
        overlyText.SetActive(false);
    }
}
