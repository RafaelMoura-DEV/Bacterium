using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class GameManager : IPersistentSingleton<GameManager>
{
    public bool playersWon = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void LoadSceneLocal(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneServer(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            playersWon = false;
        }

        if (scene.name == "Game")
        {
            SetPlayersName();
        }

        if (scene.name == "GameOver")
        {
            GameOverScreen gameOverScreen = FindAnyObjectByType<GameOverScreen>();

            if (playersWon) gameOverScreen.SetGameOverScreen(true);
            else gameOverScreen.SetGameOverScreen(false);

            if (PhotonNetwork.IsMasterClient) gameOverScreen.ToggleRestartButton(true);
            else gameOverScreen.ToggleRestartButton(false);
        }
    }

    void SetPlayersName()
    {
        HUDController hUDController = FindAnyObjectByType<HUDController>();

        if (hUDController != null)
        {
            Player[] players = PhotonNetwork.PlayerList;

            Player masterClient = PhotonNetwork.MasterClient;
            Player otherPlayer = null;

            foreach (Player player in players)
            {
                if (player != masterClient)
                {
                    otherPlayer = player;
                    break;
                }
            }

            hUDController.SetPlayersNames(masterClient.NickName, otherPlayer.NickName);
        }
    }

    public void SetDefeatCoroutine()
    {
        StartCoroutine(DefeatCoroutine());
    }

    IEnumerator DefeatCoroutine() // para a musica e toca um acorde sombrio, troca de sena, etc.
    {
        yield return new WaitForSeconds(3.0f);
        LoadSceneServer("GameOver");
    }
}
