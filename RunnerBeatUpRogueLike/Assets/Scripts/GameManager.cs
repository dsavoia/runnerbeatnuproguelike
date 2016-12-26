using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : MonoBehaviour {
       
    public struct PlayerAttributes
    {
        public int lv;
        public int experience;
        public int pointsToSpend;
        public int strenght;
        public int endurance;
        public int agility;
        public int gold;

        public int townLevel;
        public int townDefCap;
        public int townChanceToKill;

        public int equipedWeaponIndex;
        public int equipedArmorIndex;

    }

    public static GameManager instance = null;
    public PlayerAttributes playerAttributes;

    public string saveFileName = "RunnerRogueLikeBeatEmUp";   

    // Use this for initialization
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

    // Maybe do a towns Buttons?
    public void LoadTown()
    {        
        SaveGame();
        SceneManager.LoadScene("Town");
    }

    public void LoadGameScene()
    {        
        SceneManager.LoadScene("Game");
        PlayerInfo.instance.StartAttack();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");        
    }

    public void StartNewGame()
    {        
        playerAttributes = new PlayerAttributes();

        playerAttributes.experience = 0;
        playerAttributes.lv = 1;
        playerAttributes.pointsToSpend = 0;
        playerAttributes.strenght = 1;
        playerAttributes.endurance = 1;
        playerAttributes.agility = 1;
        playerAttributes.gold = 0;

        playerAttributes.townLevel = 1;
        playerAttributes.townDefCap = 3;
        playerAttributes.townChanceToKill = 0;

        playerAttributes.equipedWeaponIndex = 0;
        playerAttributes.equipedArmorIndex = 0;

        LoadTown();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName + ".dat");

        GameData gameData = new GameData();

        gameData.lv = playerAttributes.lv;
        gameData.experience = playerAttributes.experience;        
        gameData.pointsToSpend = playerAttributes.pointsToSpend;
        gameData.strenght = playerAttributes.strenght;
        gameData.endurance = playerAttributes.endurance;
        gameData.agility = playerAttributes.agility;
        gameData.gold = playerAttributes.gold;        

        gameData.townLevel = playerAttributes.townLevel;
        gameData.townDefCap = playerAttributes.townDefCap;
        gameData.townChanceToKill = playerAttributes.townChanceToKill;

        gameData.equipedWeaponIndex = playerAttributes.equipedWeaponIndex;
        gameData.equipedArmorIndex = playerAttributes.equipedArmorIndex;

        bf.Serialize(file, gameData);
        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + saveFileName + ".dat", FileMode.Open);
        GameData gameData = (GameData)bf.Deserialize(file);
        file.Close();

        playerAttributes = new PlayerAttributes();

        playerAttributes.lv = gameData.lv;
        playerAttributes.experience = gameData.experience;        
        playerAttributes.pointsToSpend = gameData.pointsToSpend;
        playerAttributes.strenght = gameData.strenght;
        playerAttributes.endurance = gameData.endurance;
        playerAttributes.agility = gameData.agility;
        playerAttributes.gold = gameData.gold;

        playerAttributes.townLevel = gameData.townLevel;
        playerAttributes.townDefCap = gameData.townDefCap;
        playerAttributes.townChanceToKill = gameData.townChanceToKill;

        playerAttributes.equipedWeaponIndex = gameData.equipedWeaponIndex;
        playerAttributes.equipedArmorIndex = gameData.equipedArmorIndex;

        LoadTown();

    }
    
    public void ClearGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveFileName + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + saveFileName + ".dat");
        }
    }
    
}

[Serializable]
public class GameData
{    
    public int lv;
    public int experience;
    public int pointsToSpend;
    public int strenght;
    public int endurance;
    public int agility;
    public int gold;

    public int townLevel;
    public int townDefCap;
    public int townChanceToKill;

    public int equipedWeaponIndex;
    public int equipedArmorIndex;
}