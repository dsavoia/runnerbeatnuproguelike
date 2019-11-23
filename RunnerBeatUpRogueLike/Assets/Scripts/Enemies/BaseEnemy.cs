using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour, IAttacker, ICombatTarget
{

    public enum BaseEnemyState
    {
        Moving,
        MovingToPlayer,
        Fighting,
        PlayerDied,
        Celebrating,        
        Dead   
    }

    bool isFirstAttack = true;

    public Transform graphics;
    public GameObject selectionBorder;
    BoxCollider2D pathArea;
    SpriteRenderer spriteRenderer;
    Color defaultColor;
    bool facingLeft = true;

    #region attributes vars
    public int maxHp;
    int currentHp;
    public int attackDamage;
    public int defense;
    public float attackCooldown;
    public float attackRange;
    public float Speed;
    public int goldValue;
    public int experienceValue;
    #endregion

    float lastAttack;

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
        
    public GameObject healthBarParent;
    public Image healthBar;    

    int celebrationDir = 1;

    GameObject player = null;
    BoxCollider2D enemyBoxCollider2D;
    BoxCollider2D playerBoxCollider2D;
    Bounds enemyBounds;
    Bounds playerBounds;
    ICombatTarget target;    

    BaseEnemyState state;   

    protected virtual void Start ()
    {
        pathArea = GameObject.FindGameObjectWithTag("Path").GetComponent<BoxCollider2D>();
        EventManager.onPlayerDeath += PlayerDied;
        EventManager.onEnemyArrivedInTown += AllyArrivedInTown;
        EventManager.onEnemiesAttackingTown += PlayerDied;
               
        currentHp = maxHp;

        spriteRenderer = graphics.gameObject.GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        state = BaseEnemyState.Moving;
        enemyBoxCollider2D = GetComponent<BoxCollider2D>();
        enemyBounds = enemyBoxCollider2D.bounds;
        CalculateRaySpacing();
    }

    void OnDisable()
    {
        EventManager.onPlayerDeath -= PlayerDied;
        EventManager.onEnemyArrivedInTown -= AllyArrivedInTown;
        EventManager.onEnemiesAttackingTown -= PlayerDied;
    }

    protected virtual void Update ()
    {
        if (!GameManager.instance.isPaused)
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
                case (BaseEnemyState.Fighting):
                    Fight();
                    break;
                case (BaseEnemyState.PlayerDied):
                    state = BaseEnemyState.Celebrating;
                    SetFocus(false);
                    Celebrate();
                    break;

            }
        }
	}

    void LateUpdate()
    {
        int sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        graphics.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        selectionBorder.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;       
    }  

    protected virtual void Move(Vector2 target)
    {
        target.y = Mathf.Clamp(target.y, pathArea.bounds.min.y, pathArea.bounds.max.y);
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
            state = BaseEnemyState.Fighting;
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
                        GetPlayerInfo(hit);                      
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
                        GetPlayerInfo(hit);
                        return;
                    }
                }
            }
        }
    }

    void GetPlayerInfo(RaycastHit2D hit)
    {
        state = BaseEnemyState.MovingToPlayer;
        player = hit.collider.gameObject;
        target = player.GetComponent<PlayerActions>();        
        playerBoxCollider2D = hit.collider.GetComponent<BoxCollider2D>();
        MoveToPlayer();
    }

    protected virtual void Fight()
    {
        Debug.DrawLine(enemyBounds.center, player.transform.position, Color.yellow);
        RaycastHit2D hit = Physics2D.Linecast(enemyBounds.center, player.transform.position, viewLayer);

        LookAtPlayer();

        if (hit.distance > attackRange)
        {
            if (PlayerInfo.instance.engagedEnemies.Contains(this))
            {
                PlayerInfo.instance.engagedEnemies.Remove(this);
            }
            state = BaseEnemyState.MovingToPlayer;
            return;
        }
        else
        {
            if (!PlayerInfo.instance.engagedEnemies.Contains(this))
            {
                PlayerInfo.instance.engagedEnemies.Add(this);
            }
        }

        if (isFirstAttack)
        {
            isFirstAttack = false;
            lastAttack = Time.time - attackCooldown/2;
        }

        if(Time.time > lastAttack + attackCooldown)
        {
            //playerActions.TakeDamage(attackDamage);
            GetTarget().Defend(Attack());
            StartCoroutine(TempAttackAnim());
            lastAttack = Time.time;
        }
    }

    IEnumerator TempAttackAnim()
    {
        spriteRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = defaultColor;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHp / (float)maxHp;
    }

    public void SetFocus(bool focus)
    {   
        if(focus)
        {
            healthBarParent.SetActive(true);
            selectionBorder.SetActive(true);
        }
        else
        {
            healthBarParent.SetActive(false);
            selectionBorder.SetActive(false);
        }        
    }

    protected virtual void Die()
    {
        EventManager.OnEnemyDeath(this);        
        Destroy(gameObject);
        ///TODO: Death animation
    }

    void AllyArrivedInTown(BaseEnemy enemy)
    {
        ///TODO: Make enemies cheer!
    }

    void PlayerDied()
    {
        state = BaseEnemyState.PlayerDied;
    }

    void Celebrate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (0.2f * celebrationDir), transform.position.z);
        celebrationDir *= -1;
        Invoke("Celebrate", Random.Range(0.2f, 0.8f));
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

        localScale = selectionBorder.transform.localScale;
        localScale.x *= -1;
        selectionBorder.transform.localScale = localScale;
        
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

    #region IAttacker
    public List<IAttack> Attack()
    {
        List<IAttack> attacks = new List<IAttack>();
        attacks.Add(new BasicAttack(attackDamage));
        return attacks;
    }

    /// <summary>
    /// IAttacker interface
    /// </summary>
    /// <returns></returns>
    public ICombatTarget GetTarget()
    {
        return target;
    }

    #endregion

    #region ICombatTarget
    public void Defend(List<IAttack> attacks)
    {
        int damage = 0;
        foreach (IAttack attack in attacks)
        {
            damage += attack.GetDamage();
        }
        TakeDamage(damage);
    }
    #endregion

}
