using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WepChange : NetworkBehaviour {

    public List<GameObject> weapons;

    private ContrMovem keysScript;  //skrypt do wyciagania broni
    private Player2 playerScript;
    private AddInfoPlayer addInfoScript;
    private PoolScript poolScript;
    private MP_HighAuthority hAuthScript;

    public KeyCode Number = (KeyCode)48;
    private GameObject usingWep = null;
    private bool ifDestroy = false;
    private GameObject weapon = null;

    public bool ifDestroyWep = false;

    public WhatScript whatScr;

    [SyncVar]
    private int nr = 0;

    void Start()
    {
        keysScript = transform.GetComponent<ContrMovem>();
        playerScript = transform.GetComponent<Player2>();
        addInfoScript = transform.GetComponent<AddInfoPlayer>();
        poolScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<PoolScript>();
        hAuthScript = transform.GetComponent<MP_HighAuthority>();
    }

    [ClientRpc]
    private void RpcUpAmmo(int actAmmo, int ammoInMag)
    {
        //if(!isLocalPlayer)    //AKUTALIZUJEMY TEZ PLAYEROWI, ZEBY NIE CHEATOWAL!
        //{
            whatScr.gunInfo.actualAmmo = actAmmo;
            whatScr.gunInfo.ammoInMag = ammoInMag;
        //}
    }

    [Command]
    private void CmdUpAmmo(int actAmmo, int ammoInMag)
    {
        RpcUpAmmo(actAmmo, ammoInMag);
    }

    public void upAmmo(int actAmmo, int ammoInMag)
    {
        if(isLocalPlayer)
        {
            CmdUpAmmo(actAmmo, ammoInMag);
        }
    }

    public void upList()
    {
        weapons.Clear();
        if (addInfoScript.race == 0)    //nadawanie glownej postaci broni
        {
            weapons.Add(poolScript.weapons[hAuthScript.weapons[6]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[7]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[8]]);
        }
        if (addInfoScript.race == 1)
        {
            weapons.Add(poolScript.weapons[hAuthScript.weapons[0]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[1]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[2]]);
        }
        if (addInfoScript.race == 2)
        {
            weapons.Add(poolScript.weapons[hAuthScript.weapons[3]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[4]]);
            weapons.Add(poolScript.weapons[hAuthScript.weapons[5]]);
        }
    }

    [Command]
    void CmdChangeWep(int a)
    {
        nr = a;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (playerScript.Dead)
            {
                nr = 0;
            }
            if (keysScript.num != 10)
            {
                nr = keysScript.num;
            }
            if (keysScript.scroll > 0)
            {
                nr = nr + 1;
            }
            if (keysScript.scroll < 0)
            {
                nr = nr - 1;
            }
            if (nr > weapons.Count)
                nr = 0;
            if (nr < 0)
                nr = weapons.Count;

            CmdChangeWep(nr);    //czesc multpilayera - pelne synchro z wyborem broni przez graczy
        }

            if (nr != ((int)Number - 48) && nr != 10 && !playerScript.Dead)
            {
                if (nr == 0)   //brak broni
                {
                    ifDestroy = true;
                    usingWep = null;
                    Number = KeyCode.Alpha0;
                }
                for (int i = 0; i < weapons.Count; i++)
                    if (nr == i + 1)
                    {
                        ifDestroy = true;
                        usingWep = weapons[i];
                        Number = (KeyCode)49 + i;
                    }


                if (ifDestroy && weapon != null)
                {
                    DestroyImmediate(weapon, true);
                }
                if (usingWep != null && ifDestroy == true)
                {
                    weapon = Instantiate(usingWep);
                    weapon.transform.parent = transform;
                    if (weapon.GetComponent<GunInfo>() != null)
                        weapon.GetComponent<GunInfo>().gunNumber = (int)Number - 48;
                    usingWep = null;
                    ifDestroy = false;
                }
            }
            if (ifDestroyWep)
            {
                DestroyImmediate(weapon, true);
                Number = KeyCode.Alpha0;
                ifDestroyWep = false;
            }
	}

}
