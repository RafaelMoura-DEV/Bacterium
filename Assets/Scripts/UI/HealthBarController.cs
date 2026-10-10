using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();

        SetMaxValue();
    }

    public void SetMaxValue()
    {
        healthBar.maxValue = character.maxHealth;
        healthBar.value = character.currentHealth;
    }

    public void UpdateHealthBar()
    {
        healthBar.value = character.currentHealth;
    }
}
