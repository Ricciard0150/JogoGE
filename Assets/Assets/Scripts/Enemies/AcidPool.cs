using UnityEngine;

public class AcidPool : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (!collision.TryGetComponent(out IDamageable damageable))
            return;

        damageable.Damage(10);
    }
}