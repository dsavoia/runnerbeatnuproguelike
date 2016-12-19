using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    PlayerInfo playerInfo;
    Vector3 targetPos;    
    bool goingToTargetpos;    
    
    void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }
	
	void Update ()
    {
        CheckClickOnScreen();
        Move();
    }

    void Move()
    {
        if (goingToTargetpos)
        {
            if (transform.position == targetPos)
            {
                goingToTargetpos = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, playerInfo.speed * Time.deltaTime);

                if (playerInfo.facingRight)
                {
                    playerInfo.ChangeState(PlayerInfo.PlayerState.Moving);
                }
            }
        }
        else
        {
            if (!playerInfo.facingRight)
            {
                playerInfo.facingRight = true;
                Flip();
            }
        }
    }

    void CheckClickOnScreen()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(clickPos.x, clickPos.y), Vector2.zero, 0);

            if (hitInfo)
            {
                //print("Clicked on: " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.gameObject.tag == "Path")
                {
                    targetPos = hitInfo.point;
                    if (hitInfo.point.x >= transform.position.x)
                    {
                        if (!playerInfo.facingRight)
                        {
                            playerInfo.facingRight = true;
                            Flip();
                        }                               
                    }
                    else
                    {
                        if (playerInfo.facingRight)
                        {
                            playerInfo.facingRight = false;
                            Flip();
                        }
                    }

                    goingToTargetpos = true;
                }
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}
