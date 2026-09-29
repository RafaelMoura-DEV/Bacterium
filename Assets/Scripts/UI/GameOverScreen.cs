using Photon.Pun;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject defeatScreen;

    [SerializeField] TextMeshProUGUI gameOverText;

    [SerializeField] GameObject restartButton;

    public void SetGameOverScreen(bool victoryBool)
    {
        if (victoryBool)
        {
            victoryScreen.SetActive(true);
            gameOverText.text = "VITÓRIA";
        }

        else
        {
            defeatScreen.SetActive(true);
            gameOverText.text = "DERROTA";
        }
    }

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
