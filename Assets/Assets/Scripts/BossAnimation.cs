using UnityEngine;
using UnityEngine.AI;

public class BossAnimation : MonoBehaviour
{
    [Header("Refer�ncias")]
    public Transform player;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Animator anim;

    [Header("Vis�o")]
    public float viewDistance = 15f;
    public float viewAngle = 90f;

    [Header("Ataque")]
    public float attackDistance = 8f;

    [Header("Pulo")]
    public float jumpHeight = 5f;
    public float jumpForwardForce = 6f;
    public float cooldown = 4f;

    [Header("Dano")]
    public float damageRadius = 5f;
    public int damage = 20;

    private bool jumping;
    private bool canJump = true;

    public bool IsJumping => jumping;

    void Update()
    {
        if (player == null)
            return;

        // anima��o de andar
        if (agent.enabled)
        {
            anim.SetBool("Walking", agent.velocity.magnitude > 0.1f);
        }

        float dist = Vector3.Distance(transform.position, player.position);

        // ataque
        if (CanSeePlayer() &&
            dist <= attackDistance &&
            !jumping &&
            canJump)
        {
            JumpAttack();
        }
    }

    void JumpAttack()
    {
        jumping = true;
        canJump = false;

        anim.SetTrigger("Slam");

        // desliga navmesh
        if (agent.enabled)
        {
            agent.enabled = false;
        }

        // ativa f�sica
        rb.isKinematic = false;

        // dire��o
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;

        // for�a
        Vector3 force = dir * jumpForwardForce;
        force.y = jumpHeight;

        // aplica velocidade
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

        // para f�sica
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        
        // dano em �rea
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(damage);
            }
        }

        // reativa navmesh
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}