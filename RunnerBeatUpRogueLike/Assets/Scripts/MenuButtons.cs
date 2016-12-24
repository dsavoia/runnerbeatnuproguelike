using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public void StartNewGame()
    {
        SceneManager.LoadScene("Town");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Town");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }    
}
