using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
     public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Out of the game.");
    }
}
