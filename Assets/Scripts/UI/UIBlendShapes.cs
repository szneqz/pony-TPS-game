using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlendShapes : MonoBehaviour {

    SkinnedMeshRenderer smr;

	void Start () {
        smr = transform.GetComponent<SkinnedMeshRenderer>();
	}

    public void changingGender(int value)
    {
        smr.SetBlendShapeWeight(46, value);
    }
	
}
