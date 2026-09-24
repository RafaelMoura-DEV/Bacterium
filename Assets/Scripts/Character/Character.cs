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

        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        
    }

    public virtual void Attack(Character target, int damage)
    {
        target.TakeDamage(damage);
    }
}
