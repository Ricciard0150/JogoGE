using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Game");
    }
    public void FecharJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}
