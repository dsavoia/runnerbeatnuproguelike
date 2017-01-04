using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour {
    
    [SerializeField]
    public enum CanvasState
    {
        Town = 0,
        PlayersHouse = 1,
        Shop = 2,
        TownHall = 3
    }

    /*
    public Text townLv;
    public Text townDefCap;
    public Text townChanceToKill;
    */

    CanvasState canvasState;

    public GameObject townCanvas;
    public GameObject shopCanvas;
    public GameObject playersHouseCanvas;
    public GameObject townHallCanvas;



    // Use this for initialization
    void Start ()
    {
        PlayerInfo.instance.SetState(PlayerInfo.PlayerState.InTown);
        ChangeTownCanvas(0);
    }
    	
	void Update ()
    {
	
	}

    /*
    void UpdateTownTexts()
    {
        townLv.text = "Town Lv: " + PlayerInfo.instance.playerAttributes.townLevel.ToString();
        townDefCap.text = "Def Cap: " + PlayerInfo.instance.playerAttributes.townDefCap.ToString();
        townChanceToKill.text = "Chance to Kill: " + PlayerInfo.instance.playerAttributes.townChanceToKill.ToString();
    }*/

    public void AttackTheEnemies()
    {
        GameManager.instance.LoadGameScene();
    }

    public void GoBackToMenu()
    {        
        GameManager.instance.MainMenu();
    }

    public void ChangeTownCanvas(int newCanvasState)
    {
        canvasState = (CanvasState)newCanvasState;
        UpdateTownCanvas();
    }

    void UpdateTownCanvas()
    {
        townCanvas.SetActive(false);
        playersHouseCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        townHallCanvas.SetActive(false);

        switch(canvasState)
        {
            case (CanvasState.Town):
                townCanvas.SetActive(true);
            break;
            case (CanvasState.PlayersHouse):
                playersHouseCanvas.SetActive(true);
            break;
            case (CanvasState.Shop):
                shopCanvas.SetActive(true);
            break;
            case (CanvasState.TownHall):
                townHallCanvas.SetActive(true);
            break;
        }
    }    

}
 