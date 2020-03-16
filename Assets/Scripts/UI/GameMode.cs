using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ListWrapper
{
    public List<GameObject> myList; //lista list buttonów do wyświetlania
}

public class GameMode : MonoBehaviour {

    private Dropdown dropDown;

    public List<ListWrapper> nList = new List<ListWrapper>();

    private void Start()
    {
        dropDown = transform.GetComponent<Dropdown>();
    }

    public void ChangingSelcetion()
    {
        int value = dropDown.value;
        for(int i = 0; i < nList.Count; i++)
        {
            if (i == value)
            {
                foreach (GameObject go in nList[i].myList)
                    go.SetActive(true);
                continue;
            }
            else
            {
                foreach (GameObject go in nList[i].myList)
                    go.SetActive(false);
            }
        }
    }
}
