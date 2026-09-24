using UnityEngine;
using Photon.Pun;
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

    public PlayerCharacter[] players;
    public Character boss;

    private int totalDamage = 0;

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
                photonView.RPC(nameof(SetPlayerActionFeedback), RpcTarget.All, actorNumber - 1, (int)PlayerAction.Attack);

                totalDamage += players[actorNumber - 1].damage;
            }
            else if (playerAction == PlayerAction.Defense)
            {
                photonView.RPC(nameof(SetPlayerActionFeedback), RpcTarget.All, actorNumber - 1, (int)PlayerAction.Defense);
            }
        }

        int targetViewID = boss.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(ApplyDamage),RpcTarget.All, targetViewID, totalDamage);

        playerActions.Clear();
        totalDamage = 0;
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


    [PunRPC]
    void ApplyDamage(int targetViewID, int damage)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);

        EnemyCharacter enemy = targetView.GetComponent<EnemyCharacter>();

        enemy.TakeDamage(damage);
    }

    [PunRPC]
    void SetPlayerActionFeedback(int playerIndex, int playerAction)
    {
        switch (playerAction)
        {
            case (int)PlayerAction.Attack:
                players[playerIndex].VirtualAttack();
                break;

            case (int)PlayerAction.Defense:
                players[playerIndex].VirtualDeffend();
                break;
        }        
    }
}
