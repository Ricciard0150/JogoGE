using UnityEngine;


public class AcidProjectile : MonoBehaviour
{
    public GameObject acidPoolPrefab;
    //public NavMeshAgent agent;

    //public bool chase;

    public float damage = 20f;
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(
            acidPoolPrefab,
            transform.position,
            Quaternion.identity
        );

        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(40);
            print("collided");
        }
    }
}
