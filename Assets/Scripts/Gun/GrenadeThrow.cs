using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{

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
    private Transform trans;

    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;

    private bool isCharging = false;
    private float chargeMultipler = 1.0f;

    private AudioSource source;
    public AudioClip grenadePin;
    private bool pinbool = false;
    public AudioClip grenadeThrow;

    private float randomNumberX;
    private float randomNumberY;

    void Start()
    {
        keysScript = transform.parent.GetComponent<ContrMovem>();
        infoScript = transform.GetComponent<GunInfo>();
        playerScript = transform.parent.GetComponent<Player2>();
        trans = transform;
        source = GetComponent<AudioSource>();
        source.volume = source.volume * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
    }

    public void SpawnBullet(float X, float Y, int side)
    {
        GameObject obj = PoolScript.SharedInstance.GetPooledObject(projectile.transform.tag);
        if (obj != null)
        {
            GrenadePhysics grenPhysScript = obj.GetComponent<GrenadePhysics>();
            grenPhysScript.player = trans.parent;  //info o sojuszniku
            grenPhysScript.team = trans.parent.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
            grenPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen

            obj.SetActive(true);
            Rigidbody clone = obj.GetComponent<Rigidbody>();
            clone.useGravity = addGravity;
            clone.transform.position = trans.position + trans.forward * weaponSize;
            clone.transform.rotation = trans.rotation;
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
        if (!playerScript.Dead)
        {
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

            if (isCharging && infoScript.canShoot && !keysScript.Fire1)
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

            velocity = Vector3.Distance(prevPos, trans.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
            if (prevPos == trans.position)
                velocity = 0.0f;
            prevPos = trans.position;
        }
    }

    void LateUpdate()
    {
        trans.position = (trans.position - trans.forward * (chargeMultipler - 1.0f));
    }
}
