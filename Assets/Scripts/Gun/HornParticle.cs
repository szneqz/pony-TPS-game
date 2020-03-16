using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornParticle : MonoBehaviour {

    private float forward = 0.2f;
    private float up = -0.55f;
    private float rotationX = -65.0f;
    private Transform horn;
    private Transform mainTransform;

    private ParticleSystem.EmissionModule em2;
    private Player2 playerScript;


    void Start ()
    {
        horn = transform.root.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2/neck/head");
        mainTransform = transform.parent;
        if (horn != null)
        {
            Color particleColor = transform.root.Find("CombinedMeshes").GetComponent<Merge>().magicColor;
            ParticleSystem gunParticle = transform.GetComponent<ParticleSystem>();
            em2 = gunParticle.emission;
            ParticleSystem.MainModule mainy2 = gunParticle.main;
            mainy2.startColor = new ParticleSystem.MinMaxGradient(particleColor);
            playerScript = transform.root.GetComponent<Player2>();
            transform.parent = horn;
            transform.position = horn.position + horn.forward * forward + horn.right * up;
            transform.rotation = horn.rotation * Quaternion.Euler(0.0f, rotationX, 0.0f);
            transform.localScale = new Vector3(1.0f, 1.0f, 0.8f);
        }
    }
	

	void Update ()
    {
        if (mainTransform == null)
            Destroy(this.gameObject);

        if (horn == null)
        {
            horn = transform.root.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2/neck/head");
            Color particleColor = transform.parent.Find("CombinedMeshes").GetComponent<Merge>().magicColor;
            ParticleSystem gunParticle = transform.GetComponent<ParticleSystem>();
            em2 = gunParticle.emission;
            ParticleSystem.MainModule mainy2 = gunParticle.main;
            mainy2.startColor = new ParticleSystem.MinMaxGradient(particleColor);
            playerScript = transform.root.GetComponent<Player2>();
            transform.parent = horn;
            transform.position = horn.position + horn.forward * forward + horn.right * up;
            transform.rotation = horn.rotation * Quaternion.Euler(0.0f, rotationX, 0.0f);
            transform.localScale = new Vector3(1.0f, 1.0f, 0.8f);
        }
        else
        {
            //transform.position = horn.position + horn.forward * forward + horn.right * up;
            //transform.rotation = horn.rotation * Quaternion.Euler(0.0f, rotationX, 0.0f);

            if (playerScript.Dead)
                em2.enabled = false;
            else
                em2.enabled = true;
        }
    }
}
