using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class datList
    {
        public List<GameObject> pri;
        public List<GameObject> sec;
        public List<GameObject> mel;
    }

public class WepList : MonoBehaviour {

    public List<datList> guns = new List<datList>();

}
