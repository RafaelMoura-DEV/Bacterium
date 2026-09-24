using UnityEngine;

public class PlayerCharacter : Character
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
        this.characterSpriteRender.color = Color.red;

        base.Attack(target, damage);
    }
}
