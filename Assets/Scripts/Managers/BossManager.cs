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
    }

    private void Start()
    {       
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
    }

    void ChooseAttack()
    {
        if(!PhotonNetwork.IsMasterClient) return;
        Invoke(nameof(Attack), 1.5f); // so pro ataque nao ser instantaneo. Feeling melhor
    }

    void Attack()
    {
        battleManager.AttackPlayer(playerTwo, bossCharacter.damage);
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
