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
    public float viewAngle = 120f;

    [Header("Melee")]
    public float meleeDistance = 3f;
    public int meleeDamage = 10;
    public float meleeCooldown = 1.5f;

    [Header("Slam")]
    public float slamDistance = 8f;
    public float jumpHeight = 5f;
    public float jumpForce = 6f;
    public float slamCooldown = 4f;

    [Header("Dano Slam")]
    public float slamRadius = 5f;
    public int slamDamage = 20;

    public bool jumping;
    private bool canJump = true;
    private bool canMelee = true;

    void Update()
    {
        if (player == null)
            return;

        bool seeingPlayer = CanSeePlayer();

        if (!jumping)
        {
            agent.SetDestination(player.position);
        }

        anim.SetBool("Walking", agent.velocity.magnitude > 0.2f);

        float dist = Vector3.Distance(transform.position, player.position);

        // MELEE
        if (seeingPlayer && dist <= meleeDistance && canMelee && !jumping)
        {
            MeleeAttack();
            return;
        }

        // SLAM
        if (seeingPlayer && dist <= slamDistance && canJump && !jumping)
        {
            JumpAttack();
        }
    }

    void MeleeAttack()
    {
        canMelee = false;

        agent.isStopped = true;

        anim.SetTrigger("Attack");

        Invoke(nameof(ResetMelee), meleeCooldown);
    }

    public void DealMeleeDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= meleeDistance + 1f)
        {
            if (player.TryGetComponent(out IDamageable dmg))
            {
                dmg.Damage(meleeDamage);
            }
        }
    }

    void ResetMelee()
    {
        canMelee = true;
        agent.isStopped = false;
    }

    void JumpAttack()
    {
        jumping = true;
        canJump = false;

        anim.SetTrigger("Slam");

        agent.enabled = false;

        rb.isKinematic = false;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        Vector3 force = dir * jumpForce;
        force.y = jumpHeight;

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
        rb.isKinematic = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable dmg))
            {
                dmg.Damage(slamDamage);
            }
        }

        agent.enabled = true;
        agent.Warp(transform.position);

        Invoke(nameof(ResetJump), slamCooldown);
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

        if (dir.magnitude > viewDistance)
            return false;

        if (Vector3.Angle(transform.forward, dir) > viewAngle / 2f)
            return false;

        if (Physics.Linecast(origin, target, out RaycastHit hit))
        {
            return hit.transform == player;
        }

        return false;
    }
}