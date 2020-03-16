using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnim : MonoBehaviour {

    public bool ifThrowEmpty = false;

    private bool reloading;
    private float thisReloadTime;
    private float localReloadTime;
    private Transform magazine;
    private Transform spine2;
    private Vector3 oldPos;
    private ParticleSystem.EmissionModule em;
    public Color particleColor;

    private GunInfo gunInfoScript;

    private void Start()
    {
        particleColor = transform.parent.Find("CombinedMeshes").GetComponent<Merge>().magicColor;
        magazine = transform.Find("Magazine");
        oldPos = magazine.position - transform.position;
        spine2 = transform.parent.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2");
        gunInfoScript = transform.GetComponent<GunInfo>();
        ParticleSystem magParticle = magazine.GetComponent<ParticleSystem>();
        em = magParticle.emission;
        ParticleSystem.MainModule mainy = magParticle.main;
        mainy.startColor = new ParticleSystem.MinMaxGradient(particleColor);

        ParticleSystem gunParticle = transform.Find("Particles").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em2 = gunParticle.emission;
        ParticleSystem.MainModule mainy2 = gunParticle.main;
        mainy2.startColor = new ParticleSystem.MinMaxGradient(particleColor);
    }

    public void Reloading(float reloadTime)
    {
            if (!reloading)
            {
                reloading = true;
                thisReloadTime = reloadTime;
                localReloadTime = 0.0f;

                if (ifThrowEmpty)
                {
                    GameObject obj = Instantiate(magazine.gameObject);
                    obj.transform.position = magazine.position;
                    obj.AddComponent<BoxCollider>();
                    Rigidbody rigid = obj.AddComponent<Rigidbody>();
                    rigid.mass = 0.0f;
                    obj.transform.localScale = magazine.lossyScale;
                    obj.layer = 2;
                    Destroy(obj, 10.0f);
                }

                magazine.position = spine2.position;
            }
    }

    private void Update()
    {
    if (!gunInfoScript.canShoot)
    {
        if (reloading)
        {
            localReloadTime += Time.deltaTime;
            magazine.position = spine2.position + (transform.position - spine2.position + oldPos.x * transform.right + oldPos.y * transform.up + oldPos.z * transform.forward) * (localReloadTime / thisReloadTime);

                if (localReloadTime >= thisReloadTime)
                reloading = false;

                em.enabled = true;
        }
        else
            {
                em.enabled = false;
            }
     }
    else
        {
             magazine.position = transform.position + oldPos.x * transform.right + oldPos.y * transform.up + oldPos.z * transform.forward;
            localReloadTime = thisReloadTime;

            em.enabled = false;
        }
    }
}
