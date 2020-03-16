using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawn : MonoBehaviour {

    public ParticleSystem Flame;
    public float frequency = 1.0f;
    public float damage = 10.0f;

    private ContrMovem keysScript;
    private GunInfo infoScript;
    private Player2 playerScript;

    private ParticleSystem.EmissionModule em;

    private float time = 0.0f;

    private AudioSource source;
    private float vol = 0.0f;

    void Start()
    {
        keysScript = transform.parent.GetComponent<ContrMovem>();
        infoScript = transform.GetComponent<GunInfo>();
        playerScript = transform.parent.GetComponent<Player2>();
        em = Flame.emission;
        em.enabled = false;
        source = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (!playerScript.Dead)
        {
            if (keysScript.Fire1)
            {
                if (infoScript.canShoot)
                {
                    em.enabled = true;
                    vol += Time.deltaTime * 3;
                    vol = Mathf.Clamp(vol, 0.0f, 1.0f);
                    source.volume = vol * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                }
                else
                {
                    em.enabled = false;
                    vol -= Time.deltaTime * 6;
                    vol = Mathf.Clamp(vol, 0.0f, 1.0f);
                    source.volume = vol * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                }
                if(time > frequency && infoScript.canShoot)
                {
                    time = 0.0f;
                    infoScript.ShootFunc(); //wystrzelenie pocisku
                }
            }
            else
            {
                em.enabled = false;
                vol -= Time.deltaTime * 6;
                vol = Mathf.Clamp(vol, 0.0f, 1.0f);
                source.volume = vol * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            }
            if (time < frequency)
            {
                time += Time.deltaTime;
            }
        }
    }
}
