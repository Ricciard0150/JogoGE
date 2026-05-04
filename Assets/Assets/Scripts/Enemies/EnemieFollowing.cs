using UnityEngine;
using UnityEngine.AI;

public class EnemieFollowing : MonoBehaviour
{
    public Transform[] pontos;
    private int pontoAtual = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        IrParaProximoPonto();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            IrParaProximoPonto();
        }
    }

    void IrParaProximoPonto()
    {
        if (pontos.Length == 0) return;

        agent.SetDestination(pontos[pontoAtual].position);
        pontoAtual = (pontoAtual + 1) % pontos.Length;
    }
}