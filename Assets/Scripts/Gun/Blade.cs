using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    public float damage = 90.0f;
    private int team = 0;
    private Transform player;
    private Transform mainCamera;
    private AudioSource source;
    public AudioClip hardsurface;
    public AudioClip body;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    void Start()
    {
        player = transform.root;
        team = player.GetComponent<AddInfoPlayer>().team;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, transform.GetComponent<Collider>());   //ignoruje kolizje z samym soba
        Physics.IgnoreCollision(mainCamera.GetComponent<Collider>(), transform.GetComponent<Collider>()); //ignoruje kolizje z kamera
        source = transform.parent.GetComponent<AudioSource>();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet" && collision.tag != "MainCamera")
        {
            if (collision.tag != "HitBox")
                source.PlayOneShot(hardsurface);
            Destroy();
        }
        if (collision.tag == "HitBox")
        {
            source.PlayOneShot(body);
            collision.GetComponent<HitBox_Info>().damage = damage;
            collision.GetComponent<HitBox_Info>().team = team;
            Destroy();
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet")
        {
            Destroy();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet")
        {
            Destroy();
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
