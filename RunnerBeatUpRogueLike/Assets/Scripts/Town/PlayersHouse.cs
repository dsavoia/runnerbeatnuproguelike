using UnityEngine;
using System.Collections.Generic;
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

    //TODO: kinda duplicated piece of code...
    Sprite[] weaponsSprite;
    Sprite[] consumablesSprite;
    List<BaseItem> itemsCol;
    public GameObject inventoryContent;
    public GameObject inventoryItemPrefab;

    GameObject equipedItem;

    void Start()
    {
        //TODO: kinda duplicated piece of code...
        weaponsSprite = Resources.LoadAll<Sprite>("WeaponsSprite");
        consumablesSprite = Resources.LoadAll<Sprite>("ConsumablesSprite");
        itemsCol = GameManager.instance.itemsCollection.items;
        LoadFieldsData();
    }

    // Update is called once per frame
    void Update() {

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
        LoadInventory();
    }

    void LoadInventory()
    {
        ClearInventory();

        for (int i = 0; i < itemsCol.Count; i++)
        {
            if (PlayerInfo.instance.playerAttributes.inventory.Contains(itemsCol[i].itemID))
            {
                AddInventoryItem(itemsCol[i]);
            }
        }
    }

    void ClearInventory()
    {
        //print("Pai: "+inventoryContent.name);

        foreach (Transform t in inventoryContent.GetComponentsInChildren<Transform>())
        {
            if (t.name != inventoryContent.name)
            {                
                Destroy(t.gameObject);
            }            
        }
    }

    //TODO: kinda duplicated piece of code...
    void AddInventoryItem(BaseItem item)
    {
        ShopItem itemScript;
        GameObject inventoryItem = Instantiate(inventoryItemPrefab);
        Button button;

        inventoryItem.name = "Inventory item " + item.name;
        itemScript = inventoryItem.GetComponent<ShopItem>();
        button = itemScript.GetComponentInChildren<Button>();
        itemScript.itemName = item.name;
        itemScript.itemNameText.text = item.name;
        itemScript.itemID = item.itemID;
        itemScript.type = item.type;
        itemScript.priceText.gameObject.SetActive(false);

        if (item.type == 0)
        {
            itemScript.icon.sprite = weaponsSprite[item.spriteIndex];
            if (PlayerInfo.instance.playerAttributes.equipedWeaponID == item.itemID)
            {
                button.interactable = false;
                button.GetComponentInChildren<Text>().text = "Equiped";
                equipedItem = inventoryItem;
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "Equip";                
            }

            AddListenerToInventoryButton(button, inventoryItem);
        }
        else if (item.type == 1)
        {
            itemScript.icon.sprite = consumablesSprite[item.spriteIndex];
            button.gameObject.SetActive(false);
        }        

        inventoryItem.transform.SetParent(inventoryContent.transform);
        inventoryItem.transform.position = new Vector3(inventoryContent.transform.position.x,
            inventoryContent.transform.position.y, inventoryContent.transform.position.z);
        inventoryItem.transform.localScale = new Vector3(1, 1, 1);
    }

    void AddListenerToInventoryButton(Button button, GameObject item)
    {
        button.onClick.AddListener(() => EquipWeapon(item));
    }

    void EquipWeapon(GameObject item)
    {        
        Button button = equipedItem.GetComponentInChildren<Button>();
        button.interactable = true;
        button.GetComponentInChildren<Text>().text = "Equip";
        
        button = item.GetComponentInChildren<Button>();
        button.interactable = false;
        button.GetComponentInChildren<Text>().text = "Equiped";
        equipedItem = item;
        PlayerInfo.instance.EquipWeapon(item.GetComponent<ShopItem>().itemID);

        UpdateWeaponTexts();
        UpdateAttributesTexts();
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
