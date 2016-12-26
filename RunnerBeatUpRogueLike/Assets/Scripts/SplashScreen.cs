using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour {    
	
	void Start ()
    {
        Invoke("LoadMenu", 1.5f);
	}

    void LoadMenu()
    {
        //GameManager.instance.MainMenu();
        SceneManager.LoadScene("MainMenu");
    }
	
}
