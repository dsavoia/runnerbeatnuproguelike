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
        LoadFieldsData();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadFieldsData()
    {
        lv.text = "Level: " + GameManager.instance.playerAttributes.lv.ToString();
        pointsToSpend.text = "Points: " + GameManager.instance.playerAttributes.pointsToSpend.ToString();
        experience.text = "Experience: " + GameManager.instance.playerAttributes.experience.ToString();
        strenght.text = "Strenght: " + GameManager.instance.playerAttributes.strenght.ToString();
        endurance.text = "Endurance: " + GameManager.instance.playerAttributes.endurance.ToString();
        agility.text = "Agility: " + GameManager.instance.playerAttributes.agility.ToString();

        hp.text = "Hp: " + PlayerInfo.instance.maxHp.ToString();
        damage.text = "Damage: " + PlayerInfo.instance.basicAttackDamage.ToString();
        atkRate.text = "Atk Rate: " + PlayerInfo.instance.basicAttackCooldown.ToString();
        movSpeed.text = "Move speed: " + PlayerInfo.instance.speed.ToString();
        weaponName.text = "Weapon: " + "Wooden club";
        weaponDmg.text = "Damage: " + "1";
        weaponAtkSpeed.text = "Atk speed: " + "1";
        armorName.text = "Armor: " + "common clothes";
        armorDef.text = "Def: " + "1";
        amorWeight.text = "Weight: " + "1";

        townLv.text = "Town Lv: " + GameManager.instance.playerAttributes.townLevel.ToString();
        townDefCap.text = "Def Cap: " + GameManager.instance.playerAttributes.townDefCap.ToString();
        townChanceToKill.text = "Chance to Kill: " + GameManager.instance.playerAttributes.townChanceToKill.ToString();
        gold.text = "Gold: " + GameManager.instance.playerAttributes.gold.ToString();
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
