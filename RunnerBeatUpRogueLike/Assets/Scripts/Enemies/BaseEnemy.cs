using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour {

    public enum BaseEnemyState
    {
        Moving,
        Fighting,
        Dead   
    }

    public int hp;
    public int attackDamage;
    public int defense;
    public float attackRate; //attacks per second
    public float Speed;
    public int goldValue;

    BaseEnemyState state;

	// Use this for initialization
	void Start ()
    {
        state = BaseEnemyState.Moving;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(state == BaseEnemyState.Moving)
        {
            Move();
        }
	}

    protected virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + Vector2.left.x, transform.position.y), Speed * Time.deltaTime);
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
}
