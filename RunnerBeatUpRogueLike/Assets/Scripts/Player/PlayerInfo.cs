using UnityEngine;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour{
    
    public enum PlayerState
    {
        MovingForward,
        MovingToPosition,        
        MovingToTarget,
        MovingToTown,
        Fighting,        
        EnemiesAttackingTown,        
        Dead,
        InTown
    }

    public struct PlayerAttributes
    {
        public int lv;
        public int experience;
        public int pointsToSpend;
        public int strength;
        public int endurance;
        public int agility;
        public int gold;

        public int townLevel;
        public int townDefCap;
        public int townChanceToKill;

        public int equipedWeaponIndex;
        public int equipedArmorIndex;
    }

    public int expToNextLevel;
    public float consLevelUp = 0.04f;

    public int atkRateLogBase = 2;

    public PlayerAttributes playerAttributes;
    public static PlayerInfo instance = null;

    [HideInInspector] public int currentHp;
    [HideInInspector] public BaseEnemy focusedEnemy = null;
    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public bool isBasicAttackOnCooldown = false;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public List<BaseEnemy> engagedEnemies;
    [HideInInspector] public int maxHp;

    public int initialHealthValue = 10;
    public float attackRange;
    public float attackRate;
    public int attackDamage;
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;

    public BaseWeapon weapon;
    public BaseArmor armor;

    public int goldEarned = 0;    
    public int maxEnemiesInTown;
    public int enemiesInTown = 0;

    public LayerMask interactiveObjectsLayer;

    public Transform townPos;

    public PlayerState state;

    void Awake()
    {
        if (instance == null)
        {            
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {        
        EventManager.onEnemyDeath += EnemyDied;
        EventManager.onEnemyArrivedInTown += EnemyArrivedInTown;        
    }

    void OnDisable()
    {
        EventManager.onEnemyDeath -= EnemyDied;
        EventManager.onEnemyArrivedInTown -= EnemyArrivedInTown;
    }
        
    void Update()
    {
        UpdateDistance();
        CheckGoBackToTown();
    }

    public void StartAttack()
    {
        goldEarned = 0;
        currentHp = maxHp;
        engagedEnemies = new List<BaseEnemy>();
        enemiesInTown = 0;
        distanceWalked = 0;

        state = PlayerState.MovingForward;
    }    

    public void SetState(PlayerState newState)
    {
        state = newState;
    }

    public void SetNewPlayerAttributes()
    {
        playerAttributes = new PlayerAttributes();

        playerAttributes.experience = 0;
        playerAttributes.lv = 1;
        playerAttributes.pointsToSpend = 0;
        playerAttributes.strength = 1;
        playerAttributes.endurance = 1;
        playerAttributes.agility = 1;
        playerAttributes.gold = 0;

        playerAttributes.townLevel = 1;
        playerAttributes.townDefCap = 3;
        playerAttributes.townChanceToKill = 0;

        playerAttributes.equipedWeaponIndex = 0;
        playerAttributes.equipedArmorIndex = 0;

        SetWeaponScript();
        SetArmorScript();
        CalculateNewHealthValue();
        CalculateNewAtkValues();
        CalculateNewAtkRateValue();
        CalculateNewMovSpeedValue();
        CalculateNewDmgReductionValue();

        GameManager.instance.LoadTown();
    }

    public void CalculateNextLevelExperience()
    {
        expToNextLevel = (int)Mathf.Pow((playerAttributes.lv / consLevelUp), 2);
    }

    public void CheckLvUp()
    {
        if (playerAttributes.experience >= expToNextLevel)
        {
            playerAttributes.pointsToSpend += 1;
            playerAttributes.lv++;
            CalculateNextLevelExperience();
        }
    }

    void UpdateDistance()
    {
        if (state == PlayerState.MovingForward)
        {
            distanceWalked += Time.deltaTime;
        }
    }

    public void CalculateNewHealthValue()
    {
        maxHp = Mathf.RoundToInt(initialHealthValue + (playerAttributes.lv / 2) + playerAttributes.endurance);
    }

    public void CalculateNewAtkValues()
    {
        attackDamage = Mathf.RoundToInt((playerAttributes.strength * 0.8f) + (weapon.damage * 0.8f));
    }    

    public void CalculateNewAtkRateValue()
    {
        attackRate = weapon.atkRate + Mathf.Log(1 + (playerAttributes.agility/10.0f));              
    }

    public void CalculateNewMovSpeedValue()
    {

    }

    public void CalculateNewDmgReductionValue()
    {

    }

    void EarnGold(int amount)
    {
        goldEarned += amount;
    }

    void EarnExperience(int aumount)
    {
        playerAttributes.experience += aumount;
        CheckLvUp();
    }

    void EnemyDied(BaseEnemy enemy)
    {
        if(engagedEnemies.Contains(enemy))
        {
            engagedEnemies.Remove(enemy);
        }

        focusedEnemy = null;
        EarnGold(enemy.goldValue);
        EarnExperience(enemy.experienceValue);


        if (engagedEnemies.Count > 0)
        {
            focusedEnemy = engagedEnemies[0];
            focusedEnemy.SetFocus(true);
            targetObject = focusedEnemy.gameObject;
            state = PlayerState.MovingToTarget;
            return;
        }              
        
        state = PlayerState.MovingForward;
    }

    void EnemyArrivedInTown(BaseEnemy enemy)
    {
        //print(gameObject.name + ": Enemy arrived in town");
        enemiesInTown++;

        if (engagedEnemies.Contains(enemy))
        {
            engagedEnemies.Remove(enemy);
        }

        if (enemiesInTown >= maxEnemiesInTown)
        {
            EventManager.OnEnemiesAttakingTown();
            focusedEnemy = null;
            state = PlayerState.EnemiesAttackingTown;
        }
    }

    void CheckGoBackToTown()
    {
        if (state == PlayerState.Dead)
        {
            Invoke("GoBackToTown", 3);
            return;
        }

        if (state == PlayerState.EnemiesAttackingTown)
        {
            Invoke("GoBackToTown", 1.5f);
            return;
        }
    }

    void GoBackToTown()
    {
        playerAttributes.gold = goldEarned;
        townPos = GameObject.Find("TownPos").transform;
        targetPos = townPos.position;
        state = PlayerState.MovingToTown;
    }

    public void SetWeaponScript()
    {
        weapon = GameManager.instance.weapons[playerAttributes.equipedWeaponIndex].GetComponent<BaseWeapon>();        
    }

    public void SetArmorScript()
    {
        armor = GameManager.instance.armors[playerAttributes.equipedArmorIndex].GetComponent<BaseArmor>();
    }
}
