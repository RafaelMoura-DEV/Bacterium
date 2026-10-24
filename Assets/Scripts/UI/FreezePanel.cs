using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class FreezePanel : MonoBehaviour
{
    CardDeckController cardDeckController;
    BattleManager battleManager;

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

    private Color buttonsOriginalColor;

    // ------------------------------------------------------------------------------------------ //

    private void Awake()
    {
        cardDeckController = FindAnyObjectByType<CardDeckController>();
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
    }

    private void OnDisable()
    {
        battleManager.OnPlayerTurnEnd -= FreezeEnemy;
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
        if (cardDeckController.energy == 0 || cardDeckController.energy < chargeAmount) return;

        cardDeckController.UpdateEnergy(-chargeAmount);     

        photonView.RPC(nameof(UpdateTotalCharge), RpcTarget.All, chargeAmount);

        chargeAmount = 0;
        chargeAmountText.text = chargeAmount.ToString();
    }

    void FreezeEnemy()
    {
        if (totalCharge < 5) return;

        Debug.Log("CONGELANDOOO");
    }


    [PunRPC] 
    void UpdateTotalCharge(int charge)
    {
        totalCharge += charge;
        totalChargeText.text = totalCharge.ToString() + "/5";
    }
}
