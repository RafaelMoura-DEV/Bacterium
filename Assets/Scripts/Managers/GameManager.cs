using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

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

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            //GameObject self = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            //self.name = PhotonConnection.Instance.nickName + " - PHOTONCONTROLLER";

            SetPlayersName();
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


}
