using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class EnemieFollowing : MonoBehaviour
{
    public Transform[] pontos;
    public Transform player;

    private int pontoAtual = 0;
    private NavMeshAgent agent;

    public float viewDistance = 10f;
    public float viewAngle = 90f;

    private bool chasing;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 3f;
        agent.speed = 2;
        IrParaProximoPonto();
    }

    void Update()
    {
        // verifica se o agent está funcionando
        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        if (CanSeePlayer())
        {
            chasing = true;
        }

        if (chasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                IrParaProximoPonto();
            }
        }
    }

    void IrParaProximoPonto()
    {
        if (pontos.Length == 0) return;

        agent.SetDestination(pontos[pontoAtual].position);
        pontoAtual = (pontoAtual + 1) % pontos.Length;
    }
    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle / 2)
            return false;

        if (Physics.Linecast(transform.position + Vector3.up, player.position, out RaycastHit hit))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }
    
}