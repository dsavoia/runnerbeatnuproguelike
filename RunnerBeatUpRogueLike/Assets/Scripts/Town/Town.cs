using UnityEngine;
using UnityEngine.UI;

public class Town : MonoBehaviour {

    public Text lv;
    public Text pointsToSpend;
    public Text experience;
    public Text strength;
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

    public GameObject buttonUpParent;

    public GameObject townCanvas;
    public GameObject shopCanvas;


    // Use this for initialization
    void Start ()
    {
        PlayerInfo.instance.SetState(PlayerInfo.PlayerState.InTown);
        PlayerInfo.instance.CalculateNextLevelExperience();
        LoadFieldsData();
        CheckIfPointsToSpend();
        townCanvas.SetActive(true);
        shopCanvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadFieldsData()
    {
        lv.text = "Level: " + PlayerInfo.instance.playerAttributes.lv.ToString();        
        experience.text = "Exp: " + PlayerInfo.instance.playerAttributes.experience.ToString() + " / " + PlayerInfo.instance.expToNextLevel;

        UpdateAttributesTexts();
        UpdateWeaponTexts();
        UpdateTownTexts();
        UpdateArmorTexts();
        UpdateGoldText();
    }

    void UpdateAttributesTexts()
    {
        strength.text = "Strength: " + PlayerInfo.instance.playerAttributes.strength.ToString();
        endurance.text = "Endurance: " + PlayerInfo.instance.playerAttributes.endurance.ToString();
        agility.text = "Agility: " + PlayerInfo.instance.playerAttributes.agility.ToString();
        pointsToSpend.text = "Points: " + PlayerInfo.instance.playerAttributes.pointsToSpend.ToString();

        hp.text = "Hp: " + PlayerInfo.instance.maxHp.ToString();
        damage.text = "Damage: " + PlayerInfo.instance.attackDamage.ToString();
        atkRate.text = "Atk Rate: " + PlayerInfo.instance.attackRate.ToString("F2");
        movSpeed.text = "Movement speed: " + PlayerInfo.instance.speed.ToString();
    }

    void UpdateWeaponTexts()
    {
        weaponName.text = "Weapon: " + PlayerInfo.instance.weapon.weaponName;
        weaponDmg.text = "Damage: " + PlayerInfo.instance.weapon.damage;
        weaponAtkSpeed.text = "Atk speed: " + PlayerInfo.instance.weapon.atkRate.ToString();        
    }

    void UpdateArmorTexts()
    {
        armorName.text = "Armor: " + PlayerInfo.instance.armor.armorName;
        armorDef.text = "Def: " + PlayerInfo.instance.armor.defense;
        amorWeight.text = "Weight: " + PlayerInfo.instance.armor.weight;
    }

    void UpdateTownTexts()
    {
        townLv.text = "Town Lv: " + PlayerInfo.instance.playerAttributes.townLevel.ToString();
        townDefCap.text = "Def Cap: " + PlayerInfo.instance.playerAttributes.townDefCap.ToString();
        townChanceToKill.text = "Chance to Kill: " + PlayerInfo.instance.playerAttributes.townChanceToKill.ToString();
    }

    void UpdateGoldText()
    {
        gold.text = "Gold: " + PlayerInfo.instance.playerAttributes.gold.ToString();
    }

    void CheckIfPointsToSpend()
    {
        if(PlayerInfo.instance.playerAttributes.pointsToSpend > 0)
        {
            buttonUpParent.SetActive(true);
        }
        else
        {
            buttonUpParent.SetActive(false);
        }
    }   

    public void AddPointToAttribute(int attCode)
    {
        switch(attCode)
        {
            case (0):
                PlayerInfo.instance.playerAttributes.strength++;
                PlayerInfo.instance.playerAttributes.pointsToSpend--;
                PlayerInfo.instance.CalculateNewAtkValues();
            break;
            case (1):
                PlayerInfo.instance.playerAttributes.endurance++;
                PlayerInfo.instance.playerAttributes.pointsToSpend--;
                PlayerInfo.instance.CalculateNewHealthValue();
                PlayerInfo.instance.CalculateNewDmgReductionValue();
                break;
            case (2):
                PlayerInfo.instance.playerAttributes.agility++;
                PlayerInfo.instance.playerAttributes.pointsToSpend--;
                PlayerInfo.instance.CalculateNewMovSpeedValue();
                PlayerInfo.instance.CalculateNewAtkRateValue();
            break;
        }
             
        UpdateAttributesTexts();
        CheckIfPointsToSpend();
    }    

    public void AttackTheEnemies()
    {
        GameManager.instance.LoadGameScene();
    }

    public void GoBackToMenu()
    {        
        GameManager.instance.MainMenu();
    }

    public void OpenShopCanvas()
    {
        townCanvas.SetActive(false);
        shopCanvas.SetActive(true);
    }

    public void OpenTownCanvas()
    {
        townCanvas.SetActive(true);
        shopCanvas.SetActive(false);
    }

}
