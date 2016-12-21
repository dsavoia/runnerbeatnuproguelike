using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update ()
    {
        CheckClickOnScreen();
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
                    playerInfo.targetPos = hitInfo.point;
                    playerInfo.SetState(PlayerInfo.PlayerState.MovingToPosition);
                }

                if(hitInfo.collider.gameObject.tag == "Enemy")
                {
                    playerInfo.targetPos = hitInfo.point;
                    playerInfo.SetState(PlayerInfo.PlayerState.MovingToTarget);
                }
            }
        }
    }
}
