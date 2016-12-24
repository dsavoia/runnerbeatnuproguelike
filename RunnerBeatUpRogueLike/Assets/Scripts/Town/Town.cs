using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour {

    public Text lv;
    public Text pointsToSpend;
    public Text experience;
    public Text strenght;
    public Text endurance;
    public Text agility;
    public Text hp;
    public Text damage;
    public Text atkRate;
    public Text movSpeed;
    public Text weaponName;
    public Text weaponDmg;
    public Text weaponAtkSpeed;
    public Text armorName;
    public Text armorDef;
    public Text amorWeight;
    public Text townLv;
    public Text townDefCap;
    public Text townChanceToKill;
    public Text gold;


    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadFieldsData()
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
