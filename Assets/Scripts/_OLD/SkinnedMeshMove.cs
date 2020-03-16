using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshMove : MonoBehaviour {

    SkinnedMeshRenderer SMR;

    void Start()
    {
    }

    void Update ()
    {
        SMR = GetComponent<SkinnedMeshRenderer>();
        SMR.GetComponent<Renderer>().transform.position = transform.parent.position;
	}
}
