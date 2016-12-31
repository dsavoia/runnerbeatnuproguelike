using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

    public GameObject weaponShopContent;
    public GameObject shopItemPrefab;

	// Use this for initialization
	void Start ()
    {
        BuildWeaponShop();        
	}	

    void BuildWeaponShop()
    {
        Sprite[] itemsSprite;

        itemsSprite = Resources.LoadAll<Sprite>("WeaponsSprite");

        ShopItem shopItemScript;

        foreach (BaseItem item in GameManager.instance.itemsCollection.items)
        {
            GameObject shopItem = Instantiate(shopItemPrefab);
            shopItemScript = shopItem.GetComponent<ShopItem>();
            shopItemScript.itemName.text = item.name;
            shopItemScript.icon.sprite = itemsSprite[item.spriteIndex];
            shopItemScript.price.text = item.price.ToString() + " gold";
            shopItem.transform.SetParent(weaponShopContent.transform);
            shopItem.transform.position = new Vector3(weaponShopContent.transform.position.x,
                weaponShopContent.transform.position.y, weaponShopContent.transform.position.z);
            shopItem.transform.localScale = new Vector3(1,1,1);
        }

        //print("Shop Loaded");
    }
}
