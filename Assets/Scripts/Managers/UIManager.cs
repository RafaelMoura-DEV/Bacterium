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
    [SerializeField] GameObject waitingPanel;

    [SerializeField] TMP_InputField nicknameInputField;
    [SerializeField] TMP_InputField newRoomInputField;

    [SerializeField] TextMeshProUGUI nicknameFeedbackText;
    [SerializeField] TextMeshProUGUI newRoomFeedbackText;
    [SerializeField] TextMeshProUGUI joinButtonText;

    [SerializeField] GameObject quitButton;

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
        string nickname = nicknameInputField.text;

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
            photonConnection.Connect();
        }

        else
        {
            nicknameFeedbackText.text = "Insira um nome válido!";
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

    public void ToggleRoomList()
    {
        if (!roomlistPanel.activeSelf)
        {
            roomlistPanel.SetActive(true);
            joinButtonText.text = "ENTRAR          -";
            quitButton.transform.localPosition = new Vector3(quitButton.transform.localPosition.x, -470, quitButton.transform.localPosition.z);
        }

        else
        {
            roomlistPanel.SetActive(false);
            joinButtonText.text = "ENTRAR          +";
            quitButton.transform.localPosition = new Vector3(quitButton.transform.localPosition.x, -48.0f, quitButton.transform.localPosition.z);
        }
    }

    public void OpenNewRoomPanel()
    {
        newRoomPanel.SetActive(true);
    }

    public void CreateNewRoom()
    {
        if (newRoomInputField.text != string.Empty)
        {
            photonConnection.JoinRoom(newRoomInputField.text);
        }

        else
        {
            newRoomFeedbackText.text = "Insira um nome válido!";
        }
    }

    public void OpenWaitingPanel(bool isMasterClient)
    {
        newRoomPanel.SetActive(false);
        waitingPanel.SetActive(true);

        if (isMasterClient)
        {
            waitingPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = photonConnection.nickName;
        }

        else
        {
            string masterClientName = photonConnection.GetMasterClientName();
            waitingPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = masterClientName;
            waitingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = photonConnection.nickName;
        }
    }

    public void UpdateWaitingPanel(string secondPlayerName)
    {
        waitingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = secondPlayerName;
        waitingPanel.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        menuScreen.SetActive(true);
        roomScreen.SetActive(false);
    }

    public void DeactivatePanels()
    {
        for (int i = 0;  i < panels.Length; i++) panels[i].SetActive(false);
    }
}
