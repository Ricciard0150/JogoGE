using UnityEngine;

public class AcidProjectile : MonoBehaviour
{
    public GameObject acidPoolPrefab;

    public Transform alvo;
    public float velocidade = 5f;

    public float damage = 20f;


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