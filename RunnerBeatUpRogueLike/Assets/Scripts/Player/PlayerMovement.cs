using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    PlayerInfo playerInfo;
    public LayerMask objectsLayer;


    void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }
	
	void Update ()
    {
        switch (playerInfo.state)
        {
            case (PlayerInfo.PlayerState.MovingToPosition):
            case (PlayerInfo.PlayerState.MovingToTown):
                Move();
            break;

            case (PlayerInfo.PlayerState.MovingForward):
                if (!playerInfo.facingRight)
                {
                    playerInfo.facingRight = true;
                }
            break;

            case (PlayerInfo.PlayerState.MovingToTarget):
                MoveToTarget();
            break;

        }
    }

    protected virtual void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerInfo.targetPos, playerInfo.speed * Time.deltaTime);
        UpdateFacingDirection();

        Debug.DrawLine(transform.position, playerInfo.targetPos, Color.yellow);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerInfo.targetPos, objectsLayer);

        if (hit.distance <= playerInfo.attackRange)
        {
            playerInfo.SetState(PlayerInfo.PlayerState.Fighting);
        }

    }

    void Move()
    {
        
        if (transform.position == playerInfo.targetPos)
        {
            if(playerInfo.state == PlayerInfo.PlayerState.MovingToPosition)
            {
                playerInfo.SetState(PlayerInfo.PlayerState.MovingForward);
            }
            else
            {
                playerInfo.LoadTown();
            }                    
        }
        else
        {
            UpdateFacingDirection();
            transform.position = Vector3.MoveTowards(transform.position, playerInfo.targetPos, playerInfo.speed * Time.deltaTime);
        }        
    }

    void UpdateFacingDirection()
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
    }
}
