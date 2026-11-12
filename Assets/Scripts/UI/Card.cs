using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardType { DamageBuff, ResistenceBuff, Heal, Energy }
    public CardType cardType;

    public int energyCost;
    public int damageBuff;
    public int resistenceBuff;
    public int heal;
    
    // -------------------------------------------------------------- //

    CardDeckController deckController;

    public bool isSelected = false;

    private void Awake()
    {
        deckController = FindAnyObjectByType<CardDeckController>();
    }

    public void ToggleSelectCard()
    {
        if (deckController.energy < energyCost)
        {
            Debug.LogWarning("ENERGIA INSUFICIENTE");
            // som de denied
        }

        else
        {
            isSelected = !isSelected;
            deckController.SetSelectedCardFeedback(this);
        }            
    }
}
