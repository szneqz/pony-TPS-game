using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Chat : NetworkBehaviour {

    public EventSystem EventSys;
    public GameObject singleMessage, content;
    public InputField chatField;

    private Player2 playerScript;

    private string playerName = "   ";
    private bool isSetted = false;

    private string actText = "";
    private Text textMsg;

    public Image scrollView;
    public Image inputViewImage;
    public Text placeHolder;
    public Text textWrited;
    public Image scrollBarVertical;
    public Image scrollBarHandle;

   // private Color textMsgColor;
    private Color scrollViewColor;
    private Color inputViewImageColor;
   // private Color placeHolderColor;
    private Color textWritedColor;
    private Color scrollBarVerticalColor;
    private Color scrollBarHandleColor;

    private float colorIntensity = 0.0f;

    private ScrollRect scrollBarVerticalOptions;

    private void Start()
    {
        textMsg = Instantiate(singleMessage, content.transform).GetComponent<Text>();
        //textMsgColor = textMsg.color;
        scrollViewColor = scrollView.color;
        inputViewImageColor = inputViewImage.color;
      //  placeHolderColor = placeHolder.color;
        textWritedColor = textWrited.color;
        scrollBarVerticalColor = scrollBarVertical.color;
        scrollBarHandleColor = scrollBarHandle.color;

        scrollBarVerticalOptions = scrollBarVertical.transform.parent.GetComponent<ScrollRect>();
    }

    void Update ()
    {
        if (!isSetted)
        {
            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (a.name == "Character_Wings")
                {
                    if (a.transform.parent.GetComponent<Player2>().isLocalPlayer)
                    {
                        playerScript = a.transform.parent.GetComponent<Player2>();
                        playerName = a.transform.parent.GetComponent<AddInfoPlayer>().nickName;
                        isSetted = true;
                        break;
                    }
                }
            }
        }
        //Debug.Log(chatField.isFocused + " " + Input.GetKeyDown(KeyCode.Return));
        if (!chatField.isFocused && Input.GetKeyDown(KeyCode.T))
        {
            EventSys.SetSelectedGameObject(chatField.gameObject);
        }
		if(chatField.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            playerScript.sendMsg("<b>" + playerName + ":</b> " + chatField.text);
            chatField.text = "";
            EventSys.SetSelectedGameObject(null);
            scrollBarVerticalOptions.verticalNormalizedPosition = 0.0f; //po wyslaniu wiadomosci leci na sam dol
        }

        if(chatField.isFocused)
        {
            colorIntensity = Mathf.Clamp(colorIntensity + Time.deltaTime, 0.0f, 1.0f);
            scrollBarVerticalOptions.velocity = new Vector2(0.0f, Mathf.Sign(Input.mouseScrollDelta.y));
        }
        else
        {
            colorIntensity = Mathf.Clamp(colorIntensity - Time.deltaTime, 0.0f, 1.0f);
        }

        //textMsg.color = textMsgColor * colorIntensity;
        scrollView.color = new Color(scrollViewColor.r, scrollViewColor.g, scrollViewColor.b, scrollViewColor.a * (0.5f + 0.5f * colorIntensity));
        inputViewImage.color = new Color(inputViewImageColor.r, inputViewImageColor.g, inputViewImageColor.b, inputViewImageColor.a * colorIntensity);
        textWrited.color = new Color(textWritedColor.r, textWritedColor.g, textWritedColor.b, textWritedColor.a * colorIntensity);
        scrollBarVertical.color = new Color(scrollBarVerticalColor.r, scrollBarVerticalColor.g, scrollBarVerticalColor.b, scrollBarVerticalColor.a * colorIntensity);
        scrollBarHandle.color = new Color(scrollBarHandleColor.r, scrollBarHandleColor.g, scrollBarHandleColor.b, scrollBarHandleColor.a * colorIntensity);
    }

    [ClientRpc]
    void RpcMsgSend(string text)
    {
        textMsg.text = text;
    }

    [Command]
    public void CmdMsgReceive(string text)
    {
        actText = actText + "\n" + text;
        if(actText.Length > 10000)
        {
            actText = actText.Substring(actText.Length - 10000);  //zakaz przekraczania progu znakow
        }
        RpcMsgSend(actText);
    }
}
