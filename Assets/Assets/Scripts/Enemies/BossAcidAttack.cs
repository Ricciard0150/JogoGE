using UnityEngine;

public class BossAcidAttack : MonoBehaviour
{
    public GameObject acidProjectilePrefab;
    public Transform shootPoint;
    public Transform playerPoint;

    public float shootForce = 25f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootAcid();
        }
    }

    public void ShootAcid()
    {
        GameObject acid = Instantiate(
            acidProjectilePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Rigidbody rb = acid.GetComponent<Rigidbody>();

        Vector3 direcao = (playerPoint.position - shootPoint.position).normalized;

        rb.linearVelocity = direcao * shootForce;
    }
}