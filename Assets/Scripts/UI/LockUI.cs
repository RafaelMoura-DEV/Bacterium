using UnityEngine;
using UnityEngine.UI;

public class LockUI : MonoBehaviour
{
    BattleManager battleManager;

    [SerializeField] GameObject lockImagePrefab;
    [SerializeField] Transform abilityLockHolderTransform;
    [SerializeField] Vector4[] lockTransformValues;

    private GameObject instantiatedLock;

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
    }

    private void OnEnable()
    {
        BossManager.OnBossAttack += InstantiateLockPrefab;
        battleManager.OnPlayerTurnEnd += DestroyLock;
    }

    private void OnDisable()
    {
        BossManager.OnBossAttack -= InstantiateLockPrefab;
        battleManager.OnPlayerTurnEnd -= DestroyLock;
    }

    void InstantiateLockPrefab(BossManager.BossAttacks bossAttack)
    {
        if (bossAttack == BossManager.BossAttacks.EnergyDrain || bossAttack == BossManager.BossAttacks.Default 
        || bossAttack == BossManager.BossAttacks.Berserk || bossAttack == BossManager.BossAttacks.LifeSteal) return;

        RectTransform lockImage = Instantiate(lockImagePrefab, abilityLockHolderTransform).GetComponent<Image>().GetComponent<RectTransform>();
        instantiatedLock = lockImage.gameObject;

        switch (bossAttack)
        {
            case BossManager.BossAttacks.NoAttack:
                lockImage.localPosition = new Vector2(lockTransformValues[0].x, lockTransformValues[0].y);
                lockImage.sizeDelta = new Vector2(lockTransformValues[0].z, lockTransformValues[0].w);
                break;

            case BossManager.BossAttacks.NoDefense:
                lockImage.localPosition = new Vector2(lockTransformValues[1].x, lockTransformValues[1].y);
                lockImage.sizeDelta = new Vector2(lockTransformValues[1].z, lockTransformValues[1].w);
                break;

            case BossManager.BossAttacks.NoCards:
                lockImage.localPosition = new Vector2(lockTransformValues[2].x, lockTransformValues[2].y);
                lockImage.sizeDelta = new Vector2(lockTransformValues[2].z, lockTransformValues[2].w);
                break;
        }
    }

    void DestroyLock()
    {
        Destroy(instantiatedLock);
    }
}
