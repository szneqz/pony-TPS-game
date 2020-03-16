using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDestroy : MonoBehaviour {


	void Start ()
    {
        Invoke("destroyThis", 2);
	}

    private void destroyThis()
    {
        GameObject thisObject = this.gameObject;
        Destroy(thisObject);
    }
}
