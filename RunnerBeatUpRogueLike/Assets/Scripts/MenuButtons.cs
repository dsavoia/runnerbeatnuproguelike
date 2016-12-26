using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuButtons : MonoBehaviour {

    public GameObject continueButton;

    public void Start()
    {
        ContinueGameButton();    
    }

    public void ContinueGameButton()
    {
        if (File.Exists(Application.persistentDataPath + "/" + GameManager.instance.saveFileName + ".dat"))
        {
            continueButton.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        GameManager.instance.MainMenu();
    }    
}
