using UnityEngine;

public class EnemyCharacter : Character
{
    BossManager bossManager;

    private void Start()
    {
        bossManager = GetComponent<BossManager>();
    }

    public override void TakeDamage(int damage)
    {
        bossManager.activeBoss.bossSilhouetteAnimator.speed = 0.0f;
        bossManager.activeBoss.bossAnimator.SetTrigger("Damaged");
        
        base.TakeDamage(damage);
    }

    protected override void VirtualDie()
    {
        base.VirtualDie();
    }

    public override void VirtualAttack()
    {
        base.VirtualAttack();
    }
}
