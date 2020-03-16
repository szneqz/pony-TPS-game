using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Properties
{
    public int level = 0;
    public int playerAmount = 1;
    public int playerClass = 0;
    public bool ifMale = false;
    public int CM = 0;
    public int frontHair = 0;
    public int backHair = 0;
    public int tail = 0;
    public Color hairColor_1 = Color.white;
    public Color hairColor_2 = Color.white;
    public Color hairColor_3 = Color.white;
    public Color magicColor = Color.white;
    public Color bodyColor = Color.white;
    public Color eyesColor = Color.white;
    public string Name;
}

[System.Serializable]
public class Properties2
{
    public int level = 0;
    public int playerAmount = 1;
    public int playerClass = 0;
    public bool ifMale = false;
    public int CM = 0;
    public int frontHair = 0;
    public int backHair = 0;
    public int tail = 0;
    public float[] hairColor_1 = new float[] {1.0f, 1.0f, 1.0f, 1.0f};
    public float[] hairColor_2 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    public float[] hairColor_3 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    public float[] magicColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    public float[] bodyColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    public float[] eyesColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
    public char[] Name;
}

[System.Serializable]
public class Options
{
    public float genAudio = 0.5f;
    public float musicAudio = 0.5f;
    public float VFXAudio = 0.5f;
    public bool realTimeS = true;
}

[System.Serializable]
public class Loadout
{
    public int[] Unicorn = new int[] { 12, 10, 14 };
    public int[] Pegasus = new int[] { 0, 7, 9 };
    public int[] EarthPony = new int[] { 0, 7, 9 };
}

public class PassDataScript : MonoBehaviour
{
    public List<Texture2D> cmTexList;
    public List<Texture2D> tailTexList;
    public List<Texture2D> backHairTexList;
    public List<Texture2D> frontHairTexList;

    public List<SkinnedMeshRenderer> tailSMRList;
    public List<SkinnedMeshRenderer> backHairSMRList;
    public List<SkinnedMeshRenderer> frontHairSMRList;

    public Properties prop;
    public Properties2 prop2;
    public Options op;
    public Loadout lo;

    //public int level = 0;
    //public int playerAmount = 1;
    //public int playerClass = 0;
    //public bool ifMale = false;
    //public int CM = 0;
    //public int frontHair = 0;
    //public int backHair = 0;
    //public int tail = 0;
    //public Color hairColor_1 = Color.white;
    //public Color hairColor_2 = Color.white;
    //public Color hairColor_3 = Color.white;
    //public Color magicColor = Color.white;
    //public Color bodyColor = Color.white;
    //public Color eyesColor = Color.white;

    void Awake()
    {
        prop = new Properties();
        prop2 = new Properties2();
        op = new Options();
        lo = new Loadout();
        DontDestroyOnLoad(this.gameObject);

        GameObject[] PDSs = GameObject.FindGameObjectsWithTag("NDGO");
        foreach(GameObject i in PDSs)
        {
            if(i != this.gameObject)
            {
                Destroy(i);
            }
        }

        StaticInfo.datScript = this;
    }

    public void changingPlayerClass2(int val)
    {
        prop.playerClass = val;
    }

    public void changingPlayerAmount(AmountDisplayer amountScript)
    {
        prop.playerAmount = (int)amountScript.amount;
    }

    public void changingPlayerClass(Dropdown dropDown)
    {
        prop.playerClass = dropDown.value;
    }

    public void FromNormalToSerialized()
    {
        prop2.level = prop.level;
        prop2.playerAmount = prop.playerAmount;
        prop2.playerClass = prop.playerClass;
        prop2.ifMale = prop.ifMale;
        prop2.CM = prop.CM;
        prop2.frontHair = prop.frontHair;
        prop2.backHair = prop.backHair;
        prop2.tail = prop.tail;
        prop2.hairColor_1 = new float[]{prop.hairColor_1.r, prop.hairColor_1.g, prop.hairColor_1.b, prop.hairColor_1.a};
        prop2.hairColor_2 = new float[] { prop.hairColor_2.r, prop.hairColor_2.g, prop.hairColor_2.b, prop.hairColor_2.a };
        prop2.hairColor_3 = new float[] { prop.hairColor_3.r, prop.hairColor_3.g, prop.hairColor_3.b, prop.hairColor_3.a };
        prop2.magicColor = new float[] { prop.magicColor.r, prop.magicColor.g, prop.magicColor.b, prop.magicColor.a };
        prop2.bodyColor = new float[] { prop.bodyColor.r, prop.bodyColor.g, prop.bodyColor.b, prop.bodyColor.a };
        prop2.eyesColor = new float[] { prop.eyesColor.r, prop.eyesColor.g, prop.eyesColor.b, prop.eyesColor.a };
        prop2.Name = (char[])prop.Name.ToCharArray();
    }

    public void FromSerializedToNormal()    //typy takie jak Color musza byc zamieniane na float[4]
    {
        prop.level = prop2.level;
        prop.playerAmount = prop2.playerAmount;
        prop.playerClass = prop2.playerClass;
        prop.ifMale = prop2.ifMale;
        prop.CM = prop2.CM;
        prop.frontHair = prop2.frontHair;
        prop.backHair = prop2.backHair;
        prop.tail = prop2.tail;
        prop.hairColor_1 = new Color(prop2.hairColor_1[0], prop2.hairColor_1[1], prop2.hairColor_1[2], 1.0f);
        prop.hairColor_2 = new Color(prop2.hairColor_2[0], prop2.hairColor_2[1], prop2.hairColor_2[2], 1.0f);
        prop.hairColor_3 = new Color(prop2.hairColor_3[0], prop2.hairColor_3[1], prop2.hairColor_3[2], 1.0f);
        prop.magicColor = new Color(prop2.magicColor[0], prop2.magicColor[1], prop2.magicColor[2], 1.0f);
        prop.bodyColor = new Color(prop2.bodyColor[0], prop2.bodyColor[1], prop2.bodyColor[2], 1.0f);
        prop.eyesColor = new Color(prop2.eyesColor[0], prop2.eyesColor[1], prop2.eyesColor[2], 1.0f);
        prop.Name = new string(prop2.Name);
    }
}
