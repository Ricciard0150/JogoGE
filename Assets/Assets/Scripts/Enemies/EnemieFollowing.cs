using UnityEngine;
using UnityEngine.AI;

public class EnemieFollowing : MonoBehaviour
{
    [Header("Patrulha")]
    public Transform[] pontos;
    public Transform player;

    [Header("Visão")]
    public float viewDistance = 10f;
    public float viewAngle = 90f;

    private int pontoAtual = 0;

    private NavMeshAgent agent;
    private BossAnimation bossAnim;

    private bool chasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        bossAnim = GetComponent<BossAnimation>();

        agent.stoppingDistance = 3f;
        agent.speed = 2f;

        IrParaProximoPonto();
    }

    void Update()
    {
        // segurança
        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        // não movimenta durante o pulo
        if (bossAnim != null && bossAnim.IsJumping)
            return;

        // verifica visão
        chasing = CanSeePlayer();

        // perseguindo
        if (chasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // patrulha
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                IrParaProximoPonto();
            }
        }
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