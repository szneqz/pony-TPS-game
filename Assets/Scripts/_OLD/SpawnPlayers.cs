using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    private RespawnPoints respScript;
    private PoolScript poolScript;
    private int amount = 24;


    void Start()
    {
        poolScript = GetComponent<PoolScript>();
        respScript = GetComponent<RespawnPoints>();

        amount = StaticInfo.datScript.prop.playerAmount; //info z globalnego scripta

        for (int i = 0; i < amount; i++)
            {
                GameObject obj = PoolScript.SharedInstance.GetPooledObject("Player");
            obj.transform.position = respScript.respawns[(int)Random.Range(0.0f, respScript.respawns.Count - 0.01f)].transform.position;
                obj.SetActive(true);
            AddInfoPlayer addInfoScript = obj.transform.GetComponent<AddInfoPlayer>();
            WepChange wepScript = obj.transform.GetComponent<WepChange>();
            wepScript.weapons.Clear();
            if (i != 0)
            {
                obj.GetComponent<ContrMovem>().ifBot = true;
                addInfoScript.gender = (Random.value > 0.5f);
                addInfoScript.race = Random.Range(0, 3);

                if (addInfoScript.race == 0)    //randomowe bronie dla botow - korzystaja one z innej grupy broni niz gracz
                {
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[2].pri[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[2].pri.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[2].sec[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[2].sec.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[2].mel[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[2].mel.Count)]);
                }
                if (addInfoScript.race == 1)
                {
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[0].pri[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[0].pri.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[0].sec[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[0].sec.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[0].mel[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[0].mel.Count)]);
                }
                if (addInfoScript.race == 2)
                {
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[1].pri[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[1].pri.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[1].sec[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[1].sec.Count)]);
                    wepScript.weapons.Add(StaticInfo.datScript.GetComponent<WepList>().guns[1].mel[Random.Range(0, StaticInfo.datScript.GetComponent<WepList>().guns[1].mel.Count)]);
                }
            }
            else
            {
                obj.name = "pony3_lowPoly";
                addInfoScript.gender = StaticInfo.datScript.prop.ifMale;
                addInfoScript.race = StaticInfo.datScript.prop.playerClass;  //info z globalnego scripta

                if (addInfoScript.race == 0)    //nadawanie glownej postaci broni
                {
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.EarthPony[0]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.EarthPony[1]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.EarthPony[2]]);
                }
                if (addInfoScript.race == 1)
                {
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Unicorn[0]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Unicorn[1]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Unicorn[2]]);
                }
                if (addInfoScript.race == 2)
                {
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Pegasus[0]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Pegasus[1]]);
                    wepScript.weapons.Add(poolScript.weapons[StaticInfo.datScript.lo.Pegasus[2]]);
                }
            }
            }
    }

}