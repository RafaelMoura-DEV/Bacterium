using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class BossManager : MonoBehaviour
{
    BattleManager battleManager;

    PhotonView photonView;

    public int currentBossIndex = 0;
    public bool isFrozen = false;

    public enum BossAttacks { NoDefense, NoAttack, NoCards, EnergyDrain, ColetiveEnergyDrain,  LifeSteal, Berserk, Critical };
    private BossAttacks currentAttack = BossAttacks.Critical; // ataque critico e o default

    [SerializeField] GameObject[] bossesObj;
    [SerializeField] BossStats[] bossesStats;

    private EnemyCharacter bossCharacter;
    public Boss activeBoss;

    [SerializeField] PlayerCharacter playerOne;
    [SerializeField] PlayerCharacter playerTwo;

    private bool isFirstCall = true; // NAO QUESTIONE MINHA CABACISSE, OBRIGADO

    public static Action<BossAttacks> OnBossAttack;

    // ----------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        bossCharacter = GetComponent<EnemyCharacter>();
        photonView = GetComponent<PhotonView>();
        UpdateActiveBoss();
    }

    void OnEnable()
    {
        battleManager.OnEnemyTurnStart += ChooseAttack;
    }

    private void OnDisable()
    {
        battleManager.OnEnemyTurnStart -= ChooseAttack;
    }

    // ----------------------------------------------------------------------------------------------------- //

    void UpdateActiveBoss()
    {
        Animator animator = bossesObj[currentBossIndex].GetComponent<Animator>();
        Animator animator1 = bossesObj[currentBossIndex].transform.GetChild(0).GetComponent<Animator>();
        SpriteRenderer spriteRenderer = bossesObj[currentBossIndex].GetComponent<SpriteRenderer>();

        activeBoss = new Boss(bossesObj[currentBossIndex], spriteRenderer, animator, animator1);

        bossCharacter.characterName = bossesStats[currentBossIndex].bossName;
        bossCharacter.currentHealth = bossesStats[currentBossIndex].bossCurrentHealth;
        bossCharacter.maxHealth = bossesStats[currentBossIndex].bossMaxHealth;
        bossCharacter.damage = bossesStats[currentBossIndex].bossDamage;
        bossCharacter.resistence = bossesStats[currentBossIndex].bossResistence;
        bossCharacter.isAlive = true;
        if (!activeBoss.bossSpriteRenderer.gameObject.activeSelf) activeBoss.bossSpriteRenderer.gameObject.SetActive(true);
    }

    void ChooseAttack()
    {
        if(!PhotonNetwork.IsMasterClient) return;

        if (!isFrozen)
        {
            photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.Critical);
            Invoke(nameof(Attack), 1.0f); // so pro ataque nao ser instantaneo. Feeling melhor
        }

        else
        {
            battleManager.PassTurnFrozen();
            isFrozen = false;
        }
    }

    void Attack()
    {
        PlayerCharacter target = null;

        if (playerOne.isDefending && !playerTwo.isDefending) target = playerOne;

        else if (playerTwo.isDefending && !playerOne.isDefending) target = playerTwo;

        else if (playerTwo.isDefending && playerOne.isDefending || !playerTwo.isDefending && !playerOne.isDefending) // PROVISORIO PRO PLAYTEST 1
        {
            int x = UnityEngine.Random.Range(0, 2);

            if (x == 0) target = playerOne;
            else target = playerTwo;
        }

        if (!playerOne.isAlive) target = playerTwo;

        else if (!playerTwo.isAlive) target = playerOne;

        AttackInfo attackInfo = SetAttack(target);

        battleManager.AttackPlayer(target, attackInfo.damage, "INIMIGO ATACOU: " + target.characterName + attackInfo.interludePanelMessage, 30, attackInfo.effect); // TESTE DE LOGICA DO EFEITO

        int targetActorNumber = CheckWhoIsTheTarget(target);
        Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(targetActorNumber);
        photonView.RPC(nameof(CallAttackEvent), targetPlayer);
    }

    AttackInfo SetAttack(PlayerCharacter target) // caso futuramente eu queira usar os status dos players como parametro de decisao da IA do boss
    {
        int attackDamage = bossCharacter.damage;
        string attackMessage = "";
        Character.CharacterEffect attackEffect = Character.CharacterEffect.None;

        int randomAttackValue = SetRandomAttackValue();

        if (randomAttackValue != 0) // todos os boss tem ataque critico (0). Caso eu queira adicionar o ataque de quebrar maquinas, so colocar && para tambem pular o switch
        {
            switch (currentBossIndex) // ataques especificos do boss
            {
                case 0:

                    if (randomAttackValue == 1)
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.NoDefense);
                        attackMessage = ". ALVO NAO VAI PODER DEFENDER NO PROXIMO TURNO";
                        attackEffect = Character.CharacterEffect.CantDeffend;
                    }

                    else
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.NoAttack);
                        attackMessage = ". ALVO NAO VAI PODER ATACAR NO PROXIMO TURNO";
                        attackEffect = Character.CharacterEffect.CantAttack;
                    }

                    break;

                case 1:

                    if (randomAttackValue == 1)
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.NoCards);
                        attackMessage = ". ALVO NAO VAI PODER USAR CARTAS NO PROXIMO TURNO";
                        attackEffect = Character.CharacterEffect.CantUseCards;
                    }

                    else
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.ColetiveEnergyDrain);
                        attackMessage = ". AMBOS JOGADORES PERDERAM 1 CARGA DE ENERGIA";
                        attackEffect = Character.CharacterEffect.None;
                        photonView.RPC(nameof(DrainEnergy), RpcTarget.All, -1);
                    }

                    break;

                case 2:

                    if (randomAttackValue == 1 && bossCharacter.currentHealth > bossCharacter.maxHealth - 30) randomAttackValue = 2; 

                    if (randomAttackValue == 1)
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.LifeSteal);
                        attackMessage = ". INIMIGO SE CUROU";
                        attackEffect = Character.CharacterEffect.None;

                        battleManager.HealBoss(30);
                    }

                    else if (randomAttackValue == 2)
                    {
                        photonView.RPC(nameof(SyncCurrentAttack), RpcTarget.All, (int)BossAttacks.EnergyDrain);
                        attackMessage = ". ALVO PERDEU 2 CARGAS DE ENERGIA";
                        attackEffect = Character.CharacterEffect.None;

                        int targetActorNumber = CheckWhoIsTheTarget(target);
                        Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(targetActorNumber);
                        photonView.RPC(nameof(DrainEnergy), targetPlayer, -2);
                    }

                    break;
            }
        }

        else
        {
            attackDamage = Mathf.RoundToInt(bossCharacter.damage + (bossCharacter.damage / 2));
            attackMessage = ". ACERTO CRITICO!";
        }


        return new AttackInfo(attackDamage, attackMessage, attackEffect);
    }

    int SetRandomAttackValue() // futuramente substituir por aleatoriedade ponderada
    {
        return UnityEngine.Random.Range(0, 3); 
    }

    int CheckWhoIsTheTarget(PlayerCharacter target)
    {
        int playerActorNumber = 0;

        if (target == playerOne) playerActorNumber = 1;
        else playerActorNumber = 2;      
        
        return playerActorNumber;
    }

    public void Die()
    {
        activeBoss.bossAnimator.speed = 0.0f;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        if (isFirstCall) // seguinte, a master ta chamando 2 vezes essa porra e eu n sei como resolver sem fuder inteiramente o battlemanager XD
        {
            isFirstCall = false;

            yield return new WaitForSeconds(0.35f);

            Color color = activeBoss.bossSpriteRenderer.color;
            float startAlpha = color.a;
            float elapsed = 0f;

            while (elapsed < 1.0f)
            {
                elapsed += Time.deltaTime;

                float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / 1.0f);

                color.a = alpha;
                activeBoss.bossSpriteRenderer.color = color;

                yield return null;
            }

            color.a = 0f;
            activeBoss.bossSpriteRenderer.color = color;

            yield return new WaitForSeconds(2.0f); // sincronizar o tempo com a troca de cenario no battle manager

            isFirstCall = true;

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(SyncActiveBoss), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void SyncActiveBoss()
    {
        currentBossIndex++;

        if (currentBossIndex == bossesStats.Length)
        {
            battleManager.SetVictory();
        }

        else
        {
            UpdateActiveBoss();
            GetComponent<HealthBarController>().SetMaxValue();
            GetComponent<HealthBarController>().UpdateHealthBar();
        }         
    }

    [PunRPC]
    void SyncCurrentAttack(int attackIndex)
    {
        currentAttack = (BossAttacks)attackIndex;
    }

    [PunRPC]
    void CallAttackEvent()
    {
        OnBossAttack?.Invoke(currentAttack);
    }

    [PunRPC]
    void DrainEnergy(int amount)
    {
        FindAnyObjectByType<CardDeckController>().UpdateEnergy(amount);
    }
}

public class Boss
{
    public Boss(GameObject newBossObj, SpriteRenderer newBossSpriteRenderer, Animator newBossAnimator, Animator newBossSilhouetteAnimator)
    {
        bossObj = newBossObj;
        bossSpriteRenderer = newBossSpriteRenderer;
        bossAnimator = newBossAnimator;
        bossSilhouetteAnimator = newBossSilhouetteAnimator;
    }

    public GameObject bossObj;
    public SpriteRenderer bossSpriteRenderer;
    public Animator bossAnimator;
    public Animator bossSilhouetteAnimator;
}

[System.Serializable]

public class BossStats
{
    public string bossName;
    public int bossCurrentHealth;
    public int bossMaxHealth;
    public int bossResistence; // valor em %
    public int bossDamage;

    public BossManager.BossAttacks firstAttack;
    public BossManager.BossAttacks secondAttack;
}

public struct AttackInfo
{
    public AttackInfo(int attackDamage, string message, Character.CharacterEffect attackEffect)
    {
        damage = attackDamage;
        interludePanelMessage = message;
        effect = attackEffect;
    }

    public int damage;
    public string interludePanelMessage;
    public Character.CharacterEffect effect;
}
