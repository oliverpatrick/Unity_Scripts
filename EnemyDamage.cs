using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{
    public float health = 100f;
    public float currentHealth;

    public PlayerHealth playerHealth;

    public void Start()
    {
        currentHealth = health;
        playerHealth.SetMaxHealth(health);
    }
    public void TakeDamage(float amount)
    {
        currentHealth = health -= amount;
        playerHealth.SetHealth(currentHealth);

        if (health <= 0f)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
