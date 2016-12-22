using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text distanceText;
    public Text goldAmount;
    public Text enemiesOnTown;

    public Image healthBar;
    public Text healthQty;   

    PlayerInfo playerInfo;

    void Start()
    {
        healthBar.fillAmount = 1;
        playerInfo = GetComponent<PlayerInfo>();
    }

	void Update ()
    {
        UpdateHUDTexts();
        UpdateHealthBar();
    }

    void UpdateHUDTexts()
    {
        distanceText.text = playerInfo.distanceWalked.ToString("F0") + " meters";
        goldAmount.text = playerInfo.goldEarned.ToString() + " g";
        enemiesOnTown.text = playerInfo.enemiesInTown.ToString() + " / " + playerInfo.maxEnemiesInTown.ToString() + " enememies in town";
        healthQty.text = playerInfo.currentHp + " / " + playerInfo.maxHp;
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)playerInfo.currentHp/(float)playerInfo.maxHp;
    }
}
