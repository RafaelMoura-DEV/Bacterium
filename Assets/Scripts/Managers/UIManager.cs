using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    PhotonConnection photonConnection;

    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject roomScreen;

    [SerializeField] GameObject[] panels;

    [SerializeField] GameObject roomlistPanel;
    [SerializeField] GameObject newRoomPanel;

    [SerializeField] TMP_InputField nicknameInputField;
    [SerializeField] TMP_InputField newRoomInputField;

    [SerializeField] TextMeshProUGUI nicknameFeedbackText;
    [SerializeField] TextMeshProUGUI newRoomFeedbackText;

    private string nickname = "";

    // ------------------------------------------------------ //

    private void Start()
    {
        photonConnection = FindAnyObjectByType<PhotonConnection>();
    }

    // ------------------------------------------------------ //

    public void Play()
    {
        DeactivatePanels();
        panels[0].SetActive(true);
    }

    public void Login()
    {
        if (nickname.ToUpper() == "O RAFA É LINDO" || nickname.ToUpper() == "O RAFA E LINDO")
        {
            nicknameFeedbackText.text = "São seus olhos, chuchuzinho";
            nicknameInputField.text = "";
            return;
        }

        if (nickname != string.Empty)
        {
            menuScreen.SetActive(false);
            roomScreen.SetActive(true);

            photonConnection.ChangeNickName(nickname);
        }

        else
        {
            nicknameFeedbackText.text = "Insert a valid nickname!";
        }
    }

    public void OpenTutorial()
    {
        DeactivatePanels();
        panels[1].SetActive(true);
    }

    public void OpenExtras()
    {
        DeactivatePanels();
        panels[2].SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quitted...");
        Application.Quit();
    }

    public void DeactivatePanels()
    {
        for (int i = 0;  i < panels.Length; i++) panels[i].SetActive(false);
    }
}
