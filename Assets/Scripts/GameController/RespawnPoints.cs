using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoints : MonoBehaviour
{
    public List<GameObject> respawns;
    public Transform bestRespawn;
    private List<GameObject> players = new List<GameObject>();
    private List<float> distances = new List<float>();

    public float seconds = 0.0f;

    private void Start()
    {
        foreach (GameObject a in respawns)
        {
            distances.Add(10000.0f);
        }
    }

    private void Update()
    {
        seconds += Time.deltaTime;  //sprawdzam stan raz na sekunde

        if(seconds >= 1.0f)
        {
            seconds = 0.0f;

            if(players.Count > 0)
            players.Clear();

            GameObject[] playersTemp = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject a in playersTemp)
            {
                if(!players.Contains(a.transform.root.gameObject))
                {
                    if (a.transform.root.gameObject.transform.GetComponent<Player2>() != null)
                    {
                        if (a.transform.root.gameObject.activeInHierarchy && !(a.transform.root.gameObject.transform.GetComponent<Player2>().Dead))
                        {
                            players.Add(a.transform.root.gameObject);   //lista zywych i aktualnie grajacych postaci
                        }
                    }
                }
            }

            int i = 0;

            for(i = 0; i < distances.Count; i++)
            {
                distances[i] = 10000.0f;
                //i++;
            }

            i = 0;

            foreach (GameObject a in players)
            {
                i = 0;
                foreach (GameObject b in respawns)
                {
                    if(Vector3.Distance(a.transform.position, b.transform.position) < distances[i])
                    distances[i] = Vector3.Distance(a.transform.position, b.transform.position);
                    i++;
                }
            }

            float sdist = 0.0f;
            i = 0;

            foreach (float dist in distances)
            {
                if(dist > sdist)
                {
                    sdist = dist;
                    bestRespawn = respawns[i].transform;
                }
                i++;
            }

            //Debug.Log(bestRespawn.name);
        }
    }

}