using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPlayerGUI : MonoBehaviour {

    private int screenH;
    private int screenW;
    private GUIStyle guiStyle = new GUIStyle();

    private HealthPoints hpScript;
    private Player2 playerScript;
    private ContrMovem movScript;

    void Start ()
    {
        hpScript = transform.GetComponent<HealthPoints>();
        playerScript = transform.GetComponent<Player2>();
        movScript = transform.GetComponent<ContrMovem>();
        screenH = Screen.height;
        screenW = Screen.width;
        guiStyle.fontSize = (int)(0.1f * screenH);
    }


    void OnGUI()
    {
        if(!playerScript.Dead && !movScript.ifBot && playerScript.isLocalPlayer)
        GUI.Label(new Rect(screenW * 0.85f, screenH - guiStyle.fontSize, 100, 100), (hpScript.StartHP).ToString(), guiStyle);   //wyswietla hp
    }
}
