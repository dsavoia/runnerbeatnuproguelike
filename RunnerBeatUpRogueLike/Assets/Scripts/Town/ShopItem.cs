using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Text itemNameText;    
    public Text priceText;
    public Image icon;
    
    public int itemID;
    public string itemName;
    public int price;
    public int type;
    public bool playerHasItem = false;    
}
