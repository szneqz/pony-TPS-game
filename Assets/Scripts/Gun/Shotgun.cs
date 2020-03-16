using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{

    public Rigidbody projectile;
    public float weaponSize = 3.0f;
    public float frequency = 1.0f;
    public float Stray = 0.0f;
    public float wepDmg = 0.0f;
    public float noDmgChangTime = 0.0f;
    public int bulletNumber = 5;

    private ContrMovem keysScript;

    private float time = 0.0f;
    private float velocity = 0.0f;
    private Vector3 prevPos;
    private GameObject gunShot;  //obiekt wystrzalu
    private Vector3 GSscale;

    void Start()
    {
        keysScript = transform.parent.GetComponent<ContrMovem>();
        gunShot = transform.Find("shotfire").gameObject;
        gunShot.SetActive(false);
        GSscale = gunShot.transform.localScale;
    }

    void Update()
    {
        if (keysScript.Fire1)
        {
            if (time > frequency)
            {
                for (int i = 0; i < bulletNumber; i++)
                {
                    GameObject obj = PoolScript.SharedInstance.GetPooledObject("Bullet");
                    if (obj != null)
                    {
                        gunShot.SetActive(true);
                        gunShot.transform.localScale = GSscale * Random.Range(0.8f, 1.2f);
                        gunShot.transform.Rotate(0.0f, 0.0f, Random.Range(0, 359.0f));

                        BulletPhysics bulPhysScript = obj.GetComponent<BulletPhysics>();
                        bulPhysScript.player = transform.parent;  //info o sojuszniku
                        bulPhysScript.team = transform.parent.GetComponent<AddInfoPlayer>().team;  //nadawanie pociskowi druzyny (0 - grey, 1 - spec, 2 - red, 3 - blue)
                        bulPhysScript.damage = wepDmg;    //nadanie pociskowi obrazen
                        bulPhysScript.noChangDmgTime = noDmgChangTime;  //po jakim czasie maja byc zmniejszone obrazenia
                        obj.SetActive(true);
                        Rigidbody clone = obj.GetComponent<Rigidbody>();
                        clone.transform.position = transform.position + transform.forward * weaponSize;
                        clone.transform.rotation = transform.rotation;
                        float randomNumberX = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                        float randomNumberY = Random.Range(-Stray - (velocity / 50.0f), Stray + (velocity * Stray / 50.0f));
                        clone.transform.Rotate(randomNumberX, randomNumberY, 0.0f);
                        clone.AddForce(clone.transform.forward * 100.0f);
                        time = 0.0f;
                        obj = null;
                    }
                }
            }
        }
        if (time < frequency)
        {
            time += Time.deltaTime;
        }
        if ((time > 0.05f || time >= frequency) && gunShot.activeInHierarchy) //czas trwania gunshotu
            gunShot.SetActive(false);

        velocity = Vector3.Distance(prevPos, transform.position) / Time.deltaTime;    //zaleznie od predkosci ruchu postaci zwiekszy sie rozrzut
        if (prevPos == transform.position)
            velocity = 0.0f;
        prevPos = transform.position;
    }
}

