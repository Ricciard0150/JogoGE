using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private float lifeMax = 2f;

    private float currentLife;

    void Start()
    {
        currentLife = lifeMax;
    }

    public void Hitted(float damage, Vector3 shootPoint)
    {
        currentLife -= damage;

        if (bloodEffect != null)
        {
            GameObject blood = Instantiate(
                bloodEffect,
                shootPoint,
                Quaternion.LookRotation(shootPoint - transform.position)
            );

            blood.transform.SetParent(transform);
        }

        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}