using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHornColor : MonoBehaviour {

    public Image but;
    private ParticleSystem.MainModule mainy;

    void Start () {
        mainy = transform.GetComponent<ParticleSystem>().main;
    }

    private void Update()
    {
        mainy.startColor = new ParticleSystem.MinMaxGradient(but.color);
    }
	
}
