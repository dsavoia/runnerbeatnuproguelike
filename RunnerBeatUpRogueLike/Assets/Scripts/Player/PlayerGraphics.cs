using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {

    public Transform graphics;
    PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    void LateUpdate()
    {
        graphics.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        if(playerInfo.facingRight)
        {
            if(Mathf.Sign(transform.localScale.x) < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
        }
        else
        {
            if (Mathf.Sign(transform.localScale.x) > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            }
        }
    }    
}
