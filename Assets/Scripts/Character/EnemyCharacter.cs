using UnityEngine;
using System.Collections;

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

        StartCoroutine(PlaySoundWithDelay(AudioManager.SFXs.Hit, 0.55f, 0.65f, 0.25f));

        base.TakeDamage(damage);
    }

    protected override IEnumerator PlaySoundWithDelay(AudioManager.SFXs sfx, float volume, float pitch, float delay)
    {
        return base.PlaySoundWithDelay(sfx, volume, pitch, delay);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
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
