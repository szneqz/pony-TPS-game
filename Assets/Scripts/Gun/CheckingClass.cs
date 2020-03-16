using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingClass : MonoBehaviour {

    public bool earth;
    public bool unicorn;
    public bool pegasus;

    private AddInfoPlayer addInfoPlayerScript;

    private void Start()
    {
        addInfoPlayerScript = transform.parent.GetComponent<AddInfoPlayer>();
    }

    private void Update()
    {
        if (!((addInfoPlayerScript.race == 0 && earth) || (addInfoPlayerScript.race == 1 && unicorn) || (addInfoPlayerScript.race == 2 && pegasus)))
            Destroy(this.gameObject);
    }
}
