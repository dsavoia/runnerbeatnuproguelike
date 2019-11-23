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

        public int equipedWeaponID;       

        public List<int> inventory;
    }

    public int expToNextLevel;
    public float consLevelUp = 0.04f;

    public int atkRateLogBase = 2;

    public PlayerAttributes playerAttributes;
    public static PlayerInfo instance = null;

    [HideInInspector] public int currentHp;
    //todo change focusedEnemy to ISimpleCombat
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
    public IAttack basicAttack;
    public int attackDamage;
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;        
    
    public GameObject weaponGO;
    public BasicWeapon weapon;

    public int goldEarned = 0;    
    public int maxEnemiesInTown;
    public int enemiesInTown = 0;

    public LayerMask interactiveObjectsLayer;

    public Transform townPos;

    public PlayerState state;

    List<BaseItem> itemsCol;

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
        itemsCol = GameManager.instance.itemsCollection.items;
        weapon = weaponGO.GetComponent<BasicWeapon>();
    }

    void OnDisable()
    {
        EventManager.onEnemyDeath -= EnemyDied;
        EventManager.onEnemyArrivedInTown -= EnemyArrivedInTown;
    }
        
    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            UpdateDistance();
            CheckGoBackToTown();
        }
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

        //TODO: REMOVE THIS VALUE. FOR TEST ONLY
        playerAttributes.gold = 99999;

        playerAttributes.townLevel = 1;
        playerAttributes.townDefCap = 3;
        playerAttributes.townChanceToKill = 0;

        playerAttributes.equipedWeaponID = 0;

        playerAttributes.inventory = new List<int>();        
        playerAttributes.inventory.Add(playerAttributes.equipedWeaponID);
        
        CalculateNewHealthValue();
        EquipWeapon(playerAttributes.equipedWeaponID);        
        CalculateNewMovSpeedValue();

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

    public void EquipWeapon(int weaponID)
    {
        playerAttributes.equipedWeaponID = weaponID;
        BaseItem weaponItem = new BaseItem();
        weaponItem = itemsCol.Find(w => w.itemID == weaponID);
        weapon.SetAttackDamage(weaponItem.attackDamage);
        weapon.SetAttackRate(weaponItem.attackRate);
        weapon.SetName(weaponItem.name);
        CalculateNewAtkValues();
        CalculateNewAtkRateValue();
    }

    public void CalculateNewAtkValues()
    {
        attackDamage = 0;
        basicAttack = new BasicAttack(Mathf.RoundToInt(playerAttributes.strength * 0.8f));
        attackDamage += basicAttack.GetDamage();       
        attackDamage += Mathf.RoundToInt(weapon.GetDamage() * 0.8f);       
        //attackDamage = Mathf.RoundToInt((playerAttributes.strength * 0.8f) + (weapon.GetDamage() * 0.8f));
    }    

    public void CalculateNewAtkRateValue()
    {
        attackRate = weapon.GetAttackRate() + Mathf.Log(1 + (playerAttributes.agility/10.0f));              
    }

    public void CalculateNewMovSpeedValue()
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
            EventManager.OnEnemiesAttackingTown();
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
}
