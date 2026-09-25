using UnityEngine;

public class BossManager : MonoBehaviour
{
    public int currentBossIndex = 0;

    [SerializeField] GameObject[] bossesObj;
    [SerializeField] Sprite[] damagedBossesSprites;

    public Boss activeBoss;

    private void Start()
    {
        UpdateActiveBoss();
    }

    void UpdateActiveBoss()
    {
        Animator animator = bossesObj[currentBossIndex].GetComponent<Animator>();
        Animator animator1 = bossesObj[currentBossIndex].transform.GetChild(0).GetComponent<Animator>();
        SpriteRenderer spriteRenderer = bossesObj[currentBossIndex].GetComponent<SpriteRenderer>();

        Debug.Log(animator1.gameObject.name);

        activeBoss = new Boss(bossesObj[currentBossIndex], spriteRenderer, damagedBossesSprites[currentBossIndex], animator, animator1);
    }
}

public class Boss
{
    public Boss(GameObject newBossObj, SpriteRenderer newBossSpriteRenderer, Sprite newBossDamagedSprite, Animator newBossAnimator, Animator newBossSilhouetteAnimator)
    {
        bossObj = newBossObj;
        bossSpriteRenderer = newBossSpriteRenderer;
        damagedBossSprite = newBossDamagedSprite;
        bossAnimator = newBossAnimator;
        bossSilhouetteAnimator = newBossSilhouetteAnimator;
    }

    public GameObject bossObj;
    public SpriteRenderer bossSpriteRenderer;
    public Sprite damagedBossSprite;
    public Animator bossAnimator;
    public Animator bossSilhouetteAnimator;
}
