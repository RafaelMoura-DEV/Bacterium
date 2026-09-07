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
            GameObject self = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            self.name = PhotonConnection.Instance.nickName;
        }
    }
}
