using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPoints : NetworkBehaviour {

    public float damage = 0.0f;

    public int MaxHP = 0;
    public int StartHP = 0; //aktualne HP
    private float resistTime = 0.2f;
    public float time = 0.0f;
   // private ContrMovem movScript;
    private Player2 playerScript;
    private WepChange wepScript;
    private ContrMovem cntr;
    // private AddInfoPlayer addInfoP;

    void Start()
    {
       // movScript = transform.GetComponent<ContrMovem>();
        playerScript = transform.GetComponent<Player2>();
        wepScript = transform.GetComponent<WepChange>();
        cntr = transform.GetComponent<ContrMovem>();
       // addInfoP = transform.GetComponent<AddInfoPlayer>();
    }

    private void ifNoHP(int HP)
    {
        if (HP <= 0)
        {
            if (playerScript != null)
            {
                if (!cntr.ifBot && playerScript.ifDead == false)
                    wepScript.upList(); //update listy broni po smierci
                playerScript.ifDead = true;
                StartHP = MaxHP;
            }
        }
    }

    [ClientRpc]
    //void RpcChangeHP(int HP, int maxHealthPoints)
    void RpcChangeHP(float DMG, int rac)
    {
        if (rac == 0)
            MaxHP = 120;
        if (rac == 1)
            MaxHP = 80;
        if (rac == 2)
            MaxHP = 100;

        if (StartHP > MaxHP)
            StartHP = MaxHP;

        time += Time.deltaTime;
        time = Mathf.Clamp(time, 0.0f, 1.0f);
        if (DMG > 0.0f && time >= resistTime)
        {
            if (playerScript.fly)
                DMG *= 2.0f; //postac w locie otrzymuje podwojony DMG
            StartHP -= (int)DMG;
            DMG = 0.0f;
            time = 0.0f;
            damage = 0.0f;
            ifNoHP(StartHP);
            //CmdChangeHP(StartHP);   //wysylanie informacji o HP do wszystkich graczy (dzieje się to po stronie klienta - lagujacy klient nie dostanie DMG)
        }
        //StartHP = HP;
        //MaxHP = maxHealthPoints;
        //damage = 0.0f;
        //ifNoHP(HP);
    }

    //[Command]
    //public void CmdChangeHP(int HP)
    //{
    //    StartHP = HP;
    //    RpcChangeHP(HP);
    //}

    //[Command]
    //private void CmdPrevHP(float DMG, int rac)
    //{
    //    if (rac == 0)
    //        MaxHP = 120;
    //    if (rac == 1)
    //        MaxHP = 80;
    //    if (rac == 2)
    //        MaxHP = 100;

    //    if (StartHP > MaxHP)
    //        StartHP = MaxHP;

    //    time += Time.deltaTime;
    //    time = Mathf.Clamp(time, 0.0f, 1.0f);
    //    if (DMG > 0.0f && time >= resistTime)
    //    {
    //        if (playerScript.fly)
    //            DMG *= 2.0f; //postac w locie otrzymuje podwojony DMG
    //        StartHP -= (int)DMG;
    //        DMG = 0.0f;
    //        time = 0.0f;
    //        RpcChangeHP(StartHP, MaxHP);
    //        //CmdChangeHP(StartHP);   //wysylanie informacji o HP do wszystkich graczy (dzieje się to po stronie klienta - lagujacy klient nie dostanie DMG)
    //    }
    //}

    [ClientRpc]
    private void RpcAddHP(int datHP)
    {
        StartHP += datHP;
        if (StartHP > MaxHP)
            StartHP = MaxHP;
    }

    //[Command]
    //private void CmdAddHP(int HP)
    //{
    //    StartHP += HP;
    //    if (StartHP > MaxHP)
    //        StartHP = MaxHP;

    //    RpcAddHP(StartHP);
    //}

    void Update()
    {
        if(isServer)
        {
            //if (StartHP > MaxHP)
            //    StartHP = MaxHP;

            //time += Time.deltaTime;
            //time = Mathf.Clamp(time, 0.0f, 1.0f);
            //if (damage > 0.0f && time >= resistTime)
            //{
            //    if (playerScript.fly)
            //        damage *= 2.0f; //postac w locie otrzymuje podwojony DMG
            //    StartHP -= (int)damage;
            //    damage = 0.0f;
            //    time = 0.0f;
            //    CmdChangeHP(StartHP);   //wysylanie informacji o HP do wszystkich graczy (dzieje się to po stronie klienta - lagujacy klient nie dostanie DMG)
            //}

           RpcChangeHP(damage, playerScript.race);
        }
	}

    public bool CanTake()
    {
        if (StartHP < MaxHP)
            return true;
        else return false;
    }

    public void HealthBox(float multipler)
    {
        if(isServer)
        RpcAddHP((int)(MaxHP * multipler));
        //CmdPrevHP(-(int)Mathf.Clamp(MaxHP * multipler, 0.0f, MaxHP), playerScript.race);
        //StartHP = (int)Mathf.Clamp(StartHP + MaxHP * multipler, 0.0f, MaxHP);
    }
}
