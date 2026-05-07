using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        // IGNORA INIMIGO
        if (other.CompareTag("Enemy"))
            return;

        // PROCURA PLAYER HEALTH NA CENA
        PlayerHealth health = FindObjectOfType<PlayerHealth>();

        // SE ACERTOU PLAYER
        if (other.CompareTag("Player"))
        {
            if (health != null)
            {
                health.TakeDamage(damage);

                Debug.Log("BALA DEU DANO");
            }
            else
            {
                Debug.LogError("PlayerHealth não encontrado!");
            }
        }

        Destroy(gameObject);
    }
}