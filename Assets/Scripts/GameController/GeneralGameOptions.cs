using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class GeneralGameOptions : MonoBehaviour {

    public GameObject datCanvas;
    public InputField IField;
    //public PassDataScript dataScript;

    void Start ()
    {
        Cursor.visible = false; //nie widać kursora
        Cursor.lockState = CursorLockMode.Locked;   //kursor jest ograniczony na środku ekranu
        //datCanvas = GameObject.Find("GameController/Canvas").gameObject;
    }

    private void Update()
    {
        if (datCanvas.activeInHierarchy || IField.isFocused) //jezeli jest aktywna pauza
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;     //kursor jest nieograniczony
                Cursor.visible = true; //widać kursor
            }
        }
        else
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;   //kursor jest ograniczony na środku ekranu
                Cursor.visible = false; //nie widać kursora
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && datCanvas.activeInHierarchy)    //wlaczanie wylaczanie canvasa za pomoca esc
        {
            datCanvas.SetActive(false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !datCanvas.activeInHierarchy)
                datCanvas.SetActive(true);
        }
    }

}
