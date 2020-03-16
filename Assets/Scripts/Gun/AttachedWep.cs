using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeapoonPool
{
    public int maxAmmo;
    public int actualAmmo;
    public int ammoInMag;
    public int gunNumber = 0;
}

public class AttachedWep : MonoBehaviour {

    public List<WeapoonPool> weaponsInfo;

	
    public WeapoonPool CheckList(int maxAmmo, int actualAmmo, int ammoInMag, int gunNumber, bool update)
    {
        if (gunNumber <= 0 || gunNumber > 9)    //jezeli spoza zakresu to opuszczam
            return null;

        for(int i = 0; i < weaponsInfo.Count; i++)
        {   //wyszukuje czy chce dodac nowa bron czy tylko przelaczyc na istniejaca
            if (weaponsInfo[i].gunNumber == gunNumber && update)
            {   //aktualizacja amunicji z zewnatrz
                weaponsInfo[i].actualAmmo = actualAmmo;
                weaponsInfo[i].ammoInMag = ammoInMag;
                return weaponsInfo[i];
            }
            if (weaponsInfo[i].gunNumber == gunNumber && !update)
            {   //pobieranie informacji z wewnątrz
                return weaponsInfo[i];
            }
        }

        //jezeli nie ma powyzszego elementu listy to dodaje sobie nowy
        WeapoonPool newClass = new WeapoonPool();
        newClass.actualAmmo = actualAmmo;
        newClass.ammoInMag = ammoInMag;
        newClass.gunNumber = gunNumber;
        newClass.maxAmmo = maxAmmo;
        weaponsInfo.Add(newClass);
        return newClass;
    }

    public void Died(bool dead)
    {
        if (dead)
        {
            weaponsInfo.Clear();
        }
    }

    public void AmmoBox(float multipler)
    {
        for (int i = 0; i < weaponsInfo.Count; i++)
        {
            if(weaponsInfo[i].actualAmmo < weaponsInfo[i].maxAmmo)
            weaponsInfo[i].actualAmmo = (int)Mathf.Clamp(weaponsInfo[i].maxAmmo * multipler + weaponsInfo[i].actualAmmo, 0.0f, weaponsInfo[i].maxAmmo);
        }
    }

    public bool CanTake()
    {
        for (int i = 0; i < weaponsInfo.Count; i++)
        {
            if(weaponsInfo[i].actualAmmo < weaponsInfo[i].maxAmmo)
            return true;
        }
        return false;
    }
}

