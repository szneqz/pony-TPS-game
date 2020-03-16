using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunContrDouble : MonoBehaviour {

    private Transform root;

    public Rigidbody projectile;
    public float weaponSize = 3.0f;
    public float frequency = 1.0f;
    public float Stray = 0.0f;
    public float wepDmg = 0.0f;
    public float noDmgChangTime = 0.0f;
    public int bulletNr = 1;
    public int bulletForce = 100;
    public bool ifDouble = true;
    public bool ifOnRight = true;
    public bool addGravity = false;

    private ContrMovem keysScript;
    private Player2 playerScript;
    private GunInfo infoScript;

    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;
    private float weaponWide;
    private int leftright = -1;
    public float gunheight = 0.0f;
    private Transform mainTransform;
    private Transform parentTransform;

    private GameObject gunShot;  //obiekt wystrzalu
    private Vector3 GSscale;
    private Vector3 GSlocation;
    private float GSlenght;

    private AudioSource source;

    float randomNumberX;
    float randomNumberY;

    void Start ()
    {
        root = transform.parent.Find("Armature/MasterCtrl/hipsCtrl/root");
        infoScript = transform.GetComponent<GunInfo>();
        keysScript = transform.parent.GetComponent<ContrMovem>();
        playerScript = transform.parent.GetComponent<Player2>();
        mainTransform = transform;
        parentTransform = transform.parent.transform;

        gunShot = transform.Find("shotfire").gameObject;
        gunShot.SetActive(false);
        GSscale = gunShot.transform.localScale;
        GSlocation = new Vector3(0.0f, gunShot.transform.localPosition.y, 0.0f);
        GSlenght = gunShot.transform.localPosition.z;
        weaponWide = Mathf.Abs(gunShot.transform.localPosition.x);

        if (ifOnRight)
            leftright = 1;
        else
            leftright = -1;

        source = GetComponent<AudioSource>();
    }
	

    public void SpawnBullet(float X, float Y, int side)
    {
        GameObject obj = PoolScript.SharedInstance.GetPooledObject(projectile.transform.tag);
        if (obj != null)
        {

            gunShot.SetActive(true);
            gunShot.transform.localScale = GSscale * Random.Range(0.8f, 1.2f);
            gunShot.transform.Rotate(0.0f, 0.0f, Random.Range(0, 359.0f));

            gunShot.transform.position = GSlocation + mainTransform.position + mainTransform.right * weaponWide * side + mainTransform.forward * GSlenght;

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
            clone.transform.position = mainTransform.position + mainTransform.forward * weaponSize + mainTransform.right * weaponWide * side + gunheight * mainTransform.up;
            //clone.transform.position = mainTransform.position - mainTransform.forward * weaponSize + mainTransform.right * weaponWide * leftright + gunheight * mainTransform.up;
            clone.transform.rotation = mainTransform.rotation;
            //randomNumberX = playerScript.randomX;
            //randomNumberY = playerScript.randomY;
            clone.transform.Rotate(X, Y, 0.0f);
            clone.AddForce(clone.transform.forward * bulletForce);
            obj = null;
        }
        }

	void Update ()
    {

        if (!playerScript.Dead)
        {

            if (keysScript.Fire1)
            {
                if (time > frequency && infoScript.canShoot)
                {
                    if (source)
                    {
                        source.volume = Random.Range(0.9f, 1.0f) * 0.9f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                        source.pitch = Random.Range(0.95f, 1.05f) * 0.9f;
                        source.PlayOneShot(source.clip);
                    }

                    if (ifDouble)
                    {
                        if (leftright == -1)
                            leftright = 1;
                        else
                            leftright = -1;
                    }
                    infoScript.ShootFunc(); //wystrzelenie pocisku
                    for (int i = 0; i < bulletNr; i++)
                    {
                        if (playerScript.isLocalPlayer)
                        {   //jezeli sobie tutaj strzelam to obliczam rozrzut i rzucam go innym graczom
                            randomNumberX = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                            randomNumberY = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                            playerScript.CmdUpdateRandoms(randomNumberX, randomNumberY, leftright);
                        }
                        time = 0.0f;
                    }
                }
            }
            if (time < frequency)
                time += Time.deltaTime;

            if ((time > 0.05f || time >= frequency) && gunShot.activeInHierarchy) //czas trwania gunshotu
                gunShot.SetActive(false);

            velocity = Vector3.Distance(prevPos, parentTransform.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
            if (prevPos == parentTransform.position)
                velocity = 0.0f;
            prevPos = parentTransform.position;
        }
        else
                gunShot.SetActive(false);
    }

    private void LateUpdate()   //w LateUpdacie nic nie laguje, bo nie walczy o pozycję
    {
        mainTransform.position = root.position;
        mainTransform.rotation = root.rotation * Quaternion.Euler(180.0f, 90.0f, 90.0f);

        if (!playerScript.Dead)
        {
            float fakeDify = keysScript.shootRot.y - mainTransform.rotation.eulerAngles.y;

            if (fakeDify > 180.0f)   //obrot broni wzgledem kamery y
                fakeDify -= 360.0f;
            if (fakeDify < -180.0f)
                fakeDify += 360.0f;
            if (fakeDify > 160.0f || fakeDify < -160.0f)
                fakeDify = 0.0f;
            fakeDify = Mathf.Clamp(fakeDify, -15.0f, 15.0f);

            float fakeDifx = keysScript.shootRot.x - mainTransform.rotation.eulerAngles.x;
            if (fakeDifx > 180.0f)   //obrot broni wzgledem kamery x
                fakeDifx -= 360.0f;
            if (fakeDifx < -180.0f)
                fakeDifx += 360.0f;
            fakeDifx = Mathf.Clamp(fakeDifx, -25.0f, 30.0f);

            mainTransform.rotation = mainTransform.rotation * Quaternion.Euler(fakeDifx, fakeDify, 0.0f);
            if (keysScript.shootRot.z > 170.0f)  //obrot broni przy przejsciu kamery przez prog 90 stopni tez trzeba zmienic, bo sie bugowalo jak jasna cholera
                mainTransform.rotation = mainTransform.rotation * Quaternion.Euler(0.0f, -2 * fakeDify, 0.0f);
        }
        }
}
