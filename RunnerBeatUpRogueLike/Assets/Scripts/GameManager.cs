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
       public int strenght;
       public int endurance;
       public int agility;
       public int gold;
    }

    public static GameManager instance = null;
    public PlayerAttributes playerAttributes;

    public string saveFileName = "RunnerRogueLikeBeatEmUp";

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            playerAttributes = new PlayerAttributes();
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName + ".dat");

        GameData gameData = new GameData();

        gameData.experience = playerAttributes.lv;
        gameData.lv = playerAttributes.experience;
        gameData.strenght = playerAttributes.strenght;
        gameData.endurance = playerAttributes.endurance;
        gameData.agility = playerAttributes.agility;
        gameData.gold = playerAttributes.gold;

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

        playerAttributes.lv = gameData.experience;
        playerAttributes.experience = gameData.lv;
        playerAttributes.strenght = gameData.strenght;
        playerAttributes.endurance = gameData.endurance;
        playerAttributes.agility = gameData.agility;
        playerAttributes.gold = gameData.gold;
    }
    
    // Maybe do a towns Buttons?
    public void LoadTown()
    {
        SceneManager.LoadScene("Town");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

[Serializable]
public class GameData
{    
    public int lv;
    public int experience;
    public int strenght;
    public int endurance;
    public int agility;
    public int gold;
}