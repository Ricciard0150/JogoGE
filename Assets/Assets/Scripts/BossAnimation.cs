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

    [Header("Poeira")]
    public GameObject slamDustPrefab;
    public int dustPoints = 12;
    public float dustRadius = 3f;

    public bool jumping;
    private bool canJump = true;
    private bool canMelee = true;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (player == null)
            return;

        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            return;

        bool seeingPlayer = CanSeePlayer();

        // 💥 MOVIMENTO CONTROLADO POR VISÃO
        if (!jumping)
        {
            if (seeingPlayer)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                // 🔥 evita erro do ResetPath
                if (agent.hasPath)
                    agent.ResetPath();
            }
        }

        // 💥 WALKING
        bool isMoving = agent.velocity.magnitude > 0.2f;
        anim.SetBool("Walking", seeingPlayer && !jumping && isMoving);

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
                dmg.Damage(meleeDamage);
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

        // 💥 DANO EM ÁREA
        Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable dmg))
                dmg.Damage(slamDamage);
        }

        SpawnDustCircle();

        agent.enabled = true;
        agent.Warp(transform.position);

        Invoke(nameof(ResetJump), slamCooldown);
    }

    void SpawnDustCircle()
    {
        for (int i = 0; i < dustPoints; i++)
        {
            float angle = i * Mathf.PI * 2f / dustPoints;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                0,
                Mathf.Sin(angle)
            ) * dustRadius;

            Vector3 spawnPos = transform.position + offset;

            if (Physics.Raycast(spawnPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
            {
                spawnPos = hit.point;
            }

            Instantiate(slamDustPrefab, spawnPos, Quaternion.identity);
        }
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
            return hit.transform == player;

        return false;
    }

    // 🔥 RESET DO BOSS
    public void ResetBoss()
    {
        jumping = false;
        canJump = true;
        canMelee = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        agent.enabled = false;

        transform.position = startPosition;
        transform.rotation = startRotation;

        agent.enabled = true;
        agent.Warp(startPosition);

        anim.SetBool("Walking", false);
    }
}