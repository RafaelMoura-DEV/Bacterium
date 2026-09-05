using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public string nickName = "user";
    public string roomName = "PUCC";

    private void Start()
    {
        //Connect();
    }
    //-------------------------------------------------------------------------
    public void ChangeNickName(string newNick)
    {
        nickName = newNick;
    }

    //-------------------------------------------------------------------------
    public void Connect()
    {
        Debug.Log("Connecting to server...");
        //muda o nome do usuario antes de conectar
        PhotonNetwork.NickName = nickName;

        //muda a versao do jogo para nao mistura com a versao final
        PhotonNetwork.GameVersion = Application.version;

        //conecta no servidor do Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    //-------------------------------------------------------------------------
    //executa quando o ConnectUsingSettings consegue conectar ao servidor do Photon
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server!");
        Debug.Log("Connecting to lobby...");
        PhotonNetwork.JoinLobby();
    }

    //-------------------------------------------------------------------------
    //Quando o player entra no Lobby, pode entrar e sair de salas e tambem
    //vai receber a lista de salas abertas
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby!");
        //JoinRoom(roomName);
    }

    //-------------------------------------------------------------------------
    public void JoinRoom(string _roomName)
    {
        roomName = _roomName;
        RoomOptions roomOptions = new()
        {
            IsVisible = true,
            MaxPlayers = 20
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }

    //-------------------------------------------------------------------------
    //aqui voce esta dentro da sala e chegou a hora de se SPAWNAR na cena
    public override void OnJoinedRoom()
    {
        Debug.Log("Entered room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.Instantiate("Player", Random.insideUnitSphere * 2, Quaternion.identity);
    }

    //-------------------------------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room: " + newPlayer.NickName);
    }

    //-------------------------------------------------------------------------
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left room: " + otherPlayer.NickName);
    }
}
