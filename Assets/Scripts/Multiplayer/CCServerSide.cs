using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CCServerSide : NetworkBehaviour {

    private float time = 0.0f;
    //private List<CheckCheating> datList;

    //private void Start()
    //{
    //    datList = new List<CheckCheating> { };
    //}

    //[ClientRpc]
    //private void RpcUpdate(float timme)
    //{
    //    datList[datList.Count - 1].myTime = timme;
    //}

    void Update ()
    {
        time += Time.smoothDeltaTime;
        //Debug.Log(time + " SERVER");
            if (time >= 5.0f)
        {

            List<CheckCheating> datList = new List<CheckCheating> { };

            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (a.transform == a.transform.root && a.GetComponent<CheckCheating>() != null)
                {
                    datList.Add(a.GetComponent<CheckCheating>());

                    datList[datList.Count - 1].TargetAskForData(a.GetComponent<NetworkIdentity>().connectionToClient);
                }
            }

            //Debug.Log(datList.Count);

            foreach (CheckCheating a in datList)
            {
                Debug.Log("SERV: " + time + " LOC: " + a.myTime + " " + a.GetComponent<AddInfoPlayer>().nickName);
                if (a.myTime > time + 1.0f)
                {
                    //Debug.Log("CHEATING!");
                    a.GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
                }
            }

            time = 0.0f;
        }
    }
}
