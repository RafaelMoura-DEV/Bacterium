using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    BattleManager battleManager;

    public int currentBossIndex = 0;

    [SerializeField] GameObject[] bossesObj;

    private EnemyCharacter bossCharacter;
    public Boss activeBoss;

    [SerializeField] PlayerCharacter playerOne;
    [SerializeField] PlayerCharacter playerTwo;

    // ----------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        bossCharacter = GetComponent<EnemyCharacter>();

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

        if (!activeBoss.bossSpriteRenderer.gameObject.activeSelf) activeBoss.bossSpriteRenderer.gameObject.SetActive(true);
    }

    void ChooseAttack()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        Invoke(nameof(Attack), 1.0f); // so pro ataque nao ser instantaneo. Feeling melhor
    }

    void Attack()
    {
        PlayerCharacter target = null;

        if (playerOne.isDefending && !playerTwo.isDefending) target = playerOne;

        else if (playerTwo.isDefending && !playerOne.isDefending) target = playerTwo;

        else if (playerTwo.isDefending && playerOne.isDefending || !playerTwo.isDefending && !playerOne.isDefending) // PROVISORIO PRO PLAYTEST 1
        {
            int x = Random.Range(0, 2);

            if (x == 0) target = playerOne;
            else target = playerTwo;
        }

        if (!playerOne.isAlive) target = playerTwo;

        else if (!playerTwo.isAlive) target = playerOne;

        battleManager.AttackPlayer(target, bossCharacter.damage);
    }

    public void Die()
    {
        activeBoss.bossAnimator.speed = 0.0f;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
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

        yield return new WaitForSeconds(2.0f);

        //UpdateActiveBoss();
         
        battleManager.SetVictory(); // PLAYTEST 1 PROVISORIO
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
