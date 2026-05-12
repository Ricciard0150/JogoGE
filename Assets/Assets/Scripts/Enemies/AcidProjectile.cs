using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public GameObject acidPoolPrefab;

    public float damage = 20f;

    private bool collided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collided) return;

        collided = true;

        // Se bater no chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(
                acidPoolPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Se acertar algo que toma dano
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(10);
        }

        // Destrói o projétil
        Destroy(gameObject);
    }
}