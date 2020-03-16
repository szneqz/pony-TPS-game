using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingMenuSMR : MonoBehaviour {

    public Material material;
    public string strMat;
    public List<Texture2D> listTex;
    public ButValue valScript;
    public List<SkinnedMeshRenderer> listSMR;
    private SkinnedMeshRenderer smr;
    private int actualNr = 0;

    private void Start()
    {
        smr = transform.GetComponent<SkinnedMeshRenderer>();
    }

    void Update () {
		if(actualNr != valScript.value)
        {
            smr.sharedMesh = listSMR[valScript.value].sharedMesh;
            material.SetTexture(strMat, listTex[valScript.value]);
            actualNr = valScript.value;
        }
	}
}
