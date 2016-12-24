using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    PlayerInfo playerInfo;
    public BoxCollider2D pathArea;

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
        Vector2 targetPosition = playerInfo.targetObject.GetComponent<BoxCollider2D>().bounds.center;
        //print("Target name: " + playerInfo.targetObject.name);

        targetPosition.x = Mathf.Clamp(targetPosition.x, pathArea.bounds.min.x, pathArea.bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, pathArea.bounds.min.y, pathArea.bounds.max.y);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, playerInfo.speed * Time.deltaTime);
        UpdateFacingDirection(targetPosition);

        Debug.DrawLine(transform.position, targetPosition, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, playerInfo.interactiveObjectsLayer);

        //print(hit.collider.name);

        if (hit.distance <= playerInfo.basicAttackRange)
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
                GameManager.instance.LoadTown();
            }                    
        }
        else
        {
            UpdateFacingDirection(playerInfo.targetPos);
            Vector2 targetPos = playerInfo.targetPos;

            if (playerInfo.state != PlayerInfo.PlayerState.MovingToTown)
            {
                targetPos.x = Mathf.Clamp(targetPos.x, pathArea.bounds.min.x, pathArea.bounds.max.x);
                targetPos.y = Mathf.Clamp(targetPos.y, pathArea.bounds.min.y, pathArea.bounds.max.y);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerInfo.speed * Time.deltaTime);
        }        
    }

    void UpdateFacingDirection(Vector3 targetPosition)
    {
        if (targetPosition.x >= transform.position.x)
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
