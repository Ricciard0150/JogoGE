using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [Header("Vida")]
    [SerializeField] private float lifeMax = 200f;

    [Header("Efeitos")]
    [SerializeField] private GameObject bloodEffect;

    private float currentLife;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        currentLife = lifeMax;

        startPosition = transform.position;
        startRotation = transform.rotation;
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
            gameObject.SetActive(false);
        }
    }

    public void ResetBoss()
    {
        currentLife = lifeMax;

        transform.position = startPosition;
        transform.rotation = startRotation;

        gameObject.SetActive(true);
    }
}