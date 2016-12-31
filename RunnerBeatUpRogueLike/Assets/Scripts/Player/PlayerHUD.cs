using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text distanceText;
    public Text goldAmount;
    public Text enemiesOnTown;
    public Text experience;

    public Image healthBar;
    public Text healthQty;

    void Start()
    {
        healthBar.fillAmount = 1;
    }

	void Update ()
    {
        UpdateHUDTexts();
        UpdateHealthBar();
    }

    void UpdateHUDTexts()
    {
        distanceText.text = PlayerInfo.instance.distanceWalked.ToString("F0") + " meters";
        goldAmount.text = PlayerInfo.instance.goldEarned.ToString() + " g";
        enemiesOnTown.text = PlayerInfo.instance.enemiesInTown.ToString() + " / " + PlayerInfo.instance.maxEnemiesInTown.ToString() + " enemies in town";
        healthQty.text = PlayerInfo.instance.currentHp + " / " + PlayerInfo.instance.maxHp;
        experience.text = "Experience: " + PlayerInfo.instance.playerAttributes.experience.ToString() ;
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)PlayerInfo.instance.currentHp /(float)PlayerInfo.instance.maxHp;
    }
}
