using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour {
    
    [SerializeField]
    public enum TownState
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

    public TownState townState;

    public GameObject townCanvas;
    public GameObject shopCanvas;
    public GameObject playersHouseCanvas;
    public GameObject townHallCanvas;

    PlayersHouse playersHouse;

    // Use this for initialization
    void Start ()
    {
        PlayerInfo.instance.SetState(PlayerInfo.PlayerState.InTown);
        playersHouse = GetComponent<PlayersHouse>();

        ChangeTownCanvas(0);
    }  

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
        townState = (TownState)newCanvasState;
        UpdateTownCanvas();
    }

    void UpdateTownCanvas()
    {
        townCanvas.SetActive(false);
        playersHouseCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        townHallCanvas.SetActive(false);

        switch(townState)
        {
            case (TownState.Town):
                townCanvas.SetActive(true);
            break;
            case (TownState.PlayersHouse):
                playersHouseCanvas.SetActive(true);
                playersHouse.LoadFieldsData();
            break;
            case (TownState.Shop):
                shopCanvas.SetActive(true);
            break;
            case (TownState.TownHall):
                townHallCanvas.SetActive(true);
            break;
        }
    }    

}
 