using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDetection : MonoBehaviour {

    private ParticleSystem.EmissionModule em;
    private ParticleSystem.ShapeModule shape;
    private Transform spine2;

    private FireSpawn fireScript;
    private HealthPoints hpScript;
    private AddInfoPlayer infoScript;
    private float onFireDamage = 5.0f;
    private float damage = 0.0f;
    private float time = 0.0f;
    private float time2 = 0.0f;
    private float onFireTime = 0.0f;
    private float onFireTime2 = 0.0f;

    public AudioSource firesource;

    private void Start()
    {
        spine2 = transform.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2");
        hpScript = transform.GetComponent<HealthPoints>();
        infoScript = transform.GetComponent<AddInfoPlayer>();
        em = transform.GetComponent<ParticleSystem>().emission;
        em.enabled = false;
        shape = transform.GetComponent<ParticleSystem>().shape;
        firesource.volume = firesource.volume * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.name == "Flame" && time <= 0.0f && (infoScript.team == 0 || infoScript.team != other.transform.parent.root.GetComponent<AddInfoPlayer>().team) && other.transform.parent.root != transform)
        {
            damage = other.transform.parent.GetComponent<FireSpawn>().damage;
            time = 0.5f;
            time2 = 1.0f;
            onFireTime = 4.0f;
            hpScript.damage = damage;
            em.enabled = true;
            if (!firesource.isPlaying)
            {
                firesource.volume = firesource.volume * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                firesource.timeSamples = (int)Random.Range(0.0f, firesource.clip.length);
                firesource.Play();
            }
        }
    }

    private void Update()
    {
        shape.position = transform.InverseTransformPoint(spine2.position);  //sprawia, ze plomien jest przypiety do spine2 (musi byc tu poniewaz spine2 po smierci znika)

        if (onFireTime > 0.0f)
        {
            time -= Time.deltaTime;
            time2 = time + 0.5f;
            onFireTime = time + 2.5f;
            onFireTime2 -= Time.deltaTime;
        }
        else
        {
            em.enabled = false;
            firesource.Stop();
        }
        if(time2 <= 0.0f)
        {
            damage = 0.0f;
        }
        if (onFireTime > 0.0f && onFireTime2 <= 0.0f && time2 < -0.5f)
        {
            hpScript.damage = onFireDamage;
            onFireTime2 = 0.5f;
        }
    }
}
