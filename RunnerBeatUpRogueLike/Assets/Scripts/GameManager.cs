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

        public int equipedWeaponIndex;
        public int equipedArmorIndex;

    }

    public static GameManager instance = null;
    public PlayerAttributes playerAttributes;

    public string saveFileName = "RunnerRogueLikeBeatEmUp";
    public GameObject continueButton;

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
        gameData.pointsToSpend = playerAttributes.pointsToSpend;
        gameData.strenght = playerAttributes.strenght;
        gameData.endurance = playerAttributes.endurance;
        gameData.agility = playerAttributes.agility;
        gameData.gold = playerAttributes.gold;
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
                
        playerAttributes.experience = gameData.lv;
        playerAttributes.lv = gameData.experience;
        playerAttributes.pointsToSpend = gameData.pointsToSpend;
        playerAttributes.strenght = gameData.strenght;
        playerAttributes.endurance = gameData.endurance;
        playerAttributes.agility = gameData.agility;
        playerAttributes.gold = gameData.gold;

        playerAttributes.equipedWeaponIndex = gameData.equipedWeaponIndex;
        playerAttributes.equipedArmorIndex = gameData.equipedArmorIndex;

    }
    
    public void ClearGameData()
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveFileName + ".dat"))
        {
            File.Delete(Application.persistentDataPath + "/" + saveFileName + ".dat");
        }
    }

    public void ContinueGameButton()
    {
        if(File.Exists(Application.persistentDataPath + "/" + saveFileName + ".dat"))
        {
            continueButton.SetActive(true);
        }
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
        ContinueGameButton();
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

    public int equipedWeaponIndex;
    public int equipedArmorIndex;
}