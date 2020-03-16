using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmountGUI : MonoBehaviour {

    public bool showAddedAmmo = false;
    public string wepName;

    private int actualAmmo = 0;
    private int ammoInMag = 0;
    private int screenH;
    private GUIStyle guiStyle = new GUIStyle();
    private GUIStyle guiStyle2 = new GUIStyle();

    private GunInfo infoScript;
    private Player2 playerScript;
    private ContrMovem movScript;

	void Start ()
    {
        infoScript = transform.GetComponent<GunInfo>();
        playerScript = transform.parent.GetComponent<Player2>();
        movScript = transform.parent.GetComponent<ContrMovem>();
        screenH = Screen.height;
        guiStyle.fontSize = (int)(0.1f * screenH);
        guiStyle2.fontSize = (int)(0.05f * screenH);
    }

    void Update()
    {
        if (infoScript)
        {
            actualAmmo = infoScript.actualAmmo;
            ammoInMag = infoScript.ammoInMag;
        }
    }

    void OnGUI()
    {
        if (playerScript.isLocalPlayer)
        {
            if (!playerScript.Dead && !movScript.ifBot)
            {
                GUI.Label(new Rect(5, screenH - 1.5f * guiStyle.fontSize, 100, 100), wepName, guiStyle2);

                if (infoScript)
                {
                    if (showAddedAmmo)
                    {
                        GUI.Label(new Rect(5, screenH - guiStyle.fontSize, 100, 100), (ammoInMag + actualAmmo).ToString(), guiStyle);   //wyswietla ammoMag+ammoAll
                    }
                    else
                    {
                        GUI.Label(new Rect(5, screenH - guiStyle.fontSize, 100, 100), (ammoInMag).ToString() + "/" + (actualAmmo).ToString(), guiStyle);  //wyswietla ammoMag/ammoAll
                    }
                }
            }
        }
    }
}
