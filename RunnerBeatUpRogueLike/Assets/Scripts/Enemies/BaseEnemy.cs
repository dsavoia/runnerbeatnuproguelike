using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

    public enum BaseEnemyState
    {
        Moving,
        MovingToPlayer,
        Fighting,
        Dead   
    }

    public int hp;
    public int attackDamage;
    public int defense;
    public float attackRate; //attacks per second
    public float Speed;
    public int goldValue;

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

    BoxCollider2D boxCollider2D;
    Bounds bounds;

    BaseEnemyState state;

    

    // Use this for initialization
    void Start ()
    {
        state = BaseEnemyState.Moving;
        boxCollider2D = GetComponent<BoxCollider2D>();
        bounds = boxCollider2D.bounds;
        CalculateRaySpacing();
    }
	
	// Update is called once per frame
	void Update ()
    {
        bounds = boxCollider2D.bounds;
        bounds.Expand(boundsOffset);
        DrawRect(); // debug Rect

        if (state == BaseEnemyState.Moving)
        {
            Move();
            LookForPlayer();
        }       
	}

    protected virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + Vector2.left.x, transform.position.y), Speed * Time.deltaTime);
    }

    protected virtual void LookForPlayer()
    {
        //print(horizontalRayCount);

        if (horizontalView)
        {
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = bounds.max;
                rayOrigin += Vector2.down * (horizontalRaySpacing * i);
                //print("rayOrigin " + rayOrigin );

                Debug.DrawRay(rayOrigin, Mathf.Sign(transform.localScale.x) * Vector2.left * viewDistance, Color.red);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Mathf.Sign(transform.localScale.x) * Vector2.left, viewDistance, viewLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        state = BaseEnemyState.MovingToPlayer;
                        return;
                    }
                }
            }
        }

        if (verticalView)
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = bounds.max;
                rayOrigin += Vector2.left * (verticalRaySpacing * i);
                //print("rayOrigin " + rayOrigin );

                Debug.DrawRay(rayOrigin, Vector2.down * viewDistance, Color.red);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, viewDistance, viewLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Player")
                    {
                        return;
                    }
                }
            }
        }
    }

    protected virtual void Attack(GameObject target)
    {

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

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    protected void CalculateRaySpacing()
    {
        bounds.Expand(boundsOffset);
        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    //Debug function
    void DrawRect()
    {
        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        Vector2 topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 topRight = new Vector2(bounds.max.x, bounds.max.y);

        Debug.DrawLine(bottomLeft, topLeft, Color.blue);
        Debug.DrawLine(topLeft, topRight, Color.blue);
        Debug.DrawLine(topRight, bottomRight, Color.blue);
        Debug.DrawLine(bottomRight, bottomLeft, Color.blue);
    }
}
