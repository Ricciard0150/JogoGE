using UnityEngine;

public class Bulleta : MonoBehaviour
{
    [Header("Config")]
    public float damage = 10f;
    public float lifeTime = 5f;
    public float speed = 40f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // ?? FOR�A CONFIGURA��O CORRETA
        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction, float speedValue)
    {
        // ?? garante dire��o limpa (sem erro de inclina��o)
        Vector3 dir = direction.normalized;

        rb.linearVelocity = dir * speedValue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ?? dano via interface padr�o
        if (collision.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage((int)damage);
        }

        // ?? compatibilidade com sistema antigo
        if (collision.collider.TryGetComponent(out IShootable shootable))
        {
            shootable.Hitted((int)damage, collision.contacts[0].point);
        }

        Destroy(gameObject);
    }
}