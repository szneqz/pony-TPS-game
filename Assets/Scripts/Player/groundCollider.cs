using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCollider : MonoBehaviour {

    public bool collision = false;

    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.parent.GetComponent<Collider>());
        foreach (Collider c in transform.parent.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(c.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    void OnTriggerEnter(Collider target)
    {
            collision = true;   //wejscie w obiekt oznacza kolizję z najprawdopodbniej ziemia
    }

    void OnTriggerStay(Collider target)
    {
            collision = true;   //pozostanie w obiekcie (ziemi)
    }

    void OnTriggerExit(Collider target)
    {
            collision = false;  //opuszczenie obiektu (ziemi)
    }
}