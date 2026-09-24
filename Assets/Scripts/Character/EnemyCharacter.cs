using UnityEngine;

public class EnemyCharacter : Character
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void Attack(Character target, int damage)
    {
        base.Attack(target, damage);
    }
}
