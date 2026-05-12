using UnityEngine;

public class PlayerLife : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    public float life = 100f;
    public float lifeMax = 100f;

    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("UI")]
    public GameObject defeatScreen;

    [Header("Boss")]
    public BossAnimation boss;

    [Header("Tecla Respawn")]
    public KeyCode respawnKey = KeyCode.R;

    private bool isDead = false;

    public void Damage(int quantity)
    {
        if (isDead) return;

        life -= quantity;
        life = Mathf.Clamp(life, 0, lifeMax);

        if (life <= 0)
            Die();
    }

    void Update()
    {
        if (!isDead) return;

        if (Input.GetKeyDown(respawnKey))
        {
            Respawn();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Player morreu");

        if (defeatScreen != null)
            defeatScreen.SetActive(true);
    }

    void Respawn()
    {
        life = lifeMax;
        isDead = false;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        if (defeatScreen != null)
            defeatScreen.SetActive(false);

        if (boss != null)
            boss.ResetBoss();
    }
}