using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour {

    public float multipler = 1.0f;
    public float resTime = 30.0f;

    private Vector3 position;
    private Rigidbody Rb;

    public AudioClip sound;

    void Start()
    {
        Rb = transform.GetComponent<Rigidbody>();
        position = Rb.position;
        RaycastHit h;
        if (Physics.Raycast(position, Vector3.down, out h, 5.0f))
        {   //od razu ustawiam urzadzenie na miejscu
            position.y = h.point.y + 0.5f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AttachedWep script = other.transform.root.GetComponent<AttachedWep>();
            if (script)
            {
                if (script.CanTake())
                {
                    script.AmmoBox(multipler);
                    Invoke("enable", resTime);
                    transform.gameObject.SetActive(false);
                    if(sound)
                    script.GetComponent<AudioSource>().PlayOneShot(sound);
                }

                GunInfo component = other.transform.root.GetComponentInChildren<GunInfo>();
                if (component != null)  //system bezposrednio uzupelnia ammo do aktualnie uzywanej broni
                {
                    if (component.actualAmmo < component.maxAmmo)
                        component.actualAmmo = (int)Mathf.Clamp(component.maxAmmo * multipler + component.actualAmmo, 0.0f, component.maxAmmo); //jak nie moge tego zrobic w attachedWep to tu zrobie
                }
            }
        }
    }

    void enable()
    {
        transform.gameObject.SetActive(true);
    }

    void Update()
    {
        Rb.MoveRotation(Quaternion.Euler(0.0f, Rb.rotation.eulerAngles.y + 180.0f * Time.deltaTime, 0.0f));
        Rb.MovePosition(new Vector3(position.x, position.y + Mathf.Sin(Time.time * 2.0f) * 0.1f, position.z));
    }
}
