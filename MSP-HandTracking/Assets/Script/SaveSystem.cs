using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    //allows to store the outfits created to load them as needed
    public static List<PlayerData> saved = new List<PlayerData>();
    public static int currentLevel, reachedLevel;

    public static void SavePlayer()
    {
        Game.current.thePlayer.currentLevel = PlayerPrefs.GetInt("Level");
        Game.current.thePlayer.levelReached = PlayerPrefs.GetInt("MaxLevel");

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData
        {
            currentLevel = Game.current.thePlayer.currentLevel,
            levelReached = Game.current.thePlayer.levelReached
        };
        Debug.Log("Saved current level : " + Game.current.thePlayer.currentLevel);
        Debug.Log("Saved max level : " + Game.current.thePlayer.levelReached);
        //adds newly saved outfit to our file to be loadable later
        saved.Add(data);
        formatter.Serialize(file, saved);//converts player data to binary file
        file.Close();

    }
    //fetches the file and loads its data
    public static void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            saved = (List<PlayerData>)formatter.Deserialize(stream);
            stream.Close();
            //load the player with the latest saved outfit
            currentLevel = saved[(saved.Count - 1)].currentLevel;
            reachedLevel = saved[(saved.Count - 1)].levelReached;
            Debug.Log("Loaded current level : " + currentLevel);
            Debug.Log("Loaded max level : " + reachedLevel);
        }
        else
        {
            Debug.LogError("Save file not found in" + path);

        }


    }
}