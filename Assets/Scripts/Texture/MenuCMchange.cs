using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCMchange : MonoBehaviour {

    public Material material;
    public string strName;
    private ButValue valScript;
    private int actualNr = 0;

    private void Start()
    {
        valScript = transform.GetComponent<ButValue>();
    }

    void Update () {
        if (actualNr != valScript.value)
        {
            material.SetTexture(strName, StaticInfo.datScript.cmTexList[valScript.value]);
            actualNr = valScript.value;
        }
	}
}
