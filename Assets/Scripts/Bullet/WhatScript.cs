using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatScript : MonoBehaviour {

    BulletSpawn scr;
    GunContrDouble scr2;
    GunMouth scr3;
    GrenadeMouthThrow scr4;
    GrenadeThrow scr5;
    int nr = 0;
    public GunInfo gunInfo;
    private WepChange wepChange;

    void Start ()
    {
        wepChange = transform.parent.GetComponent<WepChange>();
        gunInfo = GetComponent<GunInfo>();
        wepChange.whatScr = this;

        if (scr = GetComponent<BulletSpawn>())
        {
            nr = 1;
        }
        else if (scr2 = GetComponent<GunContrDouble>())
        {
            nr = 2;
        }
        else if (scr3 = GetComponent<GunMouth>())
        {
            nr = 3;
        }
        else if (scr4 = GetComponent<GrenadeMouthThrow>())
        {
            nr = 4;
        }
        else if (scr5 = GetComponent<GrenadeThrow>())
        {
            nr = 5;
        }
    }
	
    public void sendThatInfo(float X, float Y, int side)
    {
        wepChange.upAmmo(gunInfo.actualAmmo, gunInfo.ammoInMag);

        if (nr == 1)
        {
            scr.SpawnBullet(X, Y, side);
        }
        else if (nr == 2)
        {
            scr2.SpawnBullet(X, Y, side);
        }
        else if (nr == 3)
        {
            scr3.SpawnBullet(X, Y, side);
        }
        else if (nr == 4)
        {
            scr4.SpawnBullet(X, Y, side);
        }
        else if (nr == 5)
        {
            scr5.SpawnBullet(X, Y, side);
        }
    }
}
