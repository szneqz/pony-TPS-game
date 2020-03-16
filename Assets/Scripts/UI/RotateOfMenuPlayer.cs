using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateOfMenuPlayer : MonoBehaviour {

    public Transform player;
    private Slider slider;

    private void Start()
    {
        slider = transform.GetComponent<Slider>();
    }

    public void RotateStatue()
    {
        player.rotation = Quaternion.Euler(0.0f, slider.value * 360.0f, 0.0f);
    }
}
