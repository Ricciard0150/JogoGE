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

    private float nextShot;

    void Start()
    {
        // ESCONDE SCOPE
        if (scopeUI != null)
        {
            scopeUI.SetActive(false);
        }

        // FOV NORMAL
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = normalFov;
        }

        // POSIÇÃO NORMAL
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

        // TIRO
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
        if (Time.time < nextShot)
            return;

        nextShot = Time.time + shootCooldown;

        RaycastHit hit;

        if (Physics.Raycast(
            fpsCam.transform.position,
            fpsCam.transform.forward,
            out hit,
            range))
        {
            Debug.Log("Acertou: " + hit.collider.name);

            EnemyHealth enemy =
                hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                Debug.Log("DEU DANO");
            }
        }
    }

    void ZoomIn()
    {
        // ZOOM
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = zoomFov;
        }

        // MOSTRA MIRA
        if (scopeUI != null)
        {
            scopeUI.SetActive(true);
        }
    }

    void ZoomOut()
    {
        // VOLTA FOV
        if (virtualCam != null)
        {
            virtualCam.m_Lens.FieldOfView = normalFov;
        }

        // ESCONDE MIRA
        if (scopeUI != null)
        {
            scopeUI.SetActive(false);
        }
    }
}