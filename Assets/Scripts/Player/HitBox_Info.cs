using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Info : MonoBehaviour {

    public float DMGmultipler = 1.0f;
    public float damage = 0.0f;
    public int team = 0;
    public bool isTrigger = false;

    private HealthPoints HPscript;
    private AddInfoPlayer AddInfo;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        HPscript = transform.root.GetComponent<HealthPoints>();
        AddInfo = transform.root.GetComponent<AddInfoPlayer>();
    }

    void Update ()
    {
        if ((AddInfo.team == 0 || (AddInfo.team != team)) && damage > 0.0f) //jezeli jest to deathmatch lub sa to przeciwne druzyny
        {
            float random = Mathf.Round(Random.Range(-0.2f * damage, 0.2f * damage));     //losowa liczba by DMG byl lekko losowy
            HPscript.damage = ((damage + random) * DMGmultipler);
            HPscript.time = 1.0f;
            damage = 0.0f;
        }
        if (isTrigger && transform.GetComponent<Collider>().isTrigger == false)
            transform.GetComponent<Collider>().isTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1.0f && source)
        {
            source.volume = 1.0f;
            if (collision.relativeVelocity.magnitude < 10.0f)
                source.volume = collision.relativeVelocity.magnitude / 5.0f;
            source.Play();
        }
    }
}
