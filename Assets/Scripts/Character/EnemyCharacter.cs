using UnityEngine;

public class EnemyCharacter : Character
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
        base.VirtualAttack();
    }
}
