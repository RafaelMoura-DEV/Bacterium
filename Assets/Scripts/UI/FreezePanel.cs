using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FreezePanel : MonoBehaviour
{
    CardDeckController cardDeckController;
    BattleManager battleManager;
    BossManager bossManager;

    PhotonView photonView;

    [SerializeField] GameObject freezePanel;
    [SerializeField] GameObject descriptionObj;
    [SerializeField] GameObject chargeObj;

    [SerializeField] Image descriptionButtonImage;
    [SerializeField] Image chargeButtonImage;

    [SerializeField] TextMeshProUGUI totalChargeText;
    [SerializeField] TextMeshProUGUI chargeAmountText;

    public int totalCharge = 0; // total (global)
    private int chargeAmount = 0; // quantidade carregada pelo jogador
    private int turnCharge = 0; // limite de 2 por turno para cada jogador

    public bool freezeInThisBattle = false; // pra congelar 1 vez por turno

    private Color buttonsOriginalColor;

    // ------------------------------------------------------------------------------------------ //

    private void Awake()
    {
        cardDeckController = FindAnyObjectByType<CardDeckController>();
        bossManager = FindAnyObjectByType<BossManager>();
        photonView = GetComponent<PhotonView>();
        battleManager = GetComponent<BattleManager>();
    }

    private void Start()
    {
        buttonsOriginalColor = descriptionButtonImage.color;
    }

    private void OnEnable()
    {
        battleManager.OnPlayerTurnEnd += FreezeEnemy;
        battleManager.OnPlayerTurnEnd += HideFreezePanel;
    }

    private void OnDisable()
    {
        battleManager.OnPlayerTurnEnd -= FreezeEnemy;
        battleManager.OnPlayerTurnEnd -= HideFreezePanel;
    }

    // ------------------------------------------------------------------------------- //

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

    public void AddChageAmount(int amount)
    {
        chargeAmount += amount;

        if (chargeAmount == 3) chargeAmount = 2; // se quiser colocar sfx de denied dps
        if (chargeAmount == -1) chargeAmount = 0;

        chargeAmountText.text = chargeAmount.ToString();
    }

    public void ConfirmCharge()
    {
        if (cardDeckController.energy == 0 || cardDeckController.energy < chargeAmount || turnCharge == 2 || (chargeAmount == 2 && turnCharge == 1)) return;

        cardDeckController.UpdateEnergy(-chargeAmount);     

        photonView.RPC(nameof(UpdateTotalCharge), RpcTarget.All, chargeAmount);

        turnCharge += chargeAmount;

        chargeAmount = 0;
        chargeAmountText.text = chargeAmount.ToString();
    }

    void HideFreezePanel() // para esconder junto com o cardholder no final do turno
    {
        freezePanel.gameObject.SetActive(false);
    }

    void FreezeEnemy()
    {
        turnCharge = 0;

        if (freezeInThisBattle || totalCharge < 5) return;

        UpdateTotalCharge(-5);
        freezeInThisBattle = true;
        bossManager.isFrozen = true;
    }

    [PunRPC] 
    void UpdateTotalCharge(int charge)
    {
        totalCharge += charge;
        totalChargeText.text = totalCharge.ToString() + "/5";
    }
}
