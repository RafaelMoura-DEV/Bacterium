using Photon.Pun;
using UnityEngine;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] GameObject restartButton;

    public void QuitGame()
    {
        PhotonConnection.Instance.LeaveRoom();
    }

    public void RestartGame()
    {
        if (PhotonNetwork.PlayerList.Length < 2) return;
        GameManager.Instance.LoadSceneServer("Game");
    }

    public void ToggleRestartButton(bool enable)
    {
        restartButton.SetActive(enable);
    }
}
