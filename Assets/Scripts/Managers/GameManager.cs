using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : IPersistentSingleton<GameManager>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
