using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeShadowsEnabling : MonoBehaviour {

    public GameObject realtimeShadows;

    void Update ()
    {
		if(realtimeShadows)
        {
            realtimeShadows.SetActive(StaticInfo.datScript.op.realTimeS);
        }
	}
}
