using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Distancias")]
    public float shootDistance = 12f;

    [Header("Tiro")]
    public float shootCooldown = 1f;
    public float bulletForce = 25f;

    private float nextShot;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

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

        // SEGUE PLAYER
        if (distance > shootDistance)
        {
            agent.isStopped = false;

            agent.SetDestination(player.position);
        }
        // PARA E ATIRA
        else
        {
            agent.isStopped = true;

            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < nextShot)
            return;

        nextShot = Time.time + shootCooldown;

        // CRIA BALA
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // DIRE��O PRO PLAYER
            Vector3 direction =
                (player.position - firePoint.position).normalized;

            rb.linearVelocity = direction * bulletForce;
        }

        Destroy(bullet, 5f);
    }
}