using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        // PROCURA VIDA NO PLAYER
        PlayerHealth health =
            collision.collider.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage);

            Debug.Log("BALA DEU DANO");
        }

        Destroy(gameObject);
    }
}