using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckCheating : NetworkBehaviour {

    public float myTime = 0.0f;
    //private AddInfoPlayer addScript;

    //private void Start()
    //{
    //    addScript = GetComponent<AddInfoPlayer>();
    //}

    [TargetRpc]
    public void TargetAskForData(NetworkConnection target)
    {
        if (isLocalPlayer)
        {
            CmdResponse(myTime);
            myTime = 0.0f;
        }
    }

    [Command]
    private void CmdResponse(float datTime)
    {
        myTime = datTime;
    }

	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
            myTime += Time.smoothDeltaTime;
	}

    private void OnGUI()
    {
        if(isLocalPlayer)
        GUI.Label(new Rect(100, 0, 100, 100), (myTime).ToString());  
    }
}
