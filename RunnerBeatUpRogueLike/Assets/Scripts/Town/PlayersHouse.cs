using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersHouse : MonoBehaviour {
    
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
    public Text gold;
    public GameObject buttonUpParent;

    // Use this for initialization
    void Start ()
    {
        LoadFieldsData();
        print("Starting players House");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadFieldsData()
    {
        lv.text = "Level: " + PlayerInfo.instance.playerAttributes.lv.ToString();

        PlayerInfo.instance.CalculateNextLevelExperience();
        experience.text = "Exp: " + PlayerInfo.instance.playerAttributes.experience.ToString() + " / " + PlayerInfo.instance.expToNextLevel;

        CheckIfPointsToSpend();
        UpdateAttributesTexts();
        UpdateWeaponTexts();
        UpdateGoldText();
    }

    void UpdateGoldText()
    {
        gold.text = "Gold: " + PlayerInfo.instance.playerAttributes.gold.ToString();
    }

    void CheckIfPointsToSpend()
    {
        if (PlayerInfo.instance.playerAttributes.pointsToSpend > 0)
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
        switch (attCode)
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
        weaponName.text = "Weapon: " + PlayerInfo.instance.weapon.GetName();
        weaponDmg.text = "Damage: " + PlayerInfo.instance.weapon.GetDamage();
        weaponAtkSpeed.text = "Atk speed: " + PlayerInfo.instance.weapon.GetAttackRate().ToString();
    }

}
