using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {    

    // Update is called once per frame
    void Update ()
    {
        if (!GameManager.instance.isPaused)
        {
            switch (PlayerInfo.instance.state)
            {
                case PlayerInfo.PlayerState.Dead:
                case PlayerInfo.PlayerState.MovingToTown:
                case PlayerInfo.PlayerState.EnemiesAttackingTown:
                    break;
                default:
                    CheckClickOnScreen();
                    break;
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
                    if (PlayerInfo.instance.focusedEnemy != null)
                    {
                        PlayerInfo.instance.focusedEnemy.SetFocus(false);
                        PlayerInfo.instance.focusedEnemy = null;
                    }
                    PlayerInfo.instance.targetPos = new Vector3(hitInfo.point.x, hitInfo.point.y, transform.position.z);
                    PlayerInfo.instance.SetState(PlayerInfo.PlayerState.MovingToPosition);
                }

                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    if(PlayerInfo.instance.focusedEnemy != null)
                    {
                        PlayerInfo.instance.focusedEnemy.SetFocus(false);
                    }

                    PlayerInfo.instance.focusedEnemy = hitInfo.collider.GetComponent<BaseEnemy>();
                    PlayerInfo.instance.focusedEnemy.SetFocus(true);

                    PlayerInfo.instance.targetObject = hitInfo.collider.gameObject;
                    PlayerInfo.instance.SetState(PlayerInfo.PlayerState.MovingToTarget);
                }
            }            
        }
    }
}
