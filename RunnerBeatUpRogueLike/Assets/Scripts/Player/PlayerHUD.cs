using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text distanceText;
    public Text goldAmount;

    PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

	void Update ()
    {
        distanceText.text = playerInfo.distanceWalked.ToString("F0") + " meters";
        goldAmount.text = playerInfo.goldEarned.ToString() + " gold";
    }
}
