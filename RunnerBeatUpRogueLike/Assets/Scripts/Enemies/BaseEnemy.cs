using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

    public enum BaseEnemyState
    {
        Moving,
        MovingToPlayer,
        FightingAtk,
        FightingCoolDown,
        Dead   
    }

    public Transform graphics;
    bool facingLeft = true;

    #region attributes vars
    public int hp;
    public int attackDamage;
    public int defense;
    public float attackRate;
    public float attackRange;
    public float Speed;
    public int goldValue;
    #endregion

    #region viewArea vars
    public float viewDistance;
    public LayerMask viewLayer;   
    public float dstBetweenRays = 0.2f;
    public bool horizontalView = true;
    public bool verticalView;
    protected int horizontalRayCount;
    protected int verticalRayCount;
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;
    public Vector2 boundsOffset;
    #endregion


    GameObject player = null;
    BoxCollider2D enemyBoxCollider2D;
    BoxCollider2D playerBoxCollider2D;
    Bounds enemyBounds;
    Bounds playerBounds;

    BaseEnemyState state;

    protected virtual void Start ()
    {
        state = BaseEnemyState.Moving;
        enemyBoxCollider2D = GetComponent<BoxCollider2D>();
        enemyBounds = enemyBoxCollider2D.bounds;
        CalculateRaySpacing();
    }

    protected virtual void Update ()
    {
        enemyBounds = enemyBoxCollider2D.bounds;
        enemyBounds.Expand(boundsOffset);
        //DrawBoundsRect(); // debug Rect

        switch (state)
        {
            case (BaseEnemyState.Moving):
                Move(new Vector2(transform.position.x + Vector2.left.x, transform.position.y));
                LookForPlayer();
            break;
            case (BaseEnemyState.MovingToPlayer):
                MoveToPlayer();
            break;
            case (BaseEnemyState.FightingAtk):
                Fight();
            break;
        }       
	}

    void LateUpdate()
    {
        graphics.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }  

    protected virtual void Move(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
    }

    protected virtual void MoveToPlayer()
    {   
        LookAtPlayer();
        Move(playerBounds.center);

        Debug.DrawLine(enemyBounds.center, playerBounds.center, Color.yellow);
        RaycastHit2D hit = Physics2D.Linecast(enemyBounds.center, playerBounds.center, viewLayer);
        if (hit.distance <= attackRange)
        {   
            state = BaseEnemyState.FightingAtk;
        }               
    }

    protected virtual void LookForPlayer()
    {
        if (horizontalView)
        {
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = enemyBounds.max;
                rayOrigin += Vector2.down * (horizontalRaySpacing * i);               

                Debug.DrawRay(rayOrigin, Mathf.Sign(transform.localScale.x) * Vector2.left * viewDistance, Color.red);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Mathf.Sign(transform.localScale.x) * Vector2.left, viewDistance, viewLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        state = BaseEnemyState.MovingToPlayer;
                        player = hit.collider.gameObject;
                        playerBoxCollider2D = hit.collider.GetComponent<BoxCollider2D>();
                        MoveToPlayer();
                        return;
                    }
                }
            }
        }

        if (verticalView)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = enemyBounds.max;
                rayOrigin += Vector2.left * (verticalRaySpacing * i);            

                Debug.DrawRay(rayOrigin, Vector2.down * viewDistance, Color.red);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, viewDistance, viewLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        state = BaseEnemyState.MovingToPlayer;
                        player = hit.collider.gameObject;
                        playerBoxCollider2D = hit.collider.GetComponent<BoxCollider2D>();
                        MoveToPlayer();
                        return;
                    }
                }
            }
        }
    }

    protected virtual void Fight()
    {
        Debug.DrawLine(enemyBounds.center, player.transform.position, Color.yellow);
        RaycastHit2D hit = Physics2D.Linecast(enemyBounds.center, player.transform.position, viewLayer);

        LookAtPlayer();

        if (hit.distance > attackRange)
        {
            state = BaseEnemyState.MovingToPlayer;
            return;
        }


    }

    protected virtual void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        EventManager.OnEnemyDeath(this);
        Destroy(gameObject);
        ///TODO: Death animation
    }    

    void CalculateRaySpacing()
    {
        enemyBounds.Expand(boundsOffset);
        float boundsWidth = enemyBounds.size.x;
        float boundsHeight = enemyBounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = enemyBounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = enemyBounds.size.x / (verticalRayCount - 1);
    }

    void LookAtPlayer()
    {
        playerBounds = playerBoxCollider2D.bounds;

        if (player.transform.position.x > transform.position.x)
        {
            if (facingLeft)
            {
                Flip();
                facingLeft = false;
            }
        }
        else
        {
            if (!facingLeft)
            {
                Flip();
                facingLeft = true;
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = graphics.localScale;
        localScale.x *= -1;
        graphics.localScale = localScale;
    }

    //Debug function
    void DrawBoundsRect()
    {
        Vector2 bottomLeft = new Vector2(enemyBounds.min.x, enemyBounds.min.y);
        Vector2 bottomRight = new Vector2(enemyBounds.max.x, enemyBounds.min.y);
        Vector2 topLeft = new Vector2(enemyBounds.min.x, enemyBounds.max.y);
        Vector2 topRight = new Vector2(enemyBounds.max.x, enemyBounds.max.y);

        Debug.DrawLine(bottomLeft, topLeft, Color.blue);
        Debug.DrawLine(topLeft, topRight, Color.blue);
        Debug.DrawLine(topRight, bottomRight, Color.blue);
        Debug.DrawLine(bottomRight, bottomLeft, Color.blue);
    }
}
