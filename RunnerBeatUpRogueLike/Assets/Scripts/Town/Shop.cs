using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject weaponShopContent;
    public GameObject weaponItemPrefab;   

    public GameObject consumableShopContent;
    public GameObject consumableItemPrefab;    

    public GameObject inventoryShopContent;
    public GameObject inventoryItemPrefab;    
    public Text goldText;

    Sprite[] weaponsSprite;
    Sprite[] consumablesSprite;
    List<BaseItem> itemsCol;

    void Start ()
    {        
        weaponsSprite = Resources.LoadAll<Sprite>("WeaponsSprite");
        consumablesSprite = Resources.LoadAll<Sprite>("ConsumablesSprite");        
        itemsCol = GameManager.instance.itemsCollection.items;
        BuildShop();
        UpdatePlayerGold();
    }	

    void BuildShop()
    {
        for (int i = 0; i < itemsCol.Count; i++)
        {
            if (itemsCol[i].type == 0) //TODO: Decide if it's ok to let this value hardcoded
            {
                if (!PlayerInfo.instance.playerAttributes.inventory.Contains(itemsCol[i].itemID))
                {
                    AddWeaponsShopItem(itemsCol[i]);
                }
            }
            else if (itemsCol[i].type == 1)
            {
                if (!PlayerInfo.instance.playerAttributes.inventory.Contains(itemsCol[i].itemID))
                {
                    AddConsumableShopItem(itemsCol[i]);
                }
            }

            if (PlayerInfo.instance.playerAttributes.inventory.Contains(itemsCol[i].itemID))
            {
                AddInventoryShopItem(itemsCol[i]);
            }
        }        
    }

    void AddListenerToShopBuyButton(Button button, GameObject shopItem)
    {
        button.onClick.AddListener(() => BuyItem(shopItem));
    }

    void AddListenerToInventoryShopButton(Button button, GameObject shopItem)
    {
        button.onClick.AddListener(() => SellItem(shopItem));
    }

    public void BuyItem(GameObject shopItem)
    {   
        ShopItem selectedItemScript = shopItem.GetComponent<ShopItem>();

        if (PlayerInfo.instance.playerAttributes.gold >= selectedItemScript.price)
        {
            PlayerInfo.instance.playerAttributes.gold -= selectedItemScript.price;
            PlayerInfo.instance.playerAttributes.inventory.Add(selectedItemScript.itemID);

            UpdatePlayerGold();
            AddInventoryShopItem(itemsCol.Find(x => x.itemID == selectedItemScript.itemID));
            Destroy(shopItem);            
        }
    }

    public void SellItem(GameObject shopItem)
    {
        ShopItem selectedItemScript = shopItem.GetComponent<ShopItem>();
        
        PlayerInfo.instance.playerAttributes.gold += selectedItemScript.price;
        PlayerInfo.instance.playerAttributes.inventory.Remove(selectedItemScript.itemID);

        print(selectedItemScript.type);

        if (selectedItemScript.type == 0)
        {
            AddWeaponsShopItem(itemsCol.Find(x => x.itemID == selectedItemScript.itemID));
        }
        else if (selectedItemScript.type == 1)
        {
            AddConsumableShopItem(itemsCol.Find(x => x.itemID == selectedItemScript.itemID));
        }

        UpdatePlayerGold();
        Destroy(shopItem);        
    }

    void AddWeaponsShopItem(BaseItem weapon)
    {
        ShopItem shopItemScript;
        GameObject shopItem = Instantiate(weaponItemPrefab);
        shopItem.name = "Weapon item " + weapon.name;
        shopItemScript = shopItem.GetComponent<ShopItem>();
        shopItemScript.itemName = weapon.name;
        shopItemScript.itemNameText.text = weapon.name;
        shopItemScript.itemID = weapon.itemID;
        shopItemScript.price = weapon.price;
        shopItemScript.type = weapon.type;
        shopItemScript.priceText.text = shopItemScript.price.ToString() + " gold";
        shopItemScript.icon.sprite = weaponsSprite[weapon.spriteIndex];
        AddListenerToShopBuyButton(shopItemScript.GetComponentInChildren<Button>(), shopItem);
        shopItem.transform.SetParent(weaponShopContent.transform);
        shopItem.transform.position = new Vector3(weaponShopContent.transform.position.x,
            weaponShopContent.transform.position.y, weaponShopContent.transform.position.z);
        shopItem.transform.localScale = new Vector3(1, 1, 1);
    }

    void AddInventoryShopItem(BaseItem item)
    {
        ShopItem shopItemScript;
        GameObject inventoryItem = Instantiate(inventoryItemPrefab);
        Button button;

        inventoryItem.name = "Inventory item " + item.name;
        shopItemScript = inventoryItem.GetComponent<ShopItem>();
        button = shopItemScript.GetComponentInChildren<Button>();
        shopItemScript.itemName = item.name;
        shopItemScript.itemNameText.text = item.name;
        shopItemScript.itemID = item.itemID;
        shopItemScript.type = item.type;
        shopItemScript.price = Mathf.RoundToInt(item.price / 3);
        shopItemScript.priceText.text = shopItemScript.price.ToString() + " gold";

        if (item.type == 0)
        {
            shopItemScript.icon.sprite = weaponsSprite[item.spriteIndex];
            if (PlayerInfo.instance.playerAttributes.equipedWeaponID == item.itemID)
            {
                button.interactable = false;
                button.GetComponentInChildren<Text>().text = "Equiped";
            }
        }
        else if (item.type == 1)
        {
            shopItemScript.icon.sprite = consumablesSprite[item.spriteIndex];
        }

        AddListenerToInventoryShopButton(button, inventoryItem);

        inventoryItem.transform.SetParent(inventoryShopContent.transform);
        inventoryItem.transform.position = new Vector3(inventoryShopContent.transform.position.x,
            inventoryShopContent.transform.position.y, inventoryShopContent.transform.position.z);
        inventoryItem.transform.localScale = new Vector3(1, 1, 1);
    }    

    void AddConsumableShopItem(BaseItem consumable)
    {
        ShopItem shopItemScript;
        GameObject consumableItem = Instantiate(consumableItemPrefab);
        Button button;

        consumableItem.name = "Consumable item " + consumable.name;
        shopItemScript = consumableItem.GetComponent<ShopItem>();
        button = shopItemScript.GetComponentInChildren<Button>();
        shopItemScript.itemName = consumable.name;
        shopItemScript.itemNameText.text = consumable.name;
        shopItemScript.itemID = consumable.itemID;
        shopItemScript.type = consumable.type;
        shopItemScript.price = consumable.price;
        shopItemScript.priceText.text = shopItemScript.price.ToString() + " gold";
        
        shopItemScript.icon.sprite = consumablesSprite[consumable.spriteIndex];

        button.GetComponentInChildren<Text>().text = "Buy";
        AddListenerToShopBuyButton(button, consumableItem);

        consumableItem.transform.SetParent(consumableShopContent.transform);
        consumableItem.transform.position = new Vector3(consumableShopContent.transform.position.x,
            consumableShopContent.transform.position.y, consumableShopContent.transform.position.z);
        consumableItem.transform.localScale = new Vector3(1, 1, 1);
    }

    void UpdatePlayerGold()
    {
        goldText.text = "Gold: " + PlayerInfo.instance.playerAttributes.gold.ToString();
    }
}
