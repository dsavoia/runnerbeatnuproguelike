using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text distanceText;
    public Text goldAmount;
    public Text enemiesOnTown;

    PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

	void Update ()
    {
        UpdateHUDTexts();
    }

    void UpdateHUDTexts()
    {
        distanceText.text = playerInfo.distanceWalked.ToString("F0") + " meters";
        goldAmount.text = playerInfo.goldEarned.ToString() + " g";
        enemiesOnTown.text = playerInfo.enemiesInTown.ToString() + " / " + playerInfo.maxEnemiesOnTown.ToString() + " enememies in town";
    }
}
