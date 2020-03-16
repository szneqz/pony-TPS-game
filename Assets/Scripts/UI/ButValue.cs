using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButValue : MonoBehaviour {

    public int maxValue;
    private Text text;
    public int value = 0;

    private void Start()
    {
        text = transform.GetComponent<Text>();
        changingValue(0);   //wywolanie, zeby liczba sie zmienila
    }

    public void changingValue(int i)
    {
        value = value + i;
        if (value > maxValue)
            value = 0;
        if (value < 0)
            value = maxValue;

        text.text = value.ToString();
    }

}
