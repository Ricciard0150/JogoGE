using UnityEngine;
using Unity.Cinemachine;

public class SniperGun : MonoBehaviour
{
    [Header("Dano")]
    public float damage = 100f;

    [Header("Range")]
    public float range = 200f;

    [Header("Cooldown")]
    public float shootCooldown = 1f;

    [Header("Camera")]
    public Camera fpsCam;
    public CinemachineVirtualCamera virtualCam;

    [Header("Zoom")]
    public float normalFov = 60f;
    public float zoomFov = 20f;

    [Header("Scope")]
    public GameObject scopeUI;

    [Header("Sniper")]
    public Transform sniper;
    public Vector3 normalPosition;
    public Vector3 aimPosition;
    public float aimSpeed = 10f;

    [Header("Effects")]
    public GameObject bloodEffect;
    public ParticleSystem muzzleFlash;

    private float nextShot;

    void Start()
    {
        // Esconde scope
        if (scopeUI != null)
        {
            scopeUI.SetActive(false);
        }

        // FOV normal
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = normalFov;
        }

        // Posição inicial da sniper
        if (sniper != null)
        {
            sniper.localPosition = normalPosition;
        }
    }

    void Update()
    {
        // MIRAR
        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }

        // ATIRAR
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // MOVIMENTO DA ARMA
        if (sniper != null)
        {
            Vector3 targetPos =
                Input.GetMouseButton(1)
                ? aimPosition
                : normalPosition;

            sniper.localPosition = Vector3.Lerp(
                sniper.localPosition,
                targetPos,
                aimSpeed * Time.deltaTime
            );
        }
    }

    void Shoot()
    {
        // Cooldown
        if (Time.time < nextShot)
            return;

        nextShot = Time.time + shootCooldown;

        // Muzzle Flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;

        if (Physics.Raycast(
            fpsCam.transform.position,
            fpsCam.transform.forward,
            out hit,
            range))
        {
            Debug.Log("Acertou: " + hit.collider.name);

            // Procura EnemyHealth
            EnemyHealth enemy =
                hit.collider.GetComponentInParent<EnemyHealth>();

            // Se acertou inimigo
            if (enemy != null)
            {
                // Dá dano + sangue
                enemy.TakeDamage(
                    damage,
                    hit.point,
                    hit.normal
                );

                Debug.Log("DEU DANO");
            }
        }
    }

    void ZoomIn()
    {
        // Zoom
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = zoomFov;
        }

        // Scope
        if (scopeUI != null)
        {
            scopeUI.SetActive(true);
        }
    }

    void ZoomOut()
    {
        // Volta FOV
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = normalFov;
        }

        // Esconde scope
        if (scopeUI != null)
        {
            scopeUI.SetActive(false);
        }
    }
}