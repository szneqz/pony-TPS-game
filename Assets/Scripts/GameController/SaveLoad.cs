using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{

    public static Properties2 savedGames = new Properties2();
    public static Options opt = new Options();
    public static Loadout lod = new Loadout();

    //it's static so we can call it from anywhere
    public static void Save()
    {
        SaveLoad.savedGames = StaticInfo.datScript.prop2;
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/customSkin.cust"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/customSkin.cust"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/customSkin.cust", FileMode.Open);
            SaveLoad.savedGames = (Properties2)bf.Deserialize(file);
            StaticInfo.datScript.prop2 = SaveLoad.savedGames;
            file.Close();
        }
    }

    //it's static so we can call it from anywhere
    public static void Save2()
    {
        SaveLoad.opt = StaticInfo.datScript.op;
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/options.set"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.opt);
        file.Close();
    }

    public static void Load2()
    {
        if (File.Exists(Application.persistentDataPath + "/options.set"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/options.set", FileMode.Open);
            SaveLoad.opt = (Options)bf.Deserialize(file);
            StaticInfo.datScript.op = SaveLoad.opt;
            file.Close();
        }
    }

    //it's static so we can call it from anywhere
    public static void Save3()
    {
        SaveLoad.lod = StaticInfo.datScript.lo;
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/loadout.lod"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.lod);
        file.Close();
    }

    public static void Load3()
    {
        if (File.Exists(Application.persistentDataPath + "/loadout.lod"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/loadout.lod", FileMode.Open);
            SaveLoad.lod = (Loadout)bf.Deserialize(file);
            StaticInfo.datScript.lo = SaveLoad.lod;
            file.Close();
        }
    }
}