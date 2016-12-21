using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    PlayerInfo playerInfo;

	// Use this for initialization
	void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
	}

    void TakeDamage(int damage)
    {
        playerInfo.currentHp -= damage;
        if (playerInfo.currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        playerInfo.SetState(PlayerInfo.PlayerState.Dead);
        ///TODO: Go back to town
    }

}
