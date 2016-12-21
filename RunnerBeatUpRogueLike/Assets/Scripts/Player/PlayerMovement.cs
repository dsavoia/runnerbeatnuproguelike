using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    PlayerInfo playerInfo;

    void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }
	
	void Update ()
    {        
        Move();
    }

    void Move()
    {
        switch (playerInfo.state)
        {
            case (PlayerInfo.PlayerState.MovingToPosition):        
                if (transform.position == playerInfo.targetPos)
                {
                    playerInfo.SetState(PlayerInfo.PlayerState.MovingForward);
                }
                else
                {
                    if (playerInfo.targetPos.x >= transform.position.x)
                    {
                        if (!playerInfo.facingRight)
                        {
                            playerInfo.facingRight = true;
                        }
                    }
                    else
                    {
                        if (playerInfo.facingRight)
                        {
                            playerInfo.facingRight = false;
                        }
                    }

                    transform.position = Vector3.MoveTowards(transform.position, playerInfo.targetPos, playerInfo.speed * Time.deltaTime);
                }
            break;

            case (PlayerInfo.PlayerState.MovingForward):
                if (!playerInfo.facingRight)
                {
                    playerInfo.facingRight = true;                
                }                
            break;
        }
    }
}
