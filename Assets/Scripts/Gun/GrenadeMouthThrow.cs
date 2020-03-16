using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeMouthThrow : MonoBehaviour {

    private Transform head;
    public float gunForward = 0.3f;     //odleglosc broni na wprost paszczy
    public float gunUp = 0.08f;         //odleglosc broni w gore od paszczy

    public Rigidbody projectile;
    public float weaponSize = 3.0f;
    public float frequency = 1.0f;
    public float Stray = 0.0f;
    public float wepDmg = 0.0f;
    public int bulletNr = 1;
    public int bulletForce = 100;
    public bool addGravity = false;

    private ContrMovem keysScript;
    private GunInfo infoScript;
    private Player2 playerScript;
    private CrosshairSpawn crossScript;

    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;
    private Transform mainObj;

    private bool isCharging = false;
    private float chargeMultipler = 1.0f;
    private float chargeTime = 0.0f;

    private AudioSource source;
    public AudioClip grenadePin;
    private bool pinbool = false;
    public AudioClip grenadeThrow;

    float randomNumberX;
    float randomNumberY;
    float rotY;

    void Start()
    {
        keysScript = transform.parent.GetComponent<ContrMovem>();
        infoScript = transform.GetComponent<GunInfo>();
        playerScript = transform.parent.GetComponent<Player2>();
        crossScript = transform.GetComponent<CrosshairSpawn>();
        mainObj = transform;
        head = transform.parent.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2/neck/head").transform;
        source = GetComponent<AudioSource>();
        source.volume = source.volume * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
    }

    public void SpawnBullet(float X, float Y, int side)
    {
        GameObject obj = PoolScript.SharedInstance.GetPooledObject(projectile.transform.tag);
        if (obj != null)
        {
            GrenadePhysics grenPhysScript = obj.GetComponent<GrenadePhysics>();
            grenPhysScript.player = mainObj.parent;  //info o sojuszniku
            grenPhysScript.team = mainObj.parent.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
            grenPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen

            obj.SetActive(true);
            Rigidbody clone = obj.GetComponent<Rigidbody>();
            clone.useGravity = addGravity;
            clone.transform.position = mainObj.position + Quaternion.Euler(new Vector3(keysScript.camRot.x, rotY, keysScript.camRot.z)) * Vector3.forward * weaponSize;
            clone.transform.rotation = Quaternion.Euler(new Vector3(keysScript.camRot.x, rotY, keysScript.camRot.z));
            //randomNumberX = playerScript.randomX;
            //randomNumberY = playerScript.randomY;
            clone.transform.Rotate(X, Y, 0.0f);
            clone.AddForce(clone.transform.forward * bulletForce * chargeMultipler);
            chargeMultipler = 1.0f;
            isCharging = false;
            obj = null;

            if (source && grenadeThrow)
                source.PlayOneShot(grenadeThrow);
        }
    }

    void Update()
    {
        //mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
        //mainObj.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z + 90.0f);

        if (!playerScript.Dead)
        {
            float fakeDify = keysScript.camRot.y - mainObj.parent.rotation.eulerAngles.y;

            if (fakeDify > 180.0f)   //obrot broni wzgledem kamery y
                fakeDify -= 360.0f;
            if (fakeDify < -180.0f)
                fakeDify += 360.0f;
            fakeDify = Mathf.Clamp(fakeDify, -50.0f, 50.0f);
            rotY = mainObj.transform.parent.rotation.eulerAngles.y + fakeDify;

            crossScript.datForward = Quaternion.Euler(new Vector3(keysScript.camRot.x, rotY, keysScript.camRot.z)) * Vector3.forward;  //przeniesienie celownika na skrypt do celownika

            if (time > frequency && infoScript.canShoot && keysScript.Fire1)
            {
                isCharging = true;
                chargeMultipler += Time.deltaTime;
                chargeMultipler = Mathf.Clamp(chargeMultipler, 1.0f, 1.3f);
                if (pinbool && source && grenadePin)
                    source.PlayOneShot(grenadePin);
                pinbool = false;
            }
            else
            {
                pinbool = true;
            }

            if (keysScript.Fire1 && infoScript.canShoot)
            {
                chargeTime = 0.2f;
            }
            else
            {
                chargeTime -= Time.deltaTime;
            }

            if (isCharging && infoScript.canShoot && !keysScript.Fire1 && chargeTime <= 0.0f)
            {
                infoScript.ShootFunc(); //wystrzelenie pocisku
                for (int i = 0; i < bulletNr; i++)
                {
                    if (playerScript.isLocalPlayer)
                    {   //jezeli sobie tutaj strzelam to obliczam rozrzut i rzucam go innym graczom
                        randomNumberX = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                        randomNumberY = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                        playerScript.CmdUpdateRandoms(randomNumberX, randomNumberY, 1);
                    }
                    time = 0.0f;
                }
            }


            if (time < frequency)
            {
                time += Time.deltaTime;
            }

            velocity = Vector3.Distance(prevPos, mainObj.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
            if (prevPos == mainObj.position)
                velocity = 0.0f;
            prevPos = mainObj.position;

            if(keysScript.Fire1 && infoScript.canShoot)
            {
                if(fakeDify > 0.0f)
                {
                    keysScript.isChangedShootRot = true;
                    keysScript.shootRot = new Vector3(360.0f-keysScript.camRot.x, rotY - 50.0f, keysScript.camRot.z);
                }
                else
                {
                    keysScript.isChangedShootRot = true;
                    keysScript.shootRot = new Vector3(360.0f-keysScript.camRot.x, rotY + 50.0f, keysScript.camRot.z);
                }
            }
        }
    }


    private void LateUpdate()
    {
        mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
        mainObj.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z + 90.0f);
    }
}
