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
        bossManager.activeBoss.bossAnimator.SetTrigger("Attack");

        base.VirtualAttack();
    }

    public override void ResetAnimation()
    {
        bossManager.activeBoss.bossAnimator.Play("Idle", 0, 0f);
        bossManager.activeBoss.bossSilhouetteAnimator.Play("Idle", 0, 0f);
        bossManager.activeBoss.bossSilhouetteAnimator.speed = 1.0f;
        base.ResetAnimation();
    }
}
