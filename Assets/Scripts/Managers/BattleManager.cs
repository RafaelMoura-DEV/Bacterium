using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;

public class BattleManager : MonoBehaviour
{
    PhotonView photonView;

    public enum GameState { PlayerTurn, BossTurn, Interlude, Victory, Defeat };
    public GameState CurrentGameState = GameState.PlayerTurn;

    public enum PlayerAction { Attack, Defense };

    private Dictionary<int, PlayerAction> playerActions = new Dictionary<int, PlayerAction>();

    public Action OnPlayerTurnEnd;
    public Action OnPlayerTurnStart;
    public Action OnEnemyDefeated;
    public Action OnVictory;
    public Action OnDefeat;

    public Character[] players;
    public Character boss;

    // ----------------------------------------------------------------------------------- //

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // --------------------------------- MÉTODOS COMUNS PÚBLICOS  --------------------------------- //

    public void ChooseAttack()
    {
        ChooseAction(PlayerAction.Attack);
    }

    public void ChooseDefense()
    {
        ChooseAction(PlayerAction.Defense);
    }

    // --------------------------------- MÉTODOS COMUNS PRIVADOS  --------------------------------- //

    void ChooseAction(PlayerAction action)
    {
        if (CurrentGameState != GameState.PlayerTurn) return;

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        photonView.RPC(nameof(RegisterAction), PhotonNetwork.MasterClient, actorNumber, (int)action);
    }

    void ResolvePlayerTurn()
    {
        Debug.Log("TURNO DOS JOGADORES ENCERRADO!");

        foreach (var action in playerActions)
        {
            int actorNumber = action.Key;
            PlayerAction playerAction = action.Value;

            if (playerAction == PlayerAction.Attack)
            {
                //Debug.Log("Player " + actorNumber + " atacou!");
                players[actorNumber - 1].Attack(boss, 10);
            }
            else if (playerAction == PlayerAction.Defense)
            {
                Debug.Log("Player " + actorNumber + " defendeu!");
            }
        }

        playerActions.Clear();
        ChangeGameState(GameState.BossTurn);
        OnPlayerTurnEnd?.Invoke();
    }

    void ChangeGameState(GameState newState)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CurrentGameState = newState;

        photonView.RPC(nameof(SyncGameState), RpcTarget.All, (int)newState);
    }

    // -------------------------------- MÉTODOS EXCLUSIVOS DO MASTER -------------------------------- //

    [PunRPC]
    void RegisterAction(int actorNumber, int action)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        playerActions[actorNumber] = (PlayerAction)action;

        if (playerActions.Count == 2)
        {
            ResolvePlayerTurn();
        }
    }  

    [PunRPC]
    void SyncGameState(int state)
    {
        CurrentGameState = (GameState)state;
    }  
}
