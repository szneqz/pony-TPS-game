using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACID_Damage : MonoBehaviour {

    public GameObject particle;
    public Color partColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "MainCamera")
        {
            if (other.transform.root.tag == "Player")
            {
                if (other.transform == other.transform.root)
                {
                    GameObject datparticle = Instantiate(particle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                    ParticleSystem.MainModule mainy = datparticle.GetComponent<ParticleSystem>().main;
                    mainy.startColor = partColor;
                }
            }
            else
            {
                GameObject datparticle = Instantiate(particle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                ParticleSystem.MainModule mainy = datparticle.GetComponent<ParticleSystem>().main;
                mainy.startColor = partColor;

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            HealthPoints hpscript = other.transform.root.GetComponent<HealthPoints>();

            hpscript.damage = 20;
        }
    }
}
