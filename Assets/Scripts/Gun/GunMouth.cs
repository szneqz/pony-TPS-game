using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunMouth : MonoBehaviour {

    private Transform head;
    private Transform parentObj;
    private Transform mainObj;
    private GunInfo infoScript;
    private ContrMovem keysScript;
    private Player2 playerScript;
    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;

    public float weaponSize = 3.0f;
    public float frequency = 1.0f;
    public float Stray = 0.0f;
    public float wepDmg = 0.0f;
    public float noDmgChangTime = 0.0f;

    public float gunForward = 0.3f;     //odleglosc broni na wprost paszczy
    public float gunUp = 0.08f;         //odleglosc broni w gore od paszczy
    public float gunheight = 0.0f;

    public int bulletNr = 1;
    public int bulletForce = 100;
    public bool addGravity = false;

    private GameObject gunShot;  //obiekt wystrzalu
    private Vector3 GSscale;

    private AudioSource source;

    private float randomNumberX;
    private float randomNumberY;

    void Start ()
    {
        parentObj = transform.parent;
        infoScript = transform.GetComponent<GunInfo>();
        keysScript = parentObj.GetComponent<ContrMovem>();
        playerScript = parentObj.GetComponent<Player2>();
        mainObj = this.transform;
        head = parentObj.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2/neck/head").transform;

        gunShot = transform.Find("shotfire").gameObject;
        gunShot.SetActive(false);
        GSscale = gunShot.transform.localScale;
        source = GetComponent<AudioSource>();
    }

    public void SpawnBullet(float X, float Y, int side)
    {
        GameObject obj = PoolScript.SharedInstance.GetPooledObject("Bullet");
        if (obj != null)
        {
            gunShot.SetActive(true);
            gunShot.transform.localScale = GSscale * Random.Range(0.8f, 1.2f);
            gunShot.transform.Rotate(0.0f, 0.0f, Random.Range(0, 359.0f));

            BulletPhysics bulPhysScript = obj.GetComponent<BulletPhysics>();
            bulPhysScript.player = parentObj;  //info o sojuszniku
            bulPhysScript.team = parentObj.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
            bulPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen
            bulPhysScript.noChangDmgTime = noDmgChangTime;  //po jakim czasie maja byc zmniejszone obrazenia
            obj.SetActive(true);
            Rigidbody clone = obj.GetComponent<Rigidbody>();
            clone.useGravity = addGravity;
            clone.transform.position = mainObj.position + mainObj.forward * weaponSize + gunheight * mainObj.up;
            clone.transform.rotation = mainObj.rotation;
            //randomNumberX = playerScript.randomX;
            //randomNumberY = playerScript.randomY;
            clone.transform.Rotate(X, Y, 0.0f);
            clone.AddForce(clone.transform.forward * bulletForce);
        }
    }

    void Update()
    {
       // mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
       // mainObj.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z + 90.0f);
        if (!playerScript.Dead)
        {
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
                            randomNumberX = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                            randomNumberY = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                            playerScript.CmdUpdateRandoms(randomNumberX, randomNumberY, 1);
                        }
                        time = 0.0f;
                    }
                }
            }
            if (time < frequency)
                time += Time.deltaTime;

            if ((time > 0.05f || time >= frequency) && gunShot.activeInHierarchy) //czas trwania gunshotu
                gunShot.SetActive(false);

            velocity = Vector3.Distance(prevPos, parentObj.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
            if (prevPos == parentObj.position)
                velocity = 0.0f;
            prevPos = parentObj.position;
        }
        else
            gunShot.SetActive(false);
    }

    private void LateUpdate()
    {
        mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
        mainObj.rotation = Quaternion.Euler(head.rotation.eulerAngles.x, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z + 90.0f);
    }
}

