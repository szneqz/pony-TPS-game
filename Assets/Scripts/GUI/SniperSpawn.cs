using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperSpawn : MonoBehaviour
{

    public Texture2D crosshairImage;    //wybrana textura crosshaira

    private GunController gunScript;
    private Player2 playerScript;



    private void Start()
    {
        gunScript = transform.GetComponent<GunController>();
        playerScript = transform.parent.GetComponent<Player2>();
    }

    void OnGUI()
    {
        if (playerScript.isLocalPlayer)
        {
            float res = ((float)Screen.height / (float)crosshairImage.height) * 1.5f;
            float xMin = -(crosshairImage.width * res - Screen.width) / 2;   //wartosc x na ekranie
            float yMin = -(crosshairImage.height * res - Screen.height) / 2; //wartosc y na ekranie
            if (gunScript.ifScope)   //jezeli postac zyje to celownik rysowany jest jak dawniej w przeciwnym wypadku nie jest rysowany
            {
                GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width * res, crosshairImage.height * res), crosshairImage); //rysowanie
            }
            GUI.Label(new Rect(0, 0, 100, 100), (1.0f / Time.smoothDeltaTime).ToString());  //wyswietla FPSy
        }
    }
}
