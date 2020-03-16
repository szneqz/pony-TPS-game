using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour {

    private ColorPicker script;
    public Button but;

    private void Start()
    {
        script = transform.GetComponent<ColorPicker>();
    }

    void Update ()
    {
		if(script && but)
        {
            but.image.color = script.CurrentColor;
        }
	}

    public void ChangeButton(Button clickedButton)
    {
        script.CurrentColor = clickedButton.image.color;
        but = clickedButton;
    }
}
