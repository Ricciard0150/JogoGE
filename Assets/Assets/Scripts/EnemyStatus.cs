using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [Header("Vida")]
    [SerializeField] private float lifeMax = 200f;

    [Header("Efeitos")]
    [SerializeField] private GameObject bloodEffect;

    [Header("Refs")]
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;

    private float currentLife;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool isDead = false;

    void Start()
    {
        currentLife = lifeMax;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Hitted(float damage, Vector3 shootPoint)
    {
        if (isDead) return;

        currentLife -= damage;

        // 💉 sangue
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
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 🔥 parar IA
        if (agent != null)
            agent.enabled = false;

        // 🔥 parar física
        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        // 🔥 animação de morte
        if (anim != null)
            anim.SetTrigger("Die");

        // 🔥 desativar colisão opcional
        GetComponent<Collider>().enabled = false;

        // 🔥 destrói depois da animação
        Destroy(gameObject, 3f);
    }

    public void ResetBoss()
    {
        currentLife = lifeMax;
        isDead = false;

        transform.position = startPosition;
        transform.rotation = startRotation;

        if (agent != null)
            agent.enabled = true;

        GetComponent<Collider>().enabled = true;

        gameObject.SetActive(true);
    }
}