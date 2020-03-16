using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespPlayer : MonoBehaviour {

    private Player2 playerScript;
    private HealthPoints HPScript;
    //  private RespawnPoints pointScript;
    public float realRespawnTime = 5.0f;
    private float respTime;

    private void Start()
    {
        playerScript = GetComponent<Player2>();
        HPScript = GetComponent<HealthPoints>();
        respTime = realRespawnTime;
     //   pointScript = GameObject.FindWithTag("GameController").GetComponent<RespawnPoints>();
    }

    private void Update()
    {
        if(playerScript.Dead)
        {
            respTime -= Time.deltaTime;
            if(respTime <= 0.0f)
            {
                playerScript.respawnMe = true;
            //    transform.position = pointScript.bestRespawn.transform.position;
                respTime = realRespawnTime;
          //      pointScript.seconds = 1.0f; //przy kazdym respawnie ma byc od razu aktualiowana informacja o pozycji by czasem w przeciagu jednej sekundy nie zrespilo sie wiele postaci w jednym miejscu
                HPScript.damage = 0.0f;
                HPScript.StartHP = HPScript.MaxHP;
            }
        }
    }
}
