using UnityEngine;

public class PlayerInfo : MonoBehaviour{

    public enum PlayerState
    {
        isMoving,
        isFighting,
        isFightingBoss,
        isDead,
    }

    #region MovementInfo
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;
    #endregion MovementInfo    

    public int goldEarned = 0;

    public PlayerState state;   

    void Start()
    {
        state = PlayerState.isMoving;
    }

    void Update()
    {
        UpdateInfo();
    }

    public void ChangeState(PlayerState newState)
    {
        state = newState;
    }

    void UpdateInfo()
    {
        if (facingRight)
        {
            distanceWalked += Time.deltaTime;
        }
    }

    void EarnGold(int amount)
    {
        goldEarned += amount;
    }

}
