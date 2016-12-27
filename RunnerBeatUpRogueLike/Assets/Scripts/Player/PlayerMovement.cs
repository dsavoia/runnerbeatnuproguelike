using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    
    public BoxCollider2D pathArea;
	
	void Update ()
    {
        switch (PlayerInfo.instance.state)
        {
            case (PlayerInfo.PlayerState.MovingToPosition):
            case (PlayerInfo.PlayerState.MovingToTown):
                Move();
            break;

            case (PlayerInfo.PlayerState.MovingForward):
                if (!PlayerInfo.instance.facingRight)
                {
                    PlayerInfo.instance.facingRight = true;
                }
            break;

            case (PlayerInfo.PlayerState.MovingToTarget):
                MoveToTarget();
            break;

        }
    }

    protected virtual void MoveToTarget()
    {
        Vector2 targetPosition = PlayerInfo.instance.targetObject.GetComponent<BoxCollider2D>().bounds.center;        

        targetPosition.x = Mathf.Clamp(targetPosition.x, pathArea.bounds.min.x, pathArea.bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, pathArea.bounds.min.y, pathArea.bounds.max.y);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, PlayerInfo.instance.speed * Time.deltaTime);
        UpdateFacingDirection(targetPosition);

        Debug.DrawLine(transform.position, targetPosition, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, PlayerInfo.instance.interactiveObjectsLayer);

        //print(hit.collider.name);

        if (hit.distance <= PlayerInfo.instance.attackRange)
        {
            PlayerInfo.instance.SetState(PlayerInfo.PlayerState.Fighting);
        }

    }

    void Move()
    {
        
        if (transform.position == PlayerInfo.instance.targetPos)
        {
            if(PlayerInfo.instance.state == PlayerInfo.PlayerState.MovingToPosition)
            {
                PlayerInfo.instance.SetState(PlayerInfo.PlayerState.MovingForward);
            }
            else
            {
                PlayerInfo.instance.SetState(PlayerInfo.PlayerState.InTown);
                GameManager.instance.LoadTown();
            }                    
        }
        else
        {
            UpdateFacingDirection(PlayerInfo.instance.targetPos);
            Vector2 targetPos = PlayerInfo.instance.targetPos;

            if (PlayerInfo.instance.state != PlayerInfo.PlayerState.MovingToTown)
            {
                targetPos.x = Mathf.Clamp(targetPos.x, pathArea.bounds.min.x, pathArea.bounds.max.x);
                targetPos.y = Mathf.Clamp(targetPos.y, pathArea.bounds.min.y, pathArea.bounds.max.y);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, PlayerInfo.instance.speed * Time.deltaTime);
        }        
    }

    void UpdateFacingDirection(Vector3 targetPosition)
    {
        if (targetPosition.x >= transform.position.x)
        {
            if (!PlayerInfo.instance.facingRight)
            {
                PlayerInfo.instance.facingRight = true;
            }
        }
        else
        {
            if (PlayerInfo.instance.facingRight)
            {
                PlayerInfo.instance.facingRight = false;
            }
        }
    }
}
