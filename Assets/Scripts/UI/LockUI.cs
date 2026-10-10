using UnityEngine;

public class LockUI : MonoBehaviour
{
    [SerializeField] GameObject lockImagePrefab;

    private void OnEnable()
    {
        BossManager.OnBossAttack += InstantiateLockPrefab;
    }

    private void OnDisable()
    {
        BossManager.OnBossAttack -= InstantiateLockPrefab;
    }

    void InstantiateLockPrefab(BossManager.BossAttacks bossAttack)
    {
        Debug.LogWarning(bossAttack.ToString());
    }
}
