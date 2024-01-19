using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // Replace "GameScene" with the name of your actual game scene
    }

    public void Credits()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
