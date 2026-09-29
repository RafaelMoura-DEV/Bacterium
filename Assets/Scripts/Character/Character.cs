using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public int currentHealth;
    public int maxHealth;
    public int resistence; // valor em %
    public int damage;
    public bool isAlive = true; // se acabar o turno e a bool for falsa, chama o metodo Die

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        GetComponent<HealthBarController>().UpdateHealthBar();

        if (currentHealth <= 0) isAlive = false; // o metodo die so vai ser chamado ao finalizar o turno, nao quando a vida cair em 0
    }

    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        GetComponent<HealthBarController>().UpdateHealthBar();
    }

    protected virtual void VirtualDie()
    {
        
    }

    public virtual void VirtualAttack()
    {
        
    }

    public virtual void ResetAnimation()
    {
    
    }
}
