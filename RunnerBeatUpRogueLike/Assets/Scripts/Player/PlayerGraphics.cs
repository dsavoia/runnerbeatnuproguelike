using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour {

    public Transform graphics;    
    SpriteRenderer spriteRenderer;
    Color defaultColor;

    void Start()
    {        
        spriteRenderer = graphics.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    void Update()
    {
        switch (PlayerInfo.instance.state)
        {
            case (PlayerInfo.PlayerState.Fighting):
                if (!PlayerInfo.instance.isBasicAttackOnCooldown)
                {
                    StartCoroutine(TempAttackAnim());
                    PlayerInfo.instance.isBasicAttackOnCooldown = true;
                }
            break;

            case (PlayerInfo.PlayerState.Dead):
                spriteRenderer.color = Color.red;
            break;

            case (PlayerInfo.PlayerState.EnemiesAttackingTown):
                spriteRenderer.color = Color.gray;
            break;
        }
    }

    void LateUpdate()
    {
        graphics.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        if (PlayerInfo.instance.facingRight)
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

    IEnumerator TempAttackAnim()
    {
        spriteRenderer.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = defaultColor;
    }
}
