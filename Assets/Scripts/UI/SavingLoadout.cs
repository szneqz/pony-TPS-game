using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingLoadout : MonoBehaviour {

    public WepList script;
    public Dropdown UnPrim;
    public Dropdown UnSec;
    public Dropdown UnMel;
    public Dropdown PePrim;
    public Dropdown PeSec;
    public Dropdown PeMel;
    public Dropdown EPPrim;
    public Dropdown EPSec;
    public Dropdown EPMel;

    private void Start()
    {
        if (script == null)
        {
            script = GameObject.FindGameObjectWithTag("NDGO").GetComponent<WepList>();
        }

        onUndo();
    }

    public void onSave()
    {
        StaticInfo.datScript.lo.Unicorn[0] = script.guns[0].pri[UnPrim.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.Unicorn[1] = script.guns[0].sec[UnSec.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.Unicorn[2] = script.guns[0].mel[UnMel.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.Pegasus[0] = script.guns[1].pri[PePrim.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.Pegasus[1] = script.guns[1].sec[PeSec.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.Pegasus[2] = script.guns[1].mel[PeMel.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.EarthPony[0] = script.guns[2].pri[EPPrim.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.EarthPony[1] = script.guns[2].sec[EPSec.value].GetComponent<GunAddInfo>().weaponNr;
        StaticInfo.datScript.lo.EarthPony[2] = script.guns[2].mel[EPMel.value].GetComponent<GunAddInfo>().weaponNr;
        SaveLoad.Save3();
    }

    public void onUndo()
    {
        SaveLoad.Load3();
        int i = 0;

        foreach(GameObject a in script.guns[0].pri)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Unicorn[0])
                UnPrim.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[0].sec)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Unicorn[1])
                UnSec.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[0].mel)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Unicorn[2])
                UnMel.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[1].pri)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Pegasus[0])
                PePrim.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[1].sec)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Pegasus[1])
                PeSec.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[1].mel)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.Pegasus[2])
                PeMel.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[2].pri)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.EarthPony[0])
                EPPrim.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[2].sec)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.EarthPony[1])
                EPSec.value = i;
            i++;
        }

        i = 0;

        foreach (GameObject a in script.guns[2].mel)
        {
            if (a.GetComponent<GunAddInfo>().weaponNr == StaticInfo.datScript.lo.EarthPony[2])
                EPMel.value = i;
            i++;
        }

        i = 0;
    }
	
}
