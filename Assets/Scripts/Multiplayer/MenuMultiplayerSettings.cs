using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMultiplayerSettings : MonoBehaviour {

    private Button but1;
    private Button but2;
    private Text textt;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (but1 == null)
            {
                but1 = GameObject.Find("Canvas").transform.Find("MultiplayerSettingsPanel2").Find("StartHostButton").GetComponent<Button>();
                but1.onClick.AddListener(TaskStartServer);
            }
            if (but2 == null)
            {
                but2 = GameObject.Find("Canvas").transform.Find("MultiplayerSettingsPanel2").Find("StartClientButton").GetComponent<Button>();
                but2.onClick.AddListener(TaskStartClient);
            }
            if (textt == null)
            {
                textt = but2.transform.parent.Find("ClientINFO").Find("IPaddress").Find("Text").GetComponent<Text>();
            }
        }
    }

    private void TaskStartServer()
    {
        StartingAsHost();
    }

    private void TaskStartClient()
    {
        StartingAsClient(textt);
    }

    public void StartingAsHost()
    {
        GetComponent<NetworkManager>().StartHost();
    }

    public void StartingAsClient(Text IP)
    {
        NetworkManager comp = GetComponent<NetworkManager>();
        comp.networkAddress = IP.text;
       // int.Parse(IP.transform.parent.Find("PORT").Find("Text").GetComponent<Text>());
        comp.networkPort = int.Parse(IP.transform.parent.parent.Find("PORT").Find("Text").GetComponent<Text>().text);
        comp.StartClient();
    }

    //SZUKAJ NETWORKERROR
    //OC CLIENT DISCONNECT?
    //void OnFailedToConnect()
    //{
    //    Debug.Log("Could not connect to server: ");
    //}
}
