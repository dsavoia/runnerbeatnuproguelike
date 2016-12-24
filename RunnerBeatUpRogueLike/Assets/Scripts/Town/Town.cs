using UnityEngine;

public class Town : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AttackTheEnemies()
    {
        GameManager.instance.LoadGameScene();
    }

    public void GoBackToMenu()
    {
        GameManager.instance.MainMenu();
    }

}
