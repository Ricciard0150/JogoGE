using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    [Header("Blood Effect")]
    public GameObject bloodEffect;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHealth -= damage;

        Debug.Log("VIDA INIMIGO: " + currentHealth);

        // Spawn do sangue
        if (bloodEffect != null)
        {
            GameObject blood = Instantiate(
                bloodEffect,
                hitPoint,
                Quaternion.LookRotation(hitNormal)
            );

            Destroy(blood, 1f);
        }

        // Morte
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}