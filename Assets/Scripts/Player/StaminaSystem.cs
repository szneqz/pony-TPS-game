using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StaminaSystem : NetworkBehaviour {

    public bool useStamina = false;

    [SyncVar]
    private float stamina;
    public float maxStamina;
    private bool hold = true;

    private int screenH;
    private int screenW;
    private GUIStyle guiStyle = new GUIStyle();

    private Player2 playerScript;
    private ContrMovem contrScript;

	void Start ()
    {
        playerScript = transform.GetComponent<Player2>();
        contrScript = transform.GetComponent<ContrMovem>();

        stamina = maxStamina;

        screenH = Screen.height;
        screenW = Screen.width;
        guiStyle.fontSize = (int)(0.1f * screenH);
    }

    [Command]
    void CmdUpdateStamina(bool sign)
    {
        if(sign)
        {
            stamina -= 15.0f * Time.deltaTime;
        }
        else
        {
            float fakeSpeed = Mathf.Clamp(14.0f - playerScript.actualSpeed, 1.0f, 5.0f);
            stamina += (fakeSpeed) * Time.deltaTime;
        }
    }
	
	void Update ()
    {
        if (playerScript.Dead && isServer)
            stamina = maxStamina;


        stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);

        if(((contrScript.leftShift && hold) || (contrScript.leftShift && stamina > 25.0f)) && stamina > 0.0f && playerScript.actualSpeed > 2.2f)
        {
            hold = true;
            CmdUpdateStamina(true);
            useStamina = true;
        }
        else
        {
            hold = false;
            CmdUpdateStamina(false);
            useStamina = false;
        }
	}

    private void OnGUI()
    {
        if (!playerScript.Dead && !contrScript.ifBot && playerScript.isLocalPlayer)
        {
            GUI.Label(new Rect(screenW * 0.85f, screenH - 2.2f * guiStyle.fontSize, 100, 100), ((int)stamina).ToString(), guiStyle);
        }
    }
}
