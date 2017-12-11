using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

public class GameController : MonoBehaviour {


    public static GameController control;

    public int levelNumber = 0;
    public int [] starsEarned;
    public int[] temporaryStars;

    void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }else if(control != this)
        {
            Destroy(gameObject);
        }
        // Anzahl der Level + 1 
        // Verdiente Sterne in jedem Level
        starsEarned = new int[5] { 0, 0, 0, 0, 0 };
        temporaryStars = new int[5] { 0, 0, 0, 0, 0 };
        //Save();
        Load();
    }
    

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/TanksSave.dat");

        PlayerData data = new PlayerData();
        data.levelNumber = levelNumber;
        data.starsEarned = starsEarned;
        //int[] tempStars = new int[5];
        //data.starsEarned.CopyTo(tempStars,0);
        //for (int i = 0; i < starsEarned.Length; i++)
        //{
        //    if (starsEarned[i] >= tempStars[i]) data.starsEarned[i] = starsEarned[i];
        //}
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/TanksSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/TanksSave.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            levelNumber = data.levelNumber;
            starsEarned = data.starsEarned;
        }
        else Save();
    }

    //public void OnDestroy()
    //{
    //    Save();
    //}

}

[Serializable]
class PlayerData
{
    public int levelNumber;
    public int [] starsEarned;

}
