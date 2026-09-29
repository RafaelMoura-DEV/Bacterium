using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class GameManager : IPersistentSingleton<GameManager>
{
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
        if (scene.name == "Game")
        {
            SetPlayersName();
        }

        if (scene.name == "Defeat")
        {
            DefeatScreen defeatScreen = FindAnyObjectByType<DefeatScreen>();

            if (PhotonNetwork.IsMasterClient) defeatScreen.ToggleRestartButton(true);
            else defeatScreen.ToggleRestartButton(false);
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
        LoadSceneServer("Defeat");
    }
}
