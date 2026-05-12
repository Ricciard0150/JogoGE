using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Referencias")]
    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;

    [Header("Movimento")]
    public float speed = 8f;
    public float chaseDistance = 20f;
    public float attackDistance = 3f;

    [Header("Ataque")]
    public int damage = 10;
    public float attackCooldown = 1f;

    [Header("Debug")]
    public bool showDebug = false;

    private float nextAttackTime;

    void Start()
    {
        // NAVMESH AGENT
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent NÃO encontrado!");
            return;
        }

        // CONFIG AGENT
        agent.speed = speed;
        agent.angularSpeed = 700f;
        agent.acceleration = 40f;
        agent.stoppingDistance = attackDistance;

        // PROCURA PLAYER
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;

            Debug.Log("PLAYER ENCONTRADO");
        }
        else
        {
            Debug.LogError("Player NÃO encontrado! Coloque a tag Player.");
        }

        // PROCURA PLAYER HEALTH NA CENA
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
        {
            Debug.Log("PlayerHealth encontrado");
        }
        else
        {
            Debug.LogError("PlayerHealth NÃO encontrado!");
        }

        // DEBUG NAVMESH
        Debug.Log("Está no NavMesh: " + agent.isOnNavMesh);
    }

    void Update()
    {
        if (player == null || agent == null)
            return;

        // DISTÂNCIA
        float distance = Vector3.Distance(transform.position, player.position);

        if (showDebug)
        {
            Debug.Log("Distancia: " + distance);
        }

        // SEGUE PLAYER
        if (distance <= chaseDistance)
        {
            // SE LONGE -> PERSEGUE
            if (distance > attackDistance)
            {
                agent.isStopped = false;

                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(player.position);
                }
            }
            // SE PERTO -> ATACA
            else
            {
                agent.isStopped = true;

                // OLHAR PLAYER
                Vector3 lookPos = player.position - transform.position;
                lookPos.y = 0;

                if (lookPos != Vector3.zero)
                {
                    Quaternion rot = Quaternion.LookRotation(lookPos);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        rot,
                        10f * Time.deltaTime
                    );
                }

                // COOLDOWN ATAQUE
                if (Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + attackCooldown;

                    Attack();
                }
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void Attack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            Debug.Log("DEU DANO");
        }
        else
        {
            Debug.LogError("PlayerHealth NÃO encontrado!");
        }
    }

    // GIZMOS
    void OnDrawGizmosSelected()
    {
        // CHASE
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        // ATTACK
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}