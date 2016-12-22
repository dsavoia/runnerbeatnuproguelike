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
        Vector2 targetPosition = playerInfo.targetPos.GetComponent<BoxCollider2D>().bounds.center;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, playerInfo.speed * Time.deltaTime);
        UpdateFacingDirection();

        Debug.DrawLine(transform.position, playerInfo.targetPos.position, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, objectsLayer);

        print(hit.collider.name);

        if (hit.distance <= playerInfo.basicAttackRange)
        {
            playerInfo.SetState(PlayerInfo.PlayerState.Fighting);
        }

    }

    void Move()
    {
        
        if (transform.position == playerInfo.targetPos.position)
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
            transform.position = Vector3.MoveTowards(transform.position, playerInfo.targetPos.position, playerInfo.speed * Time.deltaTime);
        }        
    }

    void UpdateFacingDirection()
    {
        if (playerInfo.targetPos.position.x >= transform.position.x)
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
