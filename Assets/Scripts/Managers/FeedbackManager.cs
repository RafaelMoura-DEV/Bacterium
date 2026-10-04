using UnityEngine;
using Photon.Pun;

public class FeedbackManager : MonoBehaviour // controla todo feedback RPC, como painel de interludio, particulas, VFXs, etc.
{
    PhotonView photonView;

    InterludeUI interludeUI;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        interludeUI = FindAnyObjectByType<InterludeUI>();
    }

    // -------------------------- METODOS PUBLICOS -------------------------- //

    public void SetInterludePanel(bool showPanel, string message, int fontSize)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        photonView.RPC(nameof(SyncInterludePanel), RpcTarget.All, showPanel, message, fontSize);
    }

    // ---------------------------- METODOS RPC ---------------------------- //

    [PunRPC]
    void SyncInterludePanel(bool showPanel, string message, int fontSize)
    {
        interludeUI.ToggleInterludePanel(showPanel, message, fontSize);
    }
}
