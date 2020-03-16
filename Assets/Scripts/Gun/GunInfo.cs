using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour {

    public int maxAmmo;
    public int ammoPerMag;
    public float reloadTime;
    public float noShootTimeAfterReload = 0.5f;
    public int ammoPerOneReload;
    public int gunNumber;

    public int actualAmmo;
    public int ammoInMag;

    public bool canShoot = false;   //sprawdzane zostanie to czy mozna strzelac czy nie
    private bool reloading = false;  //czy przeladowuje teraz
    public bool animReload = false;    //czy animowac przeladowanie
    public bool ifDisMag = false;   //czy wylaczac magazynek przy braku ammo

    private MeshRenderer mag;

    private WeapoonPool pool;
    private AttachedWep wepScript;
    private ContrMovem movmScript;
    private Player2 playerScript;
    private ReloadAnim reloadScript;

    private float timeSec = 0.0f;
    private float noShootTime = 0.0f;
    private float timeAfterRel = 0.0f;

    public AudioSource source;
    public AudioClip reloadSound;
    private bool soundEND;

    private void Start()
    {
        playerScript = transform.parent.GetComponent<Player2>();
        movmScript = transform.parent.GetComponent<ContrMovem>();
        wepScript = transform.parent.GetComponent<AttachedWep>();
        if (animReload)
            reloadScript = transform.GetComponent<ReloadAnim>();
        pool = wepScript.CheckList(maxAmmo, actualAmmo, ammoPerMag, gunNumber, false);
        actualAmmo = pool.actualAmmo;
        ammoInMag = pool.ammoInMag;
        if(ifDisMag)
        {
            mag = transform.Find("Magazine").GetComponent<MeshRenderer>();
        }

        if(!source)
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerScript.Dead)
        {
            noShootTime = 1.0f;
            actualAmmo = maxAmmo;
            ammoInMag = ammoPerMag;
        }
        else
        {
            noShootTime -= Time.deltaTime;
            noShootTime = Mathf.Clamp(noShootTime, 0.0f, 1.0f);
        }

        pool = wepScript.CheckList(maxAmmo, actualAmmo, ammoInMag, gunNumber, true);
        actualAmmo = pool.actualAmmo;
        ammoInMag = pool.ammoInMag;

        if (movmScript.reload)
            reloading = true;

        if (((reloading && ammoInMag < ammoPerMag) || ammoInMag <= 0) && actualAmmo > 0 && noShootTime == 0.0f)
        {
            if (animReload)
                reloadScript.Reloading(reloadTime);
            timeAfterRel = noShootTimeAfterReload;
            reloading = true;
            canShoot = false;
            if (ammoPerOneReload == ammoPerMag)
            {
                actualAmmo += ammoInMag;    //oprozniam magazynek
                ammoInMag = 0;
            }
            timeSec += Time.deltaTime;  //odliczam czas przeladowania

            if (source && reloadSound && (reloadTime - timeSec) <= reloadSound.length && !soundEND)
            {
                source.pitch = Random.Range(0.95f, 1.05f);
                source.volume = Random.Range(0.9f, 1.0f) * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                source.PlayOneShot(reloadSound);                                    //odgrywa przeladowanie tak by odegralo sie idealnie z wlozeniem magazynka
                soundEND = true;
            }
            if (!(source && reloadSound && (reloadTime - timeSec) <= reloadSound.length))
                soundEND = false;

            if (timeSec > reloadTime)    //jezeli sie przeladowalo...
            {
                timeSec = 0.0f; //resetuje czas
                actualAmmo -= ammoPerOneReload;   //...obnizam wartosc calego ammo o magazynek
                ammoInMag += ammoPerOneReload;     //dokladam ammo do magazynka
                if (actualAmmo < 0)          //jezeli ammo jest mniej niz pojemnosc magazynka
                {
                    ammoInMag += actualAmmo;    //ammo w magazynku zwiekszam o liczbe ujemna pozostalego ammo
                    actualAmmo = 0;             //oprozniam ammo do 0
                }
            }
        }
        else
        {
            canShoot = true;
            if(timeAfterRel > 0.0f)
            timeAfterRel -= Time.deltaTime;
            soundEND = false;
        }

        if (ammoInMag == ammoPerMag || movmScript.Fire1)    //jezeli zaladuje sie lub strzeli sie podczas to przeladowanie wstrzymane
            reloading = false;


        if (ammoInMag <= 0 || noShootTime > 0.0f || timeAfterRel > 0.0f) //nie mozna strzelac na pustym magazynku i jednoczesnie pustym ammo oraz gdy jest sie martwym
            canShoot = false;

        if(ifDisMag)
        {
            if (actualAmmo + ammoInMag <= 0 && mag.enabled == true)
                mag.enabled = false;
            if (actualAmmo + ammoInMag > 0 && mag.enabled == false)
                mag.enabled = true;
        }

        //skrypt na odzyskiwanie ammo ze skrzynek jest w glownym skrypcie AttachedGun
    }

    public void ShootFunc()
    {
        ammoInMag -= 1;
    }
}
