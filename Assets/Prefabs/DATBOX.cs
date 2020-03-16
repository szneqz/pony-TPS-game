using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATBOX : MonoBehaviour
{

    private float datX = 0.0f;
    private bool side = false;
    private float velocity = 0.0f;

    void FixedUpdate()
    {
        datX += Time.fixedDeltaTime * velocity;

        if (datX > 20.0f)
            side = false;
        if (datX < -10.0f)
            side = true;

        if (side)
            velocity += Time.fixedDeltaTime * 10;
        else
            velocity -= Time.fixedDeltaTime * 10;
        if (Mathf.Abs(velocity) > 30.0f)
            velocity = Mathf.Sign(velocity) * 30.0f;

        transform.position = new Vector3(datX, transform.position.y, transform.position.z);
    }
}
