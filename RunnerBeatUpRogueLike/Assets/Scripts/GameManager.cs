using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
        
    public string saveFileName = "RunnerRogueLikeBeatEmUp";

    public ItemContainer itemsCollection;
    bool loadItemsXML = true; 

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
        itemsCollection = ItemContainer.Load();
    }   

    public void StartNewGame()
    {
        PlayerInfo.instance.SetNewPlayerAttributes();
    }
    
    public void LoadTown()
    {        
        SaveGame();

        if(loadItemsXML)
        {            
            loadItemsXML = false;
        }

        SceneManager.LoadScene("Town");
    }

    public void LoadGameScene()
    {        
        SceneManager.LoadScene("Game");
        PlayerInfo.instance.StartAttack();
    }

    public void MainMenu()
    {
        SaveGame();
        SceneManager.LoadScene("MainMenu");        
    }
     
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName + ".dat");

        GameData gameData = new GameData();

        gameData.lv = PlayerInfo.instance.playerAttributes.lv;
        gameData.experience = PlayerInfo.instance.playerAttributes.experience;        
        gameData.pointsToSpend = PlayerInfo.instance.playerAttributes.pointsToSpend;
        gameData.strength = PlayerInfo.instance.playerAttributes.strength;
        gameData.endurance = PlayerInfo.instance.playerAttributes.endurance;
        gameData.agility = PlayerInfo.instance.playerAttributes.agility;
        gameData.gold = PlayerInfo.instance.playerAttributes.gold;        

        gameData.townLevel = PlayerInfo.instance.playerAttributes.townLevel;
        gameData.townDefCap = PlayerInfo.instance.playerAttributes.townDefCap;
        gameData.townChanceToKill = PlayerInfo.instance.playerAttributes.townChanceToKill;

        gameData.equipedWeaponID = PlayerInfo.instance.playerAttributes.equipedWeaponID;
        gameData.inventory = new int[PlayerInfo.instance.playerAttributes.inventory.Count];

        for (int i = 0; i < PlayerInfo.instance.playerAttributes.inventory.Count; i++)
        {
            gameData.inventory[i] = PlayerInfo.instance.playerAttributes.inventory[i];
        }       

        bf.Serialize(file, gameData);
        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + saveFileName + ".dat", FileMode.Open);
        GameData gameData = (GameData)bf.Deserialize(file);
        file.Close();

        PlayerInfo.instance.playerAttributes = new PlayerInfo.PlayerAttributes();

        PlayerInfo.instance.playerAttributes.lv = gameData.lv;
        PlayerInfo.instance.playerAttributes.experience = gameData.experience;
        PlayerInfo.instance.playerAttributes.pointsToSpend = gameData.pointsToSpend;
        PlayerInfo.instance.playerAttributes.strength = gameData.strength;
        PlayerInfo.instance.playerAttributes.endurance = gameData.endurance;
        PlayerInfo.instance.playerAttributes.agility = gameData.agility;
        PlayerInfo.instance.playerAttributes.gold = gameData.gold;

        PlayerInfo.instance.playerAttributes.townLevel = gameData.townLevel;
        PlayerInfo.instance.playerAttributes.townDefCap = gameData.townDefCap;
        PlayerInfo.instance.playerAttributes.townChanceToKill = gameData.townChanceToKill;

        PlayerInfo.instance.playerAttributes.equipedWeaponID = gameData.equipedWeaponID;

        PlayerInfo.instance.playerAttributes.inventory = new List<int>();

        foreach (int itemIndex in gameData.inventory)
        {
            PlayerInfo.instance.playerAttributes.inventory.Add(itemIndex);
        }
        
        PlayerInfo.instance.EquipWeapon(gameData.equipedWeaponID);        
        PlayerInfo.instance.CalculateNewMovSpeedValue();        
        PlayerInfo.instance.CalculateNewHealthValue();

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
    public int strength;
    public int endurance;
    public int agility;
    public int gold;

    public int townLevel;
    public int townDefCap;
    public int townChanceToKill;

    public int equipedWeaponID;    

    public int[] inventory;
}