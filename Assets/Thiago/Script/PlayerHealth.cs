using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;

    [HideInInspector]
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Tela de Morte")]
    public GameObject deathScreen;

    [Header("Debug")]
    public bool showDebug = true;

    void Start()
    {
        // VIDA INICIAL
        currentHealth = maxHealth;

        // ESCONDE TELA DE MORTE
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }

        // CONFIGURA SLIDER
        if (healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = maxHealth;

            // números inteiros
            healthSlider.wholeNumbers = true;

            healthSlider.value = currentHealth;
        }

        if (showDebug)
        {
            Debug.Log("VIDA INICIADA: " + currentHealth);
        }

        // trava mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // garante tempo normal
        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        // tira vida
        currentHealth -= damage;

        // limita
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (showDebug)
        {
            Debug.Log("VIDA PLAYER: " + currentHealth);
        }

        // atualiza barra
        UpdateHealthUI();

        // morreu
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            // evita bug visual
            if (currentHealth <= 0)
            {
                healthSlider.value = 0;
            }
        }
    }

    void Die()
    {
        Debug.Log("PLAYER MORREU");

        // mostra tela preta
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        // libera mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // pausa jogo
        Time.timeScale = 0f;
    }

    // cura opcional
    public void Heal(int amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }
}