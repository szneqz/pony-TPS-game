using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthSword : MonoBehaviour
{

    private Transform head;
    private Transform parentObj;
    private Transform mainObj;
    private ContrMovem contrScript;
    private Player2 playerScript;
    private Blade bladeScript;

    private GameObject blade;
    private bool ifAnim = false;
    private int turn = 1;

    private float animTime = 0.0f;
    public float maxAnimTime = 6.0f;
    private float datRot = 0.0f;
    private float waitTime = 0.0f;

    public float gunForward = 0.3f;     //odleglosc broni na wprost paszczy
    public float gunUp = 0.08f;         //odleglosc broni w gore od paszczy

    private Vector3 prevHeadRot = Vector3.zero; //poprzedni obrot glowy;

    private AudioSource source;
    public AudioClip swing;
    private bool swinged = false;

    void Start()
    {
        blade = transform.Find("bladeCollider").gameObject;
        blade.SetActive(false);

        bladeScript = blade.GetComponent<Blade>();

        parentObj = transform.parent;
        contrScript = parentObj.GetComponent<ContrMovem>();
        playerScript = parentObj.GetComponent<Player2>();
        mainObj = this.transform;
        head = parentObj.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2/neck/head").transform;

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
            //float animTime2 = Mathf.Clamp(animTime * 2, 0.0f, maxAnimTime);
            //datRot = turn * 90 + (-180.0f * turn) * animTime2 / maxAnimTime;     //tu jakos sprzezyc z animTime <---------------------------
            //mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
            //mainObj.rotation = head.rotation * Quaternion.Euler(datRot, 0.0f, 0.0f);
            if (!playerScript.Dead)
            {
            if (waitTime <= 0.0f)
            {
                if (contrScript.Fire1 && ifAnim == false)    //jezeli LPM, ale jednoczesnie nie uderza
                {
                    blade.SetActive(true);
                    ifAnim = true;
                    prevHeadRot = contrScript.shootRot;
                }
            }
            else
                waitTime -= Time.deltaTime;

            if (ifAnim == true)
                {
                //time = Mathf.Clamp()
                    contrScript.isChangedShootRot = true;
                    contrScript.shootRot = new Vector3(prevHeadRot.x, prevHeadRot.y + 90.0f * turn, prevHeadRot.z);
                    if (animTime < maxAnimTime)
                    {
                        animTime += Time.deltaTime;
                        if (swinged && source && swing && animTime >= (maxAnimTime - swing.length))
                        {
                            source.pitch = Random.Range(0.95f, 1.05f);
                            source.volume = Random.Range(0.9f, 1.0f) * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                            source.PlayOneShot(swing);
                            swinged = false;
                        }
                    }
                    if (animTime > maxAnimTime)
                    {
                        ifAnim = false;
                        animTime = 0.0f;
                        turn = -turn;
                        bladeScript.Destroy();
                        waitTime = 0.2f;
                    swinged = true;
                    }
            }
        }
    }

    private void LateUpdate()
    {
        float animTime2 = Mathf.Clamp(animTime * 2, 0.0f, maxAnimTime);
        datRot = turn * 90 + (-180.0f * turn) * animTime2 / maxAnimTime;     //tu jakos sprzezyc z animTime <---------------------------
        mainObj.position = head.position + head.forward * gunForward - head.right * gunUp;
        mainObj.rotation = head.rotation * Quaternion.Euler(datRot, 0.0f, 0.0f);
    }
}

