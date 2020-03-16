using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraInMenu : MonoBehaviour {

    Transform mainCamera;
    public bool ifRotateToPlayer;

	void Start ()
    {
        mainCamera = Camera.main.transform;
	}

    public void setRotate(bool ifRot)
    {
        ifRotateToPlayer = ifRot;
    }

    void Update ()
    {
        Vector3 torot;
        Vector3 topos;

        if (ifRotateToPlayer)
        {
            torot = new Vector3(15.0f, 90.0f, 0.0f);
            topos = new Vector3(0.0f, 1.5f, -10.0f);
        }
        else
        {
            torot = new Vector3(-16.0f, 0.0f, 0.0f);
            topos = new Vector3(0.0f, 1.0f, -10.0f);
        }

            if (Vector3.Distance(mainCamera.eulerAngles, torot) > 0.01f || Vector3.Distance(mainCamera.position, topos) > 0.01f)
            {
               // mainCamera.eulerAngles = Vector3.Lerp(mainCamera.rotation.eulerAngles, new, Time.deltaTime * 2.0f);
                mainCamera.eulerAngles = new Vector3(Mathf.LerpAngle(mainCamera.eulerAngles.x, torot.x, Time.deltaTime * 2.0f), Mathf.LerpAngle(mainCamera.eulerAngles.y, torot.y, Time.deltaTime * 2.0f), Mathf.LerpAngle(mainCamera.eulerAngles.z, torot.z, Time.deltaTime * 2.0f));
                mainCamera.position = Vector3.Lerp(mainCamera.position, topos, Time.deltaTime * 2.0f);
            }
            else
            {
                mainCamera.eulerAngles = torot;
                mainCamera.position = topos;
            }

        //if(ifRotateToPlayer && mainCamera.rotation.eulerAngles.y < 90.0f)
        //      {
        //          mainCamera.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + 90.0f * Time.deltaTime, 0.0f);
        //      }
        //      else
        //      {
        //          if(mainCamera.rotation.eulerAngles.y > 90.0f)
        //              mainCamera.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        //          if(!ifRotateToPlayer && mainCamera.rotation.eulerAngles.y > 0.0f)
        //          {
        //              mainCamera.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y - 90.0f * Time.deltaTime, 0.0f);
        //          }
        //          if(!ifRotateToPlayer && mainCamera.rotation.eulerAngles.y > 180.0f)
        //          {
        //              mainCamera.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //          }
        //      }
    }
}
