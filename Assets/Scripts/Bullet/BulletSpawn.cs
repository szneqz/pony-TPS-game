using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletSpawn : MonoBehaviour {

    public Rigidbody projectile;
    public float weaponSize = 3.0f;
    public float frequency = 1.0f;
    public float Stray = 0.0f;
    public float wepDmg = 0.0f;
    public float noDmgChangTime = 0.0f;
    public int bulletNr = 1;
    public int bulletForce = 100;
    public bool addGravity = false;
    public bool ifSniper = false;

    private ContrMovem keysScript;
    private GunController gunScript;
    private GunInfo infoScript;
    private Player2 playerScript;

    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;
    private float ifStray = 1.0f;
    public bool SniperShoot = true; //zmienna ogolna dotyczy momentu po strzale, w którym jest oddalenei widoku
    private GameObject gunShot;  //obiekt wystrzalu
    private Vector3 GSscale;

    private AudioSource source;

    float randomNumberX;
    float randomNumberY;

    void Start()
    {
        keysScript = transform.parent.GetComponent<ContrMovem>();
        gunScript = transform.GetComponent<GunController>();
        infoScript = transform.GetComponent<GunInfo>();
        playerScript = transform.parent.GetComponent<Player2>();
        gunShot = transform.Find("shotfire").gameObject;
        gunShot.SetActive(false);
        GSscale = gunShot.transform.localScale;
        source = GetComponent<AudioSource>();
    }

    //[Command]
    //void CmdSyncXY(float X, float Y)
    //{
    //    randomNumberX = X;
    //    randomNumberY = Y;
    //}

    public void SpawnBullet(float X, float Y, int side)
    {
        GameObject obj = PoolScript.SharedInstance.GetPooledObject(projectile.transform.tag);
        if (obj != null)
        {
            gunShot.SetActive(true);
            gunShot.transform.localScale = GSscale * Random.Range(0.8f, 1.2f);
            gunShot.transform.Rotate(0.0f, 0.0f, Random.Range(0, 359.0f));

            BulletPhysics bulPhysScript = obj.GetComponent<BulletPhysics>();
            if (bulPhysScript)
            {
                bulPhysScript.player = transform.parent;  //info o sojuszniku
                bulPhysScript.team = transform.parent.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
                bulPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen
                bulPhysScript.noChangDmgTime = noDmgChangTime;  //po jakim czasie maja byc zmniejszone obrazenia
            }
            MisslePhysics misPhysScript = obj.GetComponent<MisslePhysics>();
            if (misPhysScript)
            {
                misPhysScript.player = transform.parent;  //info o sojuszniku
                misPhysScript.team = transform.parent.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
                misPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen
            }
            obj.SetActive(true);
            Rigidbody clone = obj.GetComponent<Rigidbody>();
            clone.useGravity = addGravity;
            clone.transform.position = transform.position + transform.forward * weaponSize;
            clone.transform.rotation = transform.rotation;
            randomNumberX = X;
            randomNumberY = Y;
            clone.transform.Rotate(randomNumberX, randomNumberY, 0.0f);
            clone.AddForce(clone.transform.forward * bulletForce);
            obj = null;
        }
    }

    void Update()
    {
        if (!playerScript.Dead)
        {
            if (ifSniper && keysScript.Fire2 && velocity < 0.05f && (gunScript.ifScope || gunScript.ifScopeBot) && SniperShoot)
            {
                ifStray = 0.0f;
            }
            else
            {
                ifStray = 1.0f;
            }
            if (keysScript.Fire1)
            {
                if (time > frequency && infoScript.canShoot)
                {
                    infoScript.ShootFunc(); //wystrzelenie pocisku

                    if (source)
                    {
                        source.volume = Random.Range(0.9f, 1.0f) * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                        source.pitch = Random.Range(0.9f, 1.0f);
                        source.Play();
                    }

                    for (int i = 0; i < bulletNr; i++)
                    {
                        if (playerScript.isLocalPlayer)
                        {   //jezeli sobie tutaj strzelam to obliczam rozrzut i rzucam go innym graczom
                            randomNumberX = Random.Range(-Stray * ifStray - (velocity / 50.0f), Stray * ifStray + (velocity * Stray * ifStray / 50.0f));
                            randomNumberY = Random.Range(-Stray * ifStray - (velocity / 50.0f), Stray * ifStray + (velocity * Stray * ifStray / 50.0f));
                            playerScript.CmdUpdateRandoms(randomNumberX, randomNumberY, 1);
                        }
                        time = 0.0f;
                    }
                }
            }
            if (time < frequency)
            {
                time += Time.deltaTime;
            }
            if ((time > 0.05f || time >= frequency) && gunShot.activeInHierarchy) //czas trwania gunshotu
                gunShot.SetActive(false);

            if (time < (frequency - 0.1f))  //po strzale na chwile oddalany jest widok
                SniperShoot = false;
            else
                SniperShoot = true;
            if (!infoScript.canShoot)   //przy przeladowaniu postac nie moze miec przyblizonego widoku
                SniperShoot = false;

            velocity = Vector3.Distance(prevPos, transform.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
            if (prevPos == transform.position)
                velocity = 0.0f;
            prevPos = transform.position;
        }
        else
            gunShot.SetActive(false);
    }
}
