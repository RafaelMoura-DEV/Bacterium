using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    CardDeckController cardDeckController;

    [System.Serializable]
    public class DeckEntry
    {
        [Tooltip("Prefab da carta que será adicionada ao baralho.")]
        public GameObject cardPrefab;

        [Min(0)]
        [Tooltip("Quantidade de cópias desta carta no baralho.")]
        public int quantity = 1;
    }

    [Header("CONFIGURAÇÃO DO BARALHO")]

    [SerializeField]
    private List<DeckEntry> deckConfiguration = new List<DeckEntry>();

    [Header("SLOTS DA MÃO")]

    [Tooltip("Arraste os três slots na ordem: 1, 2 e 3.")]
    [SerializeField]
    private Transform[] cardSlots = new Transform[3];

    [SerializeField]
    private int maxHandSize = 3;

    [Header("INICIALIZAÇÃO")]

    [SerializeField]
    private bool drawInitialHand = true;

    // Baralho de compra: contém referências aos prefabs.
    private List<GameObject> drawPile = new List<GameObject>();

    // Cada posição representa um slot fixo da mão.
    private GameObject[] hand = new GameObject[3];

    private bool isInitialized = false;


    private void Awake()
    {
        cardDeckController = GetComponent<CardDeckController>();
    }

    private void Start()
    {
        InitializeDeck();

        ShuffleDeck();

        isInitialized = true;

        if (drawInitialHand)
        {
            DrawUntilHandFull();
        }

        cardDeckController.SetCards(GetCardsInHand());
    }


    // Monta o baralho de acordo com as quantidades do Inspector.
    private void InitializeDeck()
    {
        drawPile.Clear();

        hand = new GameObject[3];

        if (cardSlots == null || cardSlots.Length != 3)
        {
            Debug.LogError(
                "DeckManager: configure exatamente três slots da mão."
            );

            return;
        }

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (cardSlots[i] == null)
            {
                Debug.LogError(
                    $"DeckManager: o slot {i + 1} não foi configurado."
                );

                return;
            }
        }

        foreach (DeckEntry entry in deckConfiguration)
        {
            if (entry.cardPrefab == null)
            {
                Debug.LogWarning(
                    "DeckManager: uma entrada não possui um prefab."
                );

                continue;
            }

            int quantity = Mathf.Max(0, entry.quantity);

            for (int i = 0; i < quantity; i++)
            {
                drawPile.Add(entry.cardPrefab);
            }
        }

        maxHandSize = Mathf.Clamp(maxHandSize, 1, 3);

        Debug.Log(
            $"Baralho inicializado com {drawPile.Count} cartas."
        );
    }


    // Embaralhamento Fisher-Yates.
    private void ShuffleDeck()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            GameObject temp = drawPile[i];

            drawPile[i] = drawPile[randomIndex];

            drawPile[randomIndex] = temp;
        }
    }


    // Compra cartas somente para os slots vazios.
    public void DrawUntilHandFull()
    {
        if (!isInitialized)
        {
            Debug.LogWarning(
                "DeckManager ainda não foi inicializado."
            );

            return;
        }

        for (int i = 0; i < maxHandSize; i++)
        {
            // Não altera slots que já possuem cartas.
            if (hand[i] != null)
            {
                continue;
            }

            // Não há cartas disponíveis para comprar.
            if (drawPile.Count == 0)
            {
                Debug.LogWarning(
                    "O baralho está vazio. Não foi possível completar a mão."
                );

                break;
            }

            DrawCardIntoSlot(i);
        }
    }


    // Compra uma carta e coloca-a em um slot específico.
    private void DrawCardIntoSlot(int slotIndex)
    {
        GameObject cardPrefab = drawPile[0];

        drawPile.RemoveAt(0);

        GameObject cardInstance = Instantiate(
            cardPrefab,
            cardSlots[slotIndex],
            false
        );

        // Posiciona a carta no centro do slot.
        RectTransform rect =
            cardInstance.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;

            rect.localRotation = Quaternion.identity;

            rect.localScale = Vector3.one;
        }
        else
        {
            cardInstance.transform.localPosition = Vector3.zero;

            cardInstance.transform.localRotation =
                Quaternion.identity;

            cardInstance.transform.localScale = Vector3.one;
        }

        // Registra o prefab original da carta.
        DeckCardInstance cardInfo =
            cardInstance.GetComponent<DeckCardInstance>();

        if (cardInfo == null)
        {
            cardInfo = cardInstance.AddComponent<DeckCardInstance>();
        }

        cardInfo.cardPrefab = cardPrefab;

        // Registra a carta na posição exata da mão.
        hand[slotIndex] = cardInstance;

        Debug.Log(
            $"Carta comprada para o slot {slotIndex + 1}: " +
            cardPrefab.name
        );
    }


    // Devolve uma carta utilizada ao baralho.
    public void ReturnUsedCard(GameObject cardInstance)
    {
        if (cardInstance == null)
        {
            Debug.LogWarning(
                "DeckManager: a referência da carta é nula."
            );

            return;
        }

        // Descobre o slot ocupado pela carta.
        int slotIndex = -1;

        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i] == cardInstance)
            {
                slotIndex = i;

                break;
            }
        }

        if (slotIndex == -1)
        {
            Debug.LogWarning(
                "DeckManager: a carta não foi encontrada na mão."
            );

            return;
        }

        DeckCardInstance cardInfo =
            cardInstance.GetComponent<DeckCardInstance>();

        if (cardInfo == null || cardInfo.cardPrefab == null)
        {
            Debug.LogError(
                "DeckManager: não foi possível identificar " +
                "o prefab original da carta."
            );

            return;
        }

        GameObject originalPrefab = cardInfo.cardPrefab;

        // Libera somente o slot ocupado pela carta utilizada.
        hand[slotIndex] = null;

        // Devolve a carta a uma posição aleatória do baralho.
        int randomIndex = Random.Range(0, drawPile.Count + 1);

        drawPile.Insert(randomIndex, originalPrefab);

        // Destrói somente a instância utilizada.
        Destroy(cardInstance);

        Debug.Log(
            $"Carta devolvida ao baralho. Slot {slotIndex + 1} liberado."
        );
    }


    // Chame este método no início de cada turno do jogador.
    public void StartPlayerTurn()
    {
        DrawUntilHandFull();
    }


    // Métodos auxiliares para consultar o estado do sistema.

    public int GetHandCount()
    {
        int count = 0;

        foreach (GameObject card in hand)
        {
            if (card != null)
            {
                count++;
            }
        }

        return count;
    }


    public int GetDrawPileCount()
    {
        return drawPile.Count;
    }


    public GameObject GetCardInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= hand.Length)
        {
            return null;
        }

        return hand[slotIndex];
    }

    Card[] GetCardsInHand()
    {
        Card[] cards = new Card[3];

        for (int i = 0; i < 3; i++)
        {
            cards[i] = hand[i].GetComponent<Card>();
        }

        return cards;
    }
}
