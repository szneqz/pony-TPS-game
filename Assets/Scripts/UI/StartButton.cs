using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StartButton : MonoBehaviour {

    public PassDataScript dataScript;
    public Texture2D ProgressBarUI;
    private AsyncOperation async = null;

    private void Start()
    {
        if (!dataScript)
           dataScript = StaticInfo.datScript;
    }

    public void LoadByIndex(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    public void LoadMap()
    {
        int sceneIndex = dataScript.prop.level;
        StartCoroutine(LoadLevel(sceneIndex));
    }

    private IEnumerator LoadLevel(int Level)
    {
        async = SceneManager.LoadSceneAsync(Level, LoadSceneMode.Single);
        yield return async;
        //Debug.Log(async);
    }

    public void datDiscornnect()
    {
            GameObject.Find("HostOrJoin").GetComponent<NetworkManager>().StopHost();
            GameObject.Find("HostOrJoin").GetComponent<NetworkManager>().StopClient();
    }

    private void OnGUI()
    {
        if (async != null)
        {
            GUI.DrawTexture(new Rect(Screen.width * 0.2f, Screen.height * 0.45f, Screen.width * 0.6f * async.progress, Screen.height * 0.05f), ProgressBarUI);
        }
    }

}
