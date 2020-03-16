using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorByName : MonoBehaviour {

    public string colorName;
    public Material material;
    private Image but;

    private void Start()
    {
        but = transform.GetComponent<Button>().image;
    }

    void Update () {
        material.SetColor(colorName, but.color);
	}
}
