using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmountDisplayer : MonoBehaviour {

    public Slider mainSlider;
    public Text amountText;
    public float amount = 0;

    public void SubmitSliderSetting()
    {
        amount = mainSlider.value;
        amountText.text = mainSlider.value.ToString();
    }
}
