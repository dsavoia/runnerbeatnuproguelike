using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public GameObject weaponShopContent;
    public GameObject shopItemPrefab;
    public List<GameObject> shopItemList;

    public GameObject consumableShopContent;
    public GameObject consumableItemPrefab;
    public List<GameObject> consumableItemList;

    public GameObject inventoryShopContent;
    public GameObject inventoryItemPrefab;
    public List<GameObject> inventoryItemList;
    public Text goldText;
     

    // Use this for initialization
    void Start ()
    {
        BuildWeaponShop();
        BuildConsumablesShop();
        BuildInventory();
    }	

    void BuildWeaponShop()
    {
        Sprite[] itemsSprite;

        itemsSprite = Resources.LoadAll<Sprite>("WeaponsSprite");

        ShopItem shopItemScript;

        List<BaseItem> storeItems = GameManager.instance.itemsCollection.items;

        for (int i = 0; i < storeItems.Count; i++)
        {
            if (!PlayerInfo.instance.playerAttributes.inventory.Contains(storeItems[i].itemID))
            {
                GameObject shopItem = Instantiate(shopItemPrefab);

                shopItem.name = "shop item " + i;

                shopItemScript = shopItem.GetComponent<ShopItem>();

                shopItemScript.itemName = storeItems[i].name;
                shopItemScript.itemNameText.text = storeItems[i].name;

                shopItemScript.itemID = storeItems[i].itemID;

                shopItemScript.price = storeItems[i].price;
                shopItemScript.priceText.text = storeItems[i].price.ToString() + " gold";               

                shopItemScript.icon.sprite = itemsSprite[storeItems[i].spriteIndex];                

                AddListenerToShopButton(shopItemScript.GetComponentInChildren<Button>(), shopItem);

                shopItem.transform.SetParent(weaponShopContent.transform);

                shopItem.transform.position = new Vector3(weaponShopContent.transform.position.x,
                    weaponShopContent.transform.position.y, weaponShopContent.transform.position.z);

                shopItem.transform.localScale = new Vector3(1, 1, 1);

                shopItemList.Add(shopItem);
            }
        }        
    }

    void AddListenerToShopButton(Button button, GameObject shopItem)
    {
        button.onClick.AddListener(() => BuyItem(shopItem));
    }

    public void BuyItem(GameObject clickedButton)
    {   
        ShopItem selectedItemScript = clickedButton.GetComponent<ShopItem>();

        //print("comprando " + selectedItemScript.itemName);

        if (PlayerInfo.instance.playerAttributes.gold >= selectedItemScript.price)
        {
            PlayerInfo.instance.playerAttributes.gold -= selectedItemScript.price;
            PlayerInfo.instance.playerAttributes.inventory.Add(selectedItemScript.itemID);

            shopItemList.Remove(clickedButton);
            Destroy(clickedButton);
            UpdatePlayerInventory();
        }
    }

    void BuildConsumablesShop()
    {
        
    }

    void BuildInventory()
    {
        goldText.text = "Gold: " + PlayerInfo.instance.playerAttributes.gold.ToString();
    }

    public void UpdatePlayerInventory()
    {
        goldText.text = "Gold: " + PlayerInfo.instance.playerAttributes.gold.ToString();
    }    
}
