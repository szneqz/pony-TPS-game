using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceChange : MonoBehaviour {

    public int NR = 0;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        StaticInfo.datScript.changingPlayerClass2(NR);
        StaticInfo.datScript.FromNormalToSerialized();
    }

    
}
