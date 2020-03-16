using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUnreal : MonoBehaviour {

    public float velocitytrue = 5.0f;
    public float maxH = 20.0f;
    public float minH = -1.2f;
    private float velocity = 5.0f;
    private Rigidbody rb;
    private bool ifgoto = false;
    private bool ifmovesup = false;
    private bool ifmovedown = false;
    private float time = 0.0f;


	void Start ()
    {
        rb = this.transform.GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!(ifmovesup || ifmovedown))
        {
            ifgoto = true;
        }
    }


    void Update ()
    {
        if (ifgoto)
        {
            time += Time.deltaTime;
            if (time > 1.0f)
            {
                ifmovesup = true;
                ifgoto = false;
                time = 0.0f;
            }
        }
        else
        {
            if (rb.position.y >= maxH && ifmovesup)
            {
                rb.MovePosition(new Vector3(rb.position.x, maxH, rb.position.z));
                time += Time.deltaTime;
                if (time >= 3.0f)
                {
                    velocity = -velocitytrue / 2;
                    ifmovesup = false;
                    ifmovedown = true;
                    time = 0.0f;
                }
            }
            if (rb.position.y <= minH && ifmovedown)
            {
                rb.MovePosition(new Vector3(rb.position.x, minH, rb.position.z));
                ifmovedown = false;
                velocity = velocitytrue;
                time = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if ((ifmovesup || ifmovedown) && time == 0.0f)
        {
            rb.MovePosition(new Vector3(rb.position.x, rb.position.y + velocity * Time.fixedDeltaTime, rb.position.z));
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
