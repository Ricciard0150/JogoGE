using UnityEngine;
using UnityEngine.AI;

public class BossGroundSlam : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform player;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Animator anim;

    [Header("Vis�o")]
    public float viewDistance = 15f;
    public float viewAngle = 90f;

    [Header("Pulo")]
    public float jumpHeight = 6f;
    public float jumpForwardForce = 3f;
    public float cooldown = 4f;

    [Header("Dano")]
    public float damageRadius = 6f;
    public int damage = 20;

    private bool jumping;
    private bool canJump = true;
    private bool canSeePlayer1;

    void Update()
    {
        if (player == null)
            return;

        // evita erro do navmesh
        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        // anima��o de andar
        anim.SetBool("Walking", CanSeePlayer());
        // pula somente vendo o player
        if (CanSeePlayer() && !jumping && canJump)
        {
            JumpAttack();
        }
    }

    void JumpAttack()
    {
        jumping = true;
        canJump = false;

        // toca anima��o
        anim.SetTrigger("Slam");

        // desliga navmesh
        agent.enabled = false;

        // dire��o do player
        Vector3 dir = (player.position - transform.position).normalized;

        // remove inclina��o
        dir.y = 0;

        // for�a
        Vector3 force = dir * jumpForwardForce;
        force.y = jumpHeight;

        // aplica velocidade
        rb.linearVelocity = force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // bateu no ch�o
        if (jumping && collision.gameObject.CompareTag("Floor"))
        {
            Slam();
        }
    }

    void Slam()
    {
        jumping = false;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }

        agent.enabled = true;

        if (agent.isOnNavMesh)
        {
            agent.Warp(transform.position);
        }

        Invoke(nameof(ResetJump), cooldown);
    }

    void ResetJump()
    {
        canJump = true;
    }

    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;

        float distance = dir.magnitude;

        // longe demais
        if (distance > viewDistance)
            return false;

        // �ngulo de vis�o
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > viewAngle / 2)
            return false;

        // verifica paredes
        if (Physics.Linecast(transform.position + Vector3.up, player.position, out RaycastHit hit))
        {
            if (hit.transform == player)
            {
                canSeePlayer1 = true;
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        // raio de dano
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        // vis�o
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}