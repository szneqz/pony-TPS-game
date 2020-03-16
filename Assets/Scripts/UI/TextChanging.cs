using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanging : MonoBehaviour
{

    public WepList script;
    public bool isUnicorn = false;
    public bool isPegasus = false;
    public bool isEarthPony = false;
    public bool isPrimary = false;
    public bool isSeconadry = false;
    public bool isMelee = false;
    public Text panelText;
    public Dropdown drop;

    private void Awake()
    {
        if (!script)
        {
            script = GameObject.FindGameObjectWithTag("NDGO").GetComponent<WepList>();
        }
    }

    private void Start()
    {

        drop.ClearOptions();

        if(isUnicorn)
        {
            if(isPrimary)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[0].pri)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if(isSeconadry)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[0].sec)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if(isMelee)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[0].mel)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            //StaticInfo.datScript.lo.Unicorn[kindOf];   //Tutaj jakos muszę przemiescic informacje numerowa za pomoca wykrywania nazw w inny numer odpowiadajacy value
        }
        if(isPegasus)
        {
            if (isPrimary)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[1].pri)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if (isSeconadry)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[1].sec)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if (isMelee)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[1].mel)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            //StaticInfo.datScript.lo.Pegasus[kindOf];   //Tutaj jakos muszę przemiescic informacje numerowa za pomoca wykrywania nazw w inny numer odpowiadajacy value
        }
        if (isEarthPony)
        {
            if (isPrimary)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[2].pri)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if (isSeconadry)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[2].sec)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            if (isMelee)
            {
                List<string> tempList = new List<string>();
                foreach (GameObject a in script.guns[2].mel)
                    tempList.Add(a.name);
                drop.AddOptions(tempList);
            }
            //StaticInfo.datScript.lo.EarthPony[kindOf];   //Tutaj jakos muszę przemiescic informacje numerowa za pomoca wykrywania nazw w inny numer odpowiadajacy value
        }
        changing();
    }

    public void changing()
    {
        if (isUnicorn)
        {
            if (isPrimary)
            {
                panelText.text = script.guns[0].pri[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isSeconadry)
            {
                panelText.text = script.guns[0].sec[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isMelee)
            {
                panelText.text = script.guns[0].mel[drop.value].GetComponent<GunAddInfo>().desc;
            }
        }
        if (isPegasus)
        {
            if (isPrimary)
            {
                panelText.text = script.guns[1].pri[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isSeconadry)
            {
                panelText.text = script.guns[1].sec[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isMelee)
            {
                panelText.text = script.guns[1].mel[drop.value].GetComponent<GunAddInfo>().desc;
            }
        }
        if (isEarthPony)
        {
            if (isPrimary)
            {
                panelText.text = script.guns[2].pri[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isSeconadry)
            {
                panelText.text = script.guns[2].sec[drop.value].GetComponent<GunAddInfo>().desc;
            }
            if (isMelee)
            {
                panelText.text = script.guns[2].mel[drop.value].GetComponent<GunAddInfo>().desc;
            }
        }
        //Debug.Log(drop.options[drop.value].text);
    }
}
