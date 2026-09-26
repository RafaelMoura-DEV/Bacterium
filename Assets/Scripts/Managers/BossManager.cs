using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    BattleManager battleManager;

    public int currentBossIndex = 0;

    [SerializeField] GameObject[] bossesObj;

    public Boss activeBoss;

    // ----------------------------------------------------------------------------------------------------- //

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void Start()
    {       
        UpdateActiveBoss();
    }

    void OnEnable()
    {
        //battleManager.OnEnemyTurnStart += ChooseAttack;
    }

    private void OnDisable()
    {
        //battleManager.OnEnemyTurnStart -= ChooseAttack;
    }

    // ----------------------------------------------------------------------------------------------------- //

    void UpdateActiveBoss()
    {
        Animator animator = bossesObj[currentBossIndex].GetComponent<Animator>();
        Animator animator1 = bossesObj[currentBossIndex].transform.GetChild(0).GetComponent<Animator>();
        SpriteRenderer spriteRenderer = bossesObj[currentBossIndex].GetComponent<SpriteRenderer>();

        activeBoss = new Boss(bossesObj[currentBossIndex], spriteRenderer, animator, animator1);
    }

    void ChooseAttack()
    {
        Invoke(nameof(Attack), 2.0f); // so pro ataque nao ser instantaneo. Feeling melhor
    }

    void Attack()
    {

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
