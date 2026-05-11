using UnityEngine;
using UnityEngine.AI;

public class BossAnimation : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Animator anim;

    [Header("Visão")]
    public float viewDistance = 15f;
    public float viewAngle = 90f;

    [Header("Pulo")]
    public float jumpHeight = 5f;
    public float jumpForwardForce = 3f;
    public float cooldown = 4f;

    [Header("Dano")]
    public float damageRadius = 5f;
    public int damage = 20;

    private bool jumping;
    private bool canJump = true;

    void Update()
    {
        if (player == null)
            return;

        // segurança navmesh
        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        // animação de andar
        anim.SetBool("Walking", agent.velocity.magnitude > 0.1f);

        // ataque
        if (CanSeePlayer() && !jumping && canJump)
        {
            JumpAttack();
        }
    }

    void JumpAttack()
    {
        jumping = true;
        canJump = false;

        // toca animação
        anim.SetTrigger("Slam");

        // desliga navmesh
        agent.enabled = false;

        // direção do player
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // força
        Vector3 force = dir * jumpForwardForce;
        force.y = jumpHeight;

        // aplica no rigidbody
        rb.linearVelocity = force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (jumping && collision.gameObject.CompareTag("Floor"))
        {
            Slam();
        }
    }

    void Slam()
    {
        jumping = false;

        rb.linearVelocity = Vector3.zero;

        // dano em área
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }

        // volta navmesh
        agent.enabled = true;
        agent.Warp(transform.position);

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}