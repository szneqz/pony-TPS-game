using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MP_HighAuthority : NetworkBehaviour {

    [SyncVar]
    private int Tail = -1;
    [SyncVar]
    private int FrontHair = -1;
    [SyncVar]
    private int BackHair = -1;
    [SyncVar]
    private Color bodyColor;
    [SyncVar]
    private Color HairBGColor;
    [SyncVar]
    private Color HairOneColor;
    [SyncVar]
    private Color HairTwoColor;
    [SyncVar]
    private Color eyesColor;
    [SyncVar]
    private Color magicColor;
    [SyncVar]
    private int cm;
    [SyncVar]
    private int tailTex;
    [SyncVar]
    private int backhairTex;
    [SyncVar]
    private int fronthairTex;
    [SyncVar]
    private bool Gender;
    [SyncVar]
    private string nickName;
    private bool isSetted = false;

    public AddInfoPlayer addScript;
    [SyncVar(hook = "ChngRace")]
    private int race;

    //***SYNCHRO BRONI***
    public SyncListInt weapons;    //U - P - EP
    public bool reset = false;
    //***END***

    public override void OnStartClient()
    {
        ChngRace(race); //wywoluj przy kazdej inicjalizacji
    }

    private void ChngRace(int val)  //a potem przy kazdej zmianie
    {
        //Debug.Log("isHappening " + race + " to " + val);
        race = val;
        addScript.race = race;
    }

    [Command]
    void CmdMeshUpdate(int tail, int backHair, int frontHair, Color bodyColory, Color hairColor_1, Color hairColor_2, Color hairColor_3, 
        Color eyesColory, Color magicColory, int CM, int tailTexture, int backHairTexture, int frontHairTexture, bool gen, string name)
    {
        Tail = tail;
        FrontHair = frontHair;
        BackHair = backHair;

        bodyColor = bodyColory;
        HairBGColor = hairColor_1;
        HairOneColor = hairColor_2;
        HairTwoColor = hairColor_3;
        eyesColor = eyesColory;
        magicColor = magicColory;
        cm = CM;
        tailTex = tailTexture;
        backhairTex = backHairTexture;
        fronthairTex = frontHairTexture;
        Gender = gen;
        nickName = name;

    }

    [Command]
    void CmdChangeRace(int datRace)
    {
        race = datRace;
    }

    [Command]
    private void CmdChngWep(int a, int b, int c, int d, int e, int f, int g, int h, int i)
    {
            weapons.Clear();
            weapons.Add(a);
            weapons.Add(b);
            weapons.Add(c);
            weapons.Add(d);
            weapons.Add(e);
            weapons.Add(f);
            weapons.Add(g);
            weapons.Add(h);
            weapons.Add(i);
       // GetComponent<WepChange>().upList(); //updatuj liste za kazda zmiana
    }

    public override void OnStartLocalPlayer()   //rzucanie swoimi danymi na prawo i lewo!
    {
        if (!isLocalPlayer)
            return;

            CmdMeshUpdate(StaticInfo.datScript.prop.tail, StaticInfo.datScript.prop.backHair, StaticInfo.datScript.prop.frontHair,
                StaticInfo.datScript.prop.bodyColor, StaticInfo.datScript.prop.hairColor_1, StaticInfo.datScript.prop.hairColor_2, StaticInfo.datScript.prop.hairColor_3,
                 StaticInfo.datScript.prop.eyesColor, StaticInfo.datScript.prop.magicColor, StaticInfo.datScript.prop.CM, StaticInfo.datScript.prop.tail,
                 StaticInfo.datScript.prop.backHair, StaticInfo.datScript.prop.frontHair, StaticInfo.datScript.prop.ifMale, StaticInfo.datScript.prop.Name);

        // v LOKALNY WYGLAD
        transform.Find("CombinedMeshes").GetComponent<AddingObjects>().MeshUpdate(StaticInfo.datScript.prop.tail, StaticInfo.datScript.prop.backHair, StaticInfo.datScript.prop.frontHair,
                StaticInfo.datScript.prop.bodyColor, StaticInfo.datScript.prop.hairColor_1, StaticInfo.datScript.prop.hairColor_2, StaticInfo.datScript.prop.hairColor_3,
                 StaticInfo.datScript.prop.eyesColor, StaticInfo.datScript.prop.magicColor, StaticInfo.datScript.prop.CM, StaticInfo.datScript.prop.tail, 
                 StaticInfo.datScript.prop.backHair, StaticInfo.datScript.prop.frontHair);

        addScript.gender = StaticInfo.datScript.prop.ifMale;
        addScript.nickName = StaticInfo.datScript.prop.Name;
        CmdChngWep(StaticInfo.datScript.lo.Unicorn[0], StaticInfo.datScript.lo.Unicorn[1], StaticInfo.datScript.lo.Unicorn[2],
            StaticInfo.datScript.lo.Pegasus[0], StaticInfo.datScript.lo.Pegasus[1], StaticInfo.datScript.lo.Pegasus[2],
            StaticInfo.datScript.lo.EarthPony[0], StaticInfo.datScript.lo.EarthPony[1], StaticInfo.datScript.lo.EarthPony[2]);
        GetComponent<WepChange>().upList(); //updatuj liste za kazda zmiana

        isSetted = true;
    }

    private void Update()    //synchronizacja dla wszystkich dołączających (synchronziacja wstecz)
    {
        if(!isLocalPlayer && Tail != -1 && BackHair != -1 && FrontHair != -1 && !isSetted)
        {
            transform.Find("CombinedMeshes").GetComponent<AddingObjects>().MeshUpdate(Tail, BackHair, FrontHair, bodyColor, HairBGColor, HairOneColor, HairTwoColor, eyesColor, magicColor, cm, tailTex, backhairTex, fronthairTex);
            addScript.gender = Gender;
            addScript.nickName = nickName;
            isSetted = true;
        }
        if (isLocalPlayer && StaticInfo.datScript.prop.playerClass != race)
        {
            CmdChangeRace(StaticInfo.datScript.prop.playerClass);   //uzganiam z serwerem - reszta sobie da rade
        }
        if (isLocalPlayer && (weapons.Count < 9 || reset))
        {
            CmdChngWep(StaticInfo.datScript.lo.Unicorn[0], StaticInfo.datScript.lo.Unicorn[1], StaticInfo.datScript.lo.Unicorn[2],
            StaticInfo.datScript.lo.Pegasus[0], StaticInfo.datScript.lo.Pegasus[1], StaticInfo.datScript.lo.Pegasus[2],
            StaticInfo.datScript.lo.EarthPony[0], StaticInfo.datScript.lo.EarthPony[1], StaticInfo.datScript.lo.EarthPony[2]);  //synchro jeżeli wcześniej nie weszło
            GetComponent<WepChange>().upList(); //updatuj liste za kazda zmiana
            reset = false;
        }
        //Debug.Log(weapons.Count);
    }


}
