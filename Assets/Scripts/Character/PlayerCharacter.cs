using UnityEngine;

public class PlayerCharacter : Character
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void VirtualDie()
    {
        base.VirtualDie();
    }

    public override void VirtualAttack()
    {
        this.characterAnimator.SetTrigger("Attack");

        base.VirtualAttack();
    }

    public void VirtualDeffend()
    {
        this.characterAnimator.SetTrigger("Deffend");
    }
}
