using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    PhotonView photonView;

    public float interludeTime = 5.0f;

    public enum GameState { PlayerTurn, BossTurn, Interlude, Victory, Defeat };
    public GameState CurrentGameState = GameState.PlayerTurn;

    public enum PlayerAction { Attack, Defense };

    private Dictionary<int, PlayerAction> playerActions = new Dictionary<int, PlayerAction>();

    public Action OnPlayerTurnStart;
    public Action OnPlayerTurnEnd;

    public Action OnEnemyTurnStart;
    public Action OnEnemyTurnEnd;

    public Action OnEnemyDefeated;

    public Action OnVictory;
    public Action OnDefeat;

    public PlayerCharacter[] players;
    public Character boss;

    // ------------------------------------------------------------------------------------------- //

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // --------------------------------- MÉTODOS COMUNS PÚBLICOS  --------------------------------- //

    // PLAYER //

    public void ChooseAttack()
    {
        ChooseAction(PlayerAction.Attack);
    }

    public void ChooseDefense()
    {
        ChooseAction(PlayerAction.Defense);
    }

    // BOSS //

    public void AttackPlayer(PlayerCharacter target, int damage)
    {
        int targetViewID = target.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(ApplyDamage), RpcTarget.All, targetViewID, damage);
        photonView.RPC(nameof(ResolveBossTurn), RpcTarget.All);
    }

    // --------------------------------- MÉTODOS COMUNS PRIVADOS  --------------------------------- //

    void ChooseAction(PlayerAction action)
    {
        if (CurrentGameState != GameState.PlayerTurn) return;

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        photonView.RPC(nameof(RegisterAction), PhotonNetwork.MasterClient, actorNumber, (int)action);
    }

    void ChangeGameState(GameState newState)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CurrentGameState = newState;

        photonView.RPC(nameof(SyncGameState), RpcTarget.All, (int)newState);
    }

    // -------------------------------- MÉTODOS RPC -------------------------------- //

    // PLAYER //

    [PunRPC]
    void RegisterAction(int actorNumber, int action)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        playerActions[actorNumber] = (PlayerAction)action;

        if (playerActions.Count == 2)
        {
            photonView.RPC(nameof(ResolvePlayerTurn), RpcTarget.All);
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

        Character target = targetView.GetComponent<Character>();

        target.TakeDamage(damage);
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

    [PunRPC] 
    void ResetCharactersAnimations()
    {
        players[0].ResetAnimation();
        players[1].ResetAnimation();
    }

    [PunRPC]
    void SetPlayerDefense(int playerIndex, bool isDefending)
    {
        players[playerIndex].isDefending = isDefending;    
    }

    [PunRPC]
    void ResolvePlayerTurn()
    {
        StartCoroutine(FeedBackCoroutine());
        StartCoroutine(PassTurnToBossCoroutine());
    }

    [PunRPC]
    void ResolveBossTurn()
    {
        StartCoroutine(PassTurnToPlayersCoroutine());
    }

    // ---------------------------- CORROTINAS ------------------------------------ //

    IEnumerator FeedBackCoroutine()
    {
        int targetViewID = boss.GetComponent<PhotonView>().ViewID;

        foreach (var action in playerActions)
        {
            int actorNumber = action.Key;
            PlayerAction playerAction = action.Value;

            if (playerAction == PlayerAction.Attack)
            {
                photonView.RPC(nameof(SetPlayerActionFeedback), RpcTarget.All, actorNumber - 1, (int)PlayerAction.Attack);
                photonView.RPC(nameof(ApplyDamage), RpcTarget.All, targetViewID, players[actorNumber - 1].damage);
            }
            else if (playerAction == PlayerAction.Defense)
            {
                photonView.RPC(nameof(SetPlayerDefense), RpcTarget.All, actorNumber -1, true);
                photonView.RPC(nameof(SetPlayerActionFeedback), RpcTarget.All, actorNumber - 1, (int)PlayerAction.Defense);
            }

            yield return new WaitForSeconds(1.5f); // tempo entre as acoes dos players, apenas para ficar com feeling melhor
        }
    
        playerActions.Clear();
    }

    IEnumerator PassTurnToBossCoroutine()
    {
        OnPlayerTurnEnd?.Invoke();
        ChangeGameState(GameState.Interlude);

        Debug.Log("INTERLUDE...");
        yield return new WaitForSeconds(interludeTime); // tempo entre os turnos para mensagens na hud e oq precisar

        Debug.Log("BOSS TURN!");
        ChangeGameState(GameState.BossTurn);
        OnEnemyTurnStart?.Invoke();
    }

    IEnumerator PassTurnToPlayersCoroutine()
    {
        OnEnemyTurnEnd?.Invoke();
        ChangeGameState(GameState.Interlude);

        Debug.Log("INTERLUDE...");
        yield return new WaitForSeconds(interludeTime); // tempo entre os turnos para mensagens na hud e oq precisar

        Debug.Log("PLAYERS TURN!");
        ChangeGameState(GameState.PlayerTurn);
        OnPlayerTurnStart?.Invoke();

        photonView.RPC(nameof(ResetCharactersAnimations), RpcTarget.All);

        photonView.RPC(nameof(SetPlayerDefense), RpcTarget.All, 0, false);
        photonView.RPC(nameof(SetPlayerDefense), RpcTarget.All, 1, false);
    }
}
