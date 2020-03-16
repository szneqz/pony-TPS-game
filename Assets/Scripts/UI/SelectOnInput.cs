using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttonSelected;

	void Update ()
    {
        //Event e = Event.current;
        ////if (e.isKey)
        //    Debug.Log("Detected character: " + e.type);
        int e = 0;
        if(Input.inputString.Length > 0)
        e = Input.inputString[0];

        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false && !((e >= 65 && e <= 90) || (e >= 97 && e <= 122)))
        {   //to jest tylko do zalaczania pierwszego przycisku - reszta jest ogarnieta przez skryptu wewnątrz unity
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
        else
        {
            if((e >= 65 && e <= 90) || (e >= 97 && e <= 122))
            {   //jezeli wciskam literki to system utrzymje mnie w pisaniu
                if (eventSystem.currentSelectedGameObject != null)
                    buttonSelected = true;
                else
                    buttonSelected = false;
            }
            else
            {   //jezeli nic nie zaznaczone i wciskam cos innego to sprawdzam czy jest cos oznaczone - jezeli nie to oznaczam standard
                if (eventSystem.currentSelectedGameObject == null)
                    buttonSelected = false;
            }
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
