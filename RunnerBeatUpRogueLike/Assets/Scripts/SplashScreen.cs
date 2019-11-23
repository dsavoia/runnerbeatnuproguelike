using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour {    
	
	void Start ()
    {
        Invoke("LoadMenu", 1.5f);
	}

    void LoadMenu()
    {        
        SceneManager.LoadScene("MainMenu");
    }	
}
