using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightBlink : MonoBehaviour {

    public float totalSeconds;     // The total of seconds the flash wil last
    public float maxIntensity;     // The maximum intensity the flash will reach
    public Material myMaterial;        // Your light
    public Light myLight;
    public Color color;

    private void Start()
    {
        StartCoroutine("flashNow");
    }

    public IEnumerator flashNow()
    {
        float waitTime = totalSeconds / 2;
        float strenght = 0.0f;
        Color endcolor = color;
        Color mainColor = Color.black;

        // Get half of the seconds (One half to get brighter and one to get darker)
        while (true)
        {
            while (strenght < maxIntensity)
            {
                strenght += Time.deltaTime / waitTime;        // Increase intensity
                endcolor = color * strenght;
                mainColor.a = strenght;
                myLight.intensity = strenght;
                myMaterial.SetColor("_EmissionColor", endcolor);
                myMaterial.SetColor("_Color", mainColor);
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
            while (strenght > 0)
            {
                strenght -= Time.deltaTime / waitTime;        //Decrease intensity
                endcolor = color * strenght;
                mainColor.a = strenght;
                myLight.intensity = strenght;
                myMaterial.SetColor("_EmissionColor", endcolor);
                myMaterial.SetColor("_Color", mainColor);
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
