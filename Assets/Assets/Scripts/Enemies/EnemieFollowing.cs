using UnityEngine;
using UnityEngine.AI;

public class EnemieFollowing : MonoBehaviour
{
    [Header("Patrulha")]
    public Transform[] pontos;
    public Transform player;

    [Header("Vis�o")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    [Header("Ataque")]
    public GameObject acidProjectilePrefab;
    public Transform shootPoint;

    public float shootForce = 25f;
    public float attackCooldown = 5f;

    private float attackTimer;

    private int pontoAtual = 0;

    private NavMeshAgent agent;
    private BossAnimation bossAnim;

    private bool chasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        bossAnim = GetComponent<BossAnimation>();

        agent.stoppingDistance = 3f;
        agent.speed = 5f;

        IrParaProximoPonto();
    }

    void Update()
    {
        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        if (!(bossAnim == null || !bossAnim.jumping))
            return;

        attackTimer += Time.deltaTime;

        chasing = CanSeePlayer();

        if (chasing)
        {
            // segue player
            agent.SetDestination(player.position);

            // ataca a cada 5 segundos
            if (attackTimer >= attackCooldown)
            {
                ShootAcid();

                attackTimer = 0f;
            }
        }
        else
        {
            // patrulha
            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                IrParaProximoPonto();
            }
        }
    }

    void ShootAcid()
    {
        GameObject acid = Instantiate(
            acidProjectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        Rigidbody rb = acid.GetComponent<Rigidbody>();

        Vector3 direction =
            (player.position - shootPoint.position).normalized;

        // movimento com arco
        rb.linearVelocity = direction * shootForce + Vector3.up * 5f;
    }

    void IrParaProximoPonto()
    {
        if (pontos.Length == 0)
            return;

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(pontos[pontoAtual].position);
        }

        pontoAtual = (pontoAtual + 1) % pontos.Length;
    }

    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up;

        Vector3 dir = target - origin;

        float distance = dir.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Linecast(origin, target, out RaycastHit hit))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}