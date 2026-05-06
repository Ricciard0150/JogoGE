using UnityEngine;
using System.Collections;

public class PlayerLife : MonoBehaviour, IDamageable
{
    public float life = 100f;
    public float lifeMax = 100f;

    public Transform respawnPoint;
    public GameObject defeatScreen;

    bool isDead = false;

    public void Damage(int quantity)
    {
        if (isDead) return;

        life -= quantity;
        life = Mathf.Clamp(life, 0, lifeMax);

        //StartCoroutine(DamageFeedback());

        if (life <= 0)
        {
            Derrota();
        }
    }



    //IEnumerator DamageFeedback()
    //{
    //    if (sprite != null)
    //    {
    //        sprite.color = Color.red;
    //        yield return new WaitForSeconds(1f);
    //        sprite.color = Color.white;
    //    }
    //}

    void Derrota()
    {
        isDead = true;

        Debug.Log("Player morreu");

        if (defeatScreen != null)
            defeatScreen.SetActive(true);

        Invoke(nameof(Respawn), 2f);
    }

    void Respawn()
    {
        life = lifeMax;
        isDead = false;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        if (defeatScreen != null)
            defeatScreen.SetActive(false);
    }
}