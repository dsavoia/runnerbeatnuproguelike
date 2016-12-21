using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {

    PlayerInfo playerInfo;

	// Use this for initialization
	void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
	}

    public void TakeDamage(int damage)
    {
        playerInfo.currentHp -= damage;
        if (playerInfo.currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerInfo.SetState(PlayerInfo.PlayerState.Dead);
        EventManager.OnPlayerDeath();
    }
}
