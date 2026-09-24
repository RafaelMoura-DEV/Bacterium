using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int currentHealth;
    public int maxHealth;
    public int resistence; // valor em %
    public int damage;

    public SpriteRenderer characterSpriteRender;
    public Animator characterAnimator;

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) VirtualDie();
    }

    protected virtual void VirtualDie()
    {
        
    }

    public virtual void VirtualAttack()
    {
        
    }
}
