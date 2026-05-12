using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    [Header("Camera")]
    public Camera cam;

    [Header("Distancia")]
    public float interactDistance = 5f;

    [Header("Porta Correta")]
    public bool isCorrectDoor = false;

    [Header("UI")]
    public TMP_Text messageText;

    void Update()
    {
        // Apertou E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Raycast do centro da tela
            Ray ray = cam.ScreenPointToRay(
                new Vector3(
                    Screen.width / 2,
                    Screen.height / 2
                )
            );

            RaycastHit hit;

            // Detecta porta
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Se acertou ESTA porta
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("INTERAGIU COM A PORTA");

                    // PORTA CERTA
                    if (isCorrectDoor)
                    {
                        SceneManager.LoadScene("Game");
                    }

                    // PORTA ERRADA
                    else
                    {
                        StartCoroutine(ShowMessage());
                    }
                }
            }
        }
    }

    IEnumerator ShowMessage()
    {
        messageText.text = "Essa nao e a sala certa...";

        yield return new WaitForSeconds(2f);

        messageText.text = "";
    }
}