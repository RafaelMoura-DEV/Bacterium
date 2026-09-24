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
        this.characterSpriteRender.color = Color.red;

        base.VirtualAttack();
    }

    public void VirtualDeffend()
    {
        this.characterSpriteRender.color = Color.blue;
    }
}
