using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthBox : NetworkBehaviour
{
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

    [ClientRpc]
    void RpcServerEnter(GameObject playerGO)
    {
        HealthPoints script = playerGO.GetComponent<HealthPoints>();
        transform.gameObject.SetActive(false);
        Invoke("enable", resTime);
        if (sound)
            script.GetComponent<AudioSource>().PlayOneShot(sound);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            if (other.tag == "Player")
            {
                HealthPoints script = other.transform.root.GetComponent<HealthPoints>();
                if (script.CanTake())
                {
                    RpcServerEnter(other.transform.root.gameObject);
                    script.HealthBox(multipler);
                    Invoke("enable", resTime);
                    transform.gameObject.SetActive(false);
                    if (sound)
                        script.GetComponent<AudioSource>().PlayOneShot(sound);
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
