using UnityEngine;

public class BossAcidAttack : MonoBehaviour
{
    public GameObject acidProjectilePrefab;

    public Transform shootPoint;
    public Transform playerPoint;

    public float shootForce = 25f;

    [Header("Ataque")]
    public float minAttackTime = 2f;
    public float maxAttackTime = 5f;

    [Header("Vis�o")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    private float attackTimer;
    private float nextAttackTime;

    private void Start()
    {
        EscolherNovoTempo();
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= nextAttackTime)
            {
                ShootAcid();

                attackTimer = 0f;

                EscolherNovoTempo();
            }
        }
    }

    void EscolherNovoTempo()
    {
        nextAttackTime = Random.Range(minAttackTime, maxAttackTime);
    }

    public void ShootAcid()
    {
        GameObject acid = Instantiate(
            acidProjectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Rigidbody rb = acid.GetComponent<Rigidbody>();

        Vector3 direcao =
            (playerPoint.position - shootPoint.position).normalized;

        rb.linearVelocity = direcao * shootForce;
    }

    bool CanSeePlayer()
    {
        if (playerPoint == null)
            return false;

        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = playerPoint.position + Vector3.up;

        Vector3 dir = target - origin;

        float distance = dir.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Linecast(origin, target, out RaycastHit hit))
        {
            if (hit.transform == playerPoint)
            {
                return true;
            }
        }

        return false;
    }
}