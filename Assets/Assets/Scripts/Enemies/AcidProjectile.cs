using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public GameObject acidPoolPrefab;

    public Transform alvo;
    public float velocidade = 5f;

    public float damage = 20f;

    void Update()
    {
        if (alvo != null)
        {
            Vector3 direcao = (alvo.position - transform.position).normalized;

            transform.position += direcao * velocidade * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(
            acidPoolPrefab,
            transform.position,
            Quaternion.Euler(0, 180, 0)
        );

        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(40);
            print("collided");
        }

    }
}