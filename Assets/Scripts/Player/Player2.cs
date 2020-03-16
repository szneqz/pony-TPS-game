using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player2 : NetworkBehaviour
{
    private Transform mainTransform;        //glowy transform postaci
    private GameObject armature;             //wszelakie kosci
    private Animator anim;                   //zmienna animacji postaci
    private Rigidbody RB;                   //zmienna rigidbody postaci
    private PlayerSounds soundScirpt;       //skrypt dotyczacy dzwiekow (chod, klus, bieg)
    private float VerMv = 0.0f;             //wartosc przod/tyl
    private float HorMv = 0.0f;             //wartosc prawo/lewo
    public float actualSpeed = 0.0f;       //aktualna predkosc liniowa
    private float realSpeed = 0.0f;         //realna predkosc liniowa
    public float runSpeed = 10.0f;          //predkosc biegu
    public float trotSpeed = 5.0f;          //predkosc klusu
    public float walkSpeed = 2.0f;          //predkosc chodu
    private float speed = 0.0f;             //aktualnie wybrana predkosc
    private bool ground = true;             //czy dotyka ziemi
    private ContrMovem keysScript;          //skrypt do poruszania sie
    private groundCollider groundColscript; //script do ground
    private HealthPoints healthPointsscript;//script od damage
    private StaminaSystem staminaScript;    //scrpit od staminy
    private GameObject groundCol;            //boxcollider do groundCol
    private Transform spine2;                //spine2
    private float spine2rot = 0.0f;
    private Transform neck;                  //neck
    private Transform head;                  //head
    private Transform eyeBoneL;              //lewe oko
    private Transform eyeBoneR;              //prawe oko
    private Transform root;                  //kosc poczatkowa
    private float headRot = 0.0f;           //obrot glowy w poziomie
    public float headRotVer = 0.0f;        //obrot glowy w pionie
    private Collider playerCollider;        //collider postaci
    private Collider bodyCollider;
    private Collider headCollider;
    private float tenSecTime = 0.0f;        //zmienna liczaca 0.1 sekundy by raycasta rysowac max 10 razy na sekunde
    private float oneSecTime = 0.0f;        //zmienna liczaca 1 sekunde od skoku by postac nie skakala za czesto
  //  private float oneSecTime2 = 0.0f;       //to samo co wyżej tylko liczy czas bezpośrednio od skoku, a nie od ladowania
    private RaycastHit h;                   //zmienna zderzenia raycasta z ziemia
    private Vector3 endVector = Vector3.zero;   //wektor obrotu postaci przy ruchu
    private Vector3 camRot;                 //rotacja kamery
    public float rotationAngle = 90.0f;     //obrot przy ruchu postaci na sekunde przy klusie
    private bool ifgotoIdle = true;         //zmienna decydujaca o przejsciu w IDLE animacji
    private Vector3 lastRotation = Vector3.zero;//zmienna zapisujaca ostatnia rotacje w pionie
    private float maxAngle = 0.0f;          //zmienna sprawdzajaca kat nachylenia postaci
    public bool Dead = false;               //zmienna opisujaca czy postac zyje
    private float rootMoveTime = 0.0f;      //czas od braku ruchu
    private Vector3 prevRootPos;            //poprzednia pozycja root'a
    public bool ifDead = false;             //zmienna do ktroej przychodzi z zewnatrz info o smierci
    private Vector3 rootOldPos;             //umiejscowienie root'a
    private AddInfoPlayer addInfo;          //skrypt od informacji dodatkowych postaci
    private AttachedWep attachWepScript;    //skrypt informujacy o broniach postaci
    private WepChange wepChangeScript;      //skrypt dotyczacy zmiany broni
    private float fiveSecTime = 0.0f;       //wywolywanie powyzszego skryptu co 5 sekund
    public int race = 0;                   //rasa z powyzszego skryptu
    private SkinnedMeshRenderer characterWings;       //<--- cialo
    public bool fly = false;               //czy leci
    private float wingShape = 0.0f;         //jak bardzo zlozone sa skrzydla
    private float colVelY = 0.0f;           //predkosc ruchu Y
    private float FallColTime = 0.0f;       //czas liczony by zapobiegac glupiemu DMG
    private float noJumpTime = 0.0f;        //czas od puszczenia skoku
    private float noJumpTime2 = 0.0f;       //czas od puszczenia skoku w locie
    private float noFlyTime = 0.0f;         //czas od mocnego uderzenia postaci w locie do ponownej mozliwosci latania
    private float deadTime = 0.0f;          //czas od odrodzenia
    private bool wasDead = false;           //czy umarl ostatnio
    public bool respawnMe = false;          //czy respic z osobnego scripta
    //private bool isRebinded = false;        //rzucanie na koniec pierwszego update anim.Rebind(), bo w Start() nie dziala, a nie mozna uzywac ciagle
    //private Vector3 spine2Pos;
    private GameObject chatBox;

    private float fieldOfView;              //standardowy FOV kamery
    private Camera cameraMain;              //glowna kamera

    private RespawnPoints pointScript;
    private MP_HighAuthority HAScript;

    [Command]
    public void CmdUpdateRandoms(float X, float Y, int side)
    {
        RpcUpdateRandoms(X, Y, side);
    }

    [ClientRpc]
    private void RpcUpdateRandoms(float X, float Y, int side)
    {
        foreach(Transform child in mainTransform)
        {
            if(child.CompareTag("Gun"))
            {
                child.GetComponent<WhatScript>().sendThatInfo(X, Y, side);
            }
        }
    }

    [Command]
    private void CmdRecMsg(string text)
    {
        if(chatBox != null)
        {
            chatBox.GetComponent<Chat>().CmdMsgReceive(text);
        }
        else
        {
            chatBox = GameObject.FindGameObjectWithTag("Chat");
            if (chatBox != null)
                chatBox.GetComponent<Chat>().CmdMsgReceive(text);
        }
    }

    public void sendMsg(string text)
    {
        CmdRecMsg(text);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!fly && collision.relativeVelocity.y > 10.0f && FallColTime > 0.5f)
            colVelY = collision.relativeVelocity.y; //zmienna do liczenia DMG od upadku
        if (fly && realSpeed > 10.0f && FallColTime > 0.5f && !ground)
            colVelY = realSpeed * 2.0f;                    //zmienna do liczenia DMG od uderzenia 
        
    }

    private void OnCollisionExit(Collision collision)
    {
        FallColTime = 0.0f;
    }

    public void SetAllCollidersStatus(bool active)  //funkcja dotyczaca pozbycia sie wszystkich colliders'ow i nadania predkosci graczowi w strone jego ruchu
    {
        foreach (Rigidbody Rb in GetComponentsInChildren<Rigidbody>())
        {
            Rb.isKinematic = !active;   //wylaczam wszelka fizyke i kolizje na wszystkich rigidbody
            if (active)
            {
                Rb.angularVelocity = Vector3.zero;
                Rb.velocity = (Vector3.zero + RB.velocity) / 1.4f;  //predkosc ciala po zragodollowaniu (tu bede mogl kiedys dodac sile uderzenia pocisku)
            }
        }
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.isTrigger = !active;  //zalczamy isTrigger do sprawdzania obrazen od postrzalu
        }
        foreach (SkinnedMeshRenderer m in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            m.updateWhenOffscreen = active;
        }
    }

    public void playerDead(bool active)
    {
        SetAllCollidersStatus(active);  //wlaczenie/pozbycie sie wszystkich rigidbody
        RB.isKinematic = active;   //wlaczam/wylaczam rigidbody glownej postaci
        playerCollider.isTrigger = active;   //tutaj isTrigger musi byc zalezne od stanu
        bodyCollider.enabled = !active;
        headCollider.enabled = !active;
        bodyCollider.GetComponent<BoxCollider>().isTrigger = active;
        headCollider.GetComponent<CapsuleCollider>().isTrigger = active;
        groundCol.GetComponent<BoxCollider>().isTrigger = true; //tutaj isTrigger musi byc stale
        anim.enabled = !active; //wlaczam/wylaczam animacje
        if (active)
        {
            RB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;  //zamrazam rigidbody postaci
            wepChangeScript.Number = KeyCode.Alpha0;
            fly = false;    //lot zostaje wylaczony
        }
        else
        {
            RB.constraints = RigidbodyConstraints.FreezeRotation;   //odmrazam pozycje rigidbody postaci
            root.localPosition = rootOldPos;  //ogarniecie root'a by ten wracal na swoje miejsce (nie wiem dlaczego jako jedyny ze wszystkich bonów nie wie jak się zachować
            root.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }

    private void changeRaceAndGender(int race, bool gender)
    {
        if (race == 0)
        {
            staminaScript.maxStamina = 120; //stamina
            //healthPointsscript.MaxHP = 120; //zycie
            runSpeed = 12.0f;   //predkosc biegu
            trotSpeed = 6.0f;   //predkosc klusu
            rotationAngle = 150.0f; //predkosc obrotu
            characterWings.SetBlendShapeWeight(43, 0.0f);   //rozlozone skrzydla
            characterWings.SetBlendShapeWeight(44, 100.0f); //brak skrzydel
            characterWings.SetBlendShapeWeight(45, 100.0f); //brak rogu
        }
        if (race == 1)
        {
            staminaScript.maxStamina = 80;
            //healthPointsscript.MaxHP = 80;
            runSpeed = 8.0f;
            trotSpeed = 4.0f;
            rotationAngle = 130.0f;
            characterWings.SetBlendShapeWeight(43, 0.0f);   //rozlozone skrzydla
            characterWings.SetBlendShapeWeight(44, 100.0f); //brak skrzydel
            characterWings.SetBlendShapeWeight(45, 0.0f);   //jest rog
        }
        if (race == 2)
        {
            staminaScript.maxStamina = 100;
            //healthPointsscript.MaxHP = 100;
            runSpeed = 10.0f;
            trotSpeed = 5.0f;
            rotationAngle = 140.0f;
            characterWings.SetBlendShapeWeight(44, 0.0f); //sa skrzydla
            characterWings.SetBlendShapeWeight(45, 100.0f); //brak rogu
        }
        if (gender)
        {
            characterWings.SetBlendShapeWeight(46, 100.0f); //plec meska
        }
        else
        {
            characterWings.SetBlendShapeWeight(46, 0.0f); //plec zenska
        }
    }

    private void Start()
    {
        mainTransform = transform;

        armature = mainTransform.Find("Armature").gameObject;
        anim = mainTransform.GetComponent<Animator>();
        groundCol = mainTransform.Find("groundCollider").gameObject;
        spine2 = mainTransform.Find("Armature/MasterCtrl/hipsCtrl/root/spine1/spine2");
        neck = spine2.Find("neck");                  
        head = neck.Find("head");                  
        eyeBoneL = head.Find("sizeBone_L/eyeBone_L");
        eyeBoneR = head.Find("sizeBone_R/eyeBone_R");
        root = mainTransform.Find("Armature/MasterCtrl/hipsCtrl/root");
        bodyCollider = mainTransform.Find("bodyCollider").GetComponent<Collider>();
        headCollider = mainTransform.Find("headCollider").GetComponent<Collider>();
        chatBox = GameObject.FindGameObjectWithTag("Chat");

        cameraMain = Camera.main;
        fieldOfView = cameraMain.fieldOfView;

        anim.SetInteger("AnimPar", 0);

        rootOldPos = root.localPosition;
        RB = mainTransform.GetComponent<Rigidbody>();
        playerCollider = mainTransform.GetComponent<Collider>();

        groundColscript = groundCol.GetComponent<groundCollider>(); //odwolanie do skryptu od boxcollidera pod nogami
        keysScript = mainTransform.GetComponent<ContrMovem>();
        addInfo = mainTransform.GetComponent<AddInfoPlayer>();
        healthPointsscript = mainTransform.GetComponent<HealthPoints>();
        attachWepScript = mainTransform.GetComponent<AttachedWep>();
        wepChangeScript = mainTransform.GetComponent<WepChange>();
        staminaScript = mainTransform.GetComponent<StaminaSystem>();
        soundScirpt = mainTransform.GetComponent<PlayerSounds>();
        HAScript = mainTransform.GetComponent<MP_HighAuthority>();

        characterWings = mainTransform.Find("Character_Wings").GetComponent<SkinnedMeshRenderer>();

        changeRaceAndGender(addInfo.race, addInfo.gender);
        race = addInfo.race;

        Physics.IgnoreCollision(groundCol.GetComponent<Collider>(), GetComponent<Collider>());  //brak kolizji miedzy boxcolliderem od nog i tego collidera
        Physics.IgnoreCollision(groundCol.GetComponent<Collider>(), bodyCollider.GetComponent<Collider>()); //
        Physics.IgnoreCollision(groundCol.GetComponent<Collider>(), headCollider.GetComponent<Collider>()); //

        playerDead(false); //gracz zyje na poczatku
        //playerDead(true);
        //playerDead(false);

        pointScript = GameObject.FindWithTag("GameController").GetComponent<RespawnPoints>();
        healthPointsscript.damage = 1000;
        respawnMe = true;

        anim.Rebind();
    }

    bool isGrounded()   //sprawdza isGrounded 
    {
        if (groundColscript.collision == true)  //jezeli boxcollider dotyka powierzchni to jest sie uziemionym
            return true;
        else
            return false;
    }

    void animAnimator(int number)
    {
        if (isLocalPlayer)
        {
            anim.SetInteger("AnimPar", number);  //odgrywana animacja
            ifgotoIdle = false;     //wylaczanie IDLE
        }
    }

    void animations()
    {
        float locCamRotHor = RB.rotation.eulerAngles.y - keysScript.camRot.y;        //zmienna sprawdzajaca czy kamera jest po lewej czy po prawej stronie od gracza
        if (locCamRotHor < 0.0f) locCamRotHor += 360.0f;
        locCamRotHor -= 180.0f;

        ifgotoIdle = true;

        if (!fly)
        {   //jezeli nie lata
            if (race == 2)
            {
                characterWings.SetBlendShapeWeight(43, wingShape);   //zlozone skrzydla 
                wingShape += 100.0f * Time.deltaTime;
                wingShape = Mathf.Clamp(wingShape, 0.0f, 100.0f);
            }
            if (realSpeed <= (1.2f * trotSpeed) && realSpeed > 1.2f * walkSpeed && ground)
            {
                animAnimator(1);    //kłus
                soundScirpt.StartCoroutine(soundScirpt.trotSound());
            }
            if ((realSpeed > (1.2f * trotSpeed)) && ground)
            {
                animAnimator(6);    //bieg
                soundScirpt.StartCoroutine(soundScirpt.runSound());
            }
            if (((realSpeed > 0.2f * walkSpeed && realSpeed <= 1.2f * walkSpeed)) && ground)
            {
                animAnimator(7);    //chod
                soundScirpt.StartCoroutine(soundScirpt.stepSound());
            }
            if (locCamRotHor < 0 && locCamRotHor > -179.0f && keysScript.Fire2 && actualSpeed == 0.0f && ground)
            {
                animAnimator(2);    //obrot w lewo
            }
            if (locCamRotHor > 0 && locCamRotHor < 179.0f && keysScript.Fire2 && actualSpeed == 0.0f && ground)
            {
                animAnimator(3);    //obrot w prawo
            }
            if (keysScript.Fire2 && realSpeed > 0.2f * walkSpeed && HorMv < 0.0f && ground)
            {
                animAnimator(9);    //chod bokiem w lewo
                soundScirpt.StartCoroutine(soundScirpt.horizontalStepSound());
            }
            if (keysScript.Fire2 && realSpeed > 0.2f * walkSpeed && HorMv > 0.0f && ground)
            {
                animAnimator(10);   //chod bokiem w prawo
                soundScirpt.StartCoroutine(soundScirpt.horizontalStepSound());
            }
            if (keysScript.Fire2 && ((realSpeed > 0.2f * walkSpeed && realSpeed <= 1.2f * walkSpeed)) && VerMv < 0.0f && HorMv != 0.0f && ground)
            {
                animAnimator(8);    //chod tylem
                soundScirpt.StartCoroutine(soundScirpt.stepSound());
            }
            if (keysScript.Jump && ground && oneSecTime > 1.0f)  //skok na spacji
            {
                animAnimator(4);    //Jump start
            }
            if (!ground)    //spadek bez spacji
            {
                animAnimator(5);   //Fall
            }
            if ((anim.GetInteger("AnimPar") == 5 || anim.GetInteger("AnimPar") == 4) && ground)   //powrot do pozycji wyjsciowej po skoku/upadku
            {
                ifgotoIdle = true;
                soundScirpt.StartCoroutine(soundScirpt.dropSound());
            }
            if (ifgotoIdle)  //jezeli nic innego sie nie wykonuje to gotoIdle
            {
                if(isLocalPlayer)
                anim.SetInteger("AnimPar", 0);  //przy braku ruchu IDLE 
            }
        }
        else
        {   //jezeli lata
            characterWings.SetBlendShapeWeight(43, wingShape);   //rozlozone skrzydla 
            wingShape -= 100.0f * Time.deltaTime;
            wingShape = Mathf.Clamp(wingShape, 0.0f, 100.0f);

            float flyModifier = 1.5f;
            if (realSpeed <= (1.6f * trotSpeed * flyModifier) && realSpeed > 1.4f * walkSpeed * flyModifier)
            {
                animAnimator(13); //TROT FLY
                soundScirpt.StartCoroutine(soundScirpt.wingSound(0.45f));
            }
            if (realSpeed > (1.6f * trotSpeed * flyModifier) && realSpeed <= (runSpeed * flyModifier))
            {
                animAnimator(14); //FAST FLY
                soundScirpt.StartCoroutine(soundScirpt.wingSound(0.35f));
            }
            if (realSpeed > (runSpeed * flyModifier))
            {
                animAnimator(15); //SLIDING FLY
            }
            if ((realSpeed > 0.4f * walkSpeed * flyModifier && realSpeed <= 1.4f * walkSpeed * flyModifier))
            {
                animAnimator(12); //SLOW FLY
                soundScirpt.StartCoroutine(soundScirpt.wingSound(0.65f));
            }
            if (ifgotoIdle)  //jezeli nic innego sie nie wykonuje to gotoIdle
            {
                if(isLocalPlayer)
                anim.SetInteger("AnimPar", 11);  //przy braku ruchu STEADY FLY
                soundScirpt.StartCoroutine(soundScirpt.wingSound(0.7f));
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.J))
        //    anim.Rebind();

        if (!keysScript.ifBot && !(keysScript.Fire2 && cameraMain.fieldOfView == fieldOfView && Vector3.Angle(mainTransform.forward, cameraMain.transform.forward) < 30.0f)) //jezeli nie trzyma PPM to kamera nie jest zblizona - przydatne przy strzelaniu z sniper rifla
        cameraMain.fieldOfView = fieldOfView;

        if(noFlyTime > 0.0f)
        {
            fly = false;
            noFlyTime -= Time.deltaTime;
        }
        if (noFlyTime < 0.0f)
            noFlyTime = 0.0f;

        if (Dead)
        {
            deadTime = 0.0f;
            wasDead = true;

            Vector3 rootPos = new Vector3(Mathf.Round(root.position.x * 100), Mathf.Round(root.position.y * 100), Mathf.Round(root.position.z * 100));
            if (prevRootPos != rootPos)     //gdy postac umrze to szkielet ma zniknac by nie kolidowac
                rootMoveTime = 0.0f;
            else
            {
                if (rootMoveTime >= 0.1f)
                    armature.SetActive(false);
            }
            if (rootMoveTime >= 0.1f)
                rootMoveTime = 0.0f;
            rootMoveTime += Time.deltaTime;
            prevRootPos = rootPos;
        }   // /Dead
        if (ifDead && Dead == false)
        {
            RB.useGravity = false; //pozbywamy sie grawitacji
            playerDead(true);
            Dead = true;
            HAScript.reset = true;  //przy smierci postac ponownie wczytuje dane na temat broni
            wepChangeScript.upList();
        }
        else if (respawnMe && Dead == true)                  //<----------------------------------------- to bedzie do zmiany - wstawi sie tu normalny czas odrodzenia itp. blah blah
        {
            armature.SetActive(true);   //po odrodzeniu szkielet ma wrocic
            RB.useGravity = true;  //zwracamy grawitacje
            Dead = false;
            playerDead(Dead);
            actualSpeed = 0.0f; //usuwam aktualna predkosc postaci by po wstaniu nie pedzila
            anim.Rebind();      //ustawienie kosci w odpowiednim miejscu
            wepChangeScript.ifDestroyWep = true;
            healthPointsscript.StartHP = healthPointsscript.MaxHP;
            //if(isLocalPlayer)
            //{
            //    healthPointsscript.CmdChangeHP(healthPointsscript.MaxHP);
            //}
            healthPointsscript.damage = 0.0f;
            if(isServer)
            healthPointsscript.HealthBox(1.0f); //pelne HP po respawnie
            ifDead = false;
            respawnMe = false;
            mainTransform.position = pointScript.bestRespawn.transform.position;
            pointScript.seconds = 1.0f; //przy kazdym respawnie ma byc od razu aktualiowana informacja o pozycji by czasem w przeciagu jednej sekundy nie zrespilo sie wiele postaci w jednym miejscu
            changeRaceAndGender(addInfo.race, addInfo.gender);  //zmiana klasy po smierci
            race = addInfo.race;
        }

        if (Dead == false)
        {
            root.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);  //<------------ po aktualizacji 2018.1 musiałem to dodać, bo się wszystko popsuło!!!!!!

            deadTime += Time.deltaTime;
            deadTime = Mathf.Clamp(deadTime, 0.0f, 0.6f);

            if (deadTime > 0.5f && wasDead)
            {
                attachWepScript.Died(true); //usuwam wszystkie informacje o broniach postaci po smierci
                wasDead = false;
            }

            if (!keysScript.Jump && !ground && race == 2)
            {   //czas liczony od puszczenia skoku
                noJumpTime += Time.deltaTime;
                noJumpTime = Mathf.Clamp(noJumpTime, 0.0f, 1.0f);
                noJumpTime2 += Time.deltaTime;
                noJumpTime2 = Mathf.Clamp(noJumpTime2, 0.0f, 1.0f);
            }

            FallColTime += Time.deltaTime;
            FallColTime = Mathf.Clamp(FallColTime, 0.0f, 1.0f);

            ground = isGrounded();  //podstawienie pod zmienna przyciagania
            camRot = keysScript.camRot;
            camRot = camRot - mainTransform.forward;
            if (ground)
            {
                oneSecTime += Time.deltaTime;   //czas liczony od poprzedniego upadku

            }

            if (keysScript.Fire2 && !fly)
            {
                endVector = new Vector3(0.0f, camRot.y); //wektor ruchu dla obrotu w dana strone
            }
            else
            {   //jezeli nie zmuszam do patrzenia to dzialanie normalne
                if(fly)
                    endVector = new Vector3(0.0f, camRot.y); //obrot podczas lotu w miejscu

                if (VerMv != 0.0f)
                {   //wektor obrotu dla ruchu przod/tyl
                    endVector = new Vector3(0.0f, camRot.y + VerMv * 90.0f - 90.0f);
                }
                if (HorMv != 0.0f)
                {   //wektor obrotu dla ruchu prawo/lewo
                    endVector = new Vector3(0, 90 * HorMv, 0) + new Vector3(0.0f, camRot.y);
                }
                if (HorMv != 0.0f && VerMv != 0.0f)
                {   //wektor obrotu dla ruchów ukośnych
                    endVector = (new Vector3(0, 90 * HorMv, 0) + new Vector3(0.0f, camRot.y) + new Vector3(0.0f, camRot.y + VerMv * 90.0f - 90.0f)) / 2.0f;
                    if (HorMv > 0.0f && VerMv < 0.0f)   //dla ukosu prawo_tyl, bo cos nie dziala
                        endVector = new Vector3(0.0f, endVector.y - 180.0f);
                }
                if (camRot.z > 170.0f) //jezeli obrot kamery o 180 stopni w z (czyli kamera przestapila prog 90 stopni x) to postac ma isc w przeciwnym kierunku czyli poprawnym
                   endVector.y -= 180.0f;
            }

            float flyModifier = 1.0f;
            float flyModifier2 = 0.0f;
            if (fly)
            {   //jezeli lata
                RB.useGravity = false;  //wylacze grawitacje
                flyModifier = 1.5f;     //zmienie modyfikator przyspieszenia i predkosci
                if(!staminaScript.useStamina)
                flyModifier2 = -0.5f * mainTransform.forward.y;

                realSpeed = RB.velocity.magnitude;  //obliczanie realspeed
                if (RB.velocity.x + RB.velocity.y + RB.velocity.z == 0.0f)
                    realSpeed = 0.0f;

                endVector.x = camRot.x * VerMv; //jezeli lata to moze obracac sie wraz z kamera

                if (colVelY > 0.0f)
                {   //jezeli latajaca postac uderzy w cos
                    healthPointsscript.damage = (colVelY - 10.0f) * 1.25f;
                    if ((colVelY - 10.0f) * 2.5f > 40.0f)
                        noFlyTime = 3.0f;   //3 sekundy bez latania
                    colVelY = 0.0f;
                }
            }

            if (keysScript.leftAlt || keysScript.Fire2)      //jezeli lewy alt lub PPM do ruchu to chod
                speed = walkSpeed * flyModifier + walkSpeed * flyModifier2;
            else
            {
                if (staminaScript.useStamina)    //jezeli lewy shift do ruchu to bieg
                    speed = runSpeed * flyModifier + runSpeed * flyModifier2;
                else
                    speed = trotSpeed * flyModifier + trotSpeed * flyModifier2;             //jezeli brak dodatkowego przycisku to klus
            }

            float maxSpeed = runSpeed * flyModifier + runSpeed;

            if ((VerMv != 0.0f || HorMv != 0.0f) && (ground || fly) && actualSpeed < speed)
            {
                actualSpeed += speed * Time.deltaTime * 2 + flyModifier2;    //zwiekszanie predkosci aktualnej
                if(fly)
                actualSpeed = Mathf.Clamp(actualSpeed, walkSpeed, maxSpeed);
                else
                actualSpeed = Mathf.Clamp(actualSpeed, 0.0f, speed);
            }
            else
            {
                if (ground || !fly)
                {
                    actualSpeed -= speed * Time.deltaTime * 2 / flyModifier;    //zmniejszanie predkosci aktualnej
                    actualSpeed = Mathf.Clamp(actualSpeed, 0.0f, speed);
                }
                if(fly && (VerMv == 0.0f && HorMv == 0.0f))
                {
                    actualSpeed -= actualSpeed * Time.deltaTime / 2;
                    actualSpeed = Mathf.Clamp(actualSpeed, 0.0f, maxSpeed);
                }
                if(fly && flyModifier2 < 0.0f && actualSpeed > walkSpeed)
                {
                    actualSpeed -= walkSpeed * Time.deltaTime * (-flyModifier2) * 2.0f;
                    actualSpeed = Mathf.Clamp(actualSpeed, 0.0f, maxSpeed);
                }
            }                                                                     //takie zabezpieczenie

            fiveSecTime += Time.deltaTime;
            if (fiveSecTime > 5.0f)
            {   //wprowadzenie info o rasie i plci + zmiana
                fiveSecTime = 0.0f;
                //changeRaceAndGender(addInfo.race, addInfo.gender);
                //race = addInfo.race;
            }

            if (!fly)
            {   //jezeli nie lata
                RB.useGravity = true;
                realSpeed = new Vector3(RB.velocity.x, 0.0f, RB.velocity.z).magnitude;
                if (RB.velocity.x + RB.velocity.z == 0.0f)
                    realSpeed = 0.0f;

                tenSecTime += Time.deltaTime;
                if (tenSecTime > 0.1f)
                {   //wyznaczanie 10 razy na sekunde raycasta i szukanie obrotu podloza 
                    tenSecTime = 0.0f;
                    Physics.Raycast(RB.position, -mainTransform.up, out h, playerCollider.bounds.extents.y + 1.5f);

                    maxAngle = Vector3.Angle(h.normal, Vector3.up);
                    //Debug.Log(maxAngle);
                }

            }

            animations();   //rozpoczecie funkcji dotyczacej animacji wszelakich

            if (Vector3.Distance(spine2.position, mainTransform.position) > 2.5f)   //naprawa animacji po mergeingu (trudne do naprawienia)
            {
                anim.Rebind();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Dead == false)
        {
            VerMv = keysScript.VerMv;   //ktore z tych: przod, tyl
            HorMv = keysScript.HorMv;   //ktore z tych: prawo, lewo

            if (fly)
            {
                if (actualSpeed < 1.5 * walkSpeed && mainTransform.forward.y > 0.5f)  //obrot do pionu przy predkosci ruchu mniejszej niz <----
                {
                    RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(0.0f, RB.rotation.eulerAngles.y, 0.0f), 45.0f * Time.fixedDeltaTime);
                    endVector.x = 0.0f;
                }
            }

            if (!fly)
            {   //jezeli nie lata

                if (colVelY > 10.0f)
                {
                    healthPointsscript.damage = (colVelY - 10.0f) * 8.0f;   //
                    colVelY = 0.0f;                                         //tu jest liczony DMG od upadku
                }

                if (ground)  //jezeli na ziemi      Obrot postaci wzgledem podloza
                {
                    if(maxAngle < 70.0f)
                    RB.MoveRotation(Quaternion.Slerp(RB.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(mainTransform.forward, h.normal), h.normal), Time.deltaTime * 10.0f));
                    RB.drag = 3.0f;
                }
                else
                {   //jezeli w powietrzu to postac ma nachylic sie o 25 stopni do przodu
                    RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(25.0f, RB.rotation.eulerAngles.y, 0.0f), 15.0f * Time.fixedDeltaTime);
                    RB.drag = 0.0f;
                }

                if (actualSpeed > 0.0f && ground && maxAngle < 70.0f && !keysScript.Fire2)   //jezeli predkosc jest mniejsza niz obliczona to na postac ma dzialac sila ciazenia
                {                                                       //i do tego przy braku ruchu oraz zbyt duzym kacie nachylenia maja dzialac inne sily (zeslizgniecie sie)

                    Vector3 vDNorm = mainTransform.forward.normalized;
                    float dot = Vector3.Dot(RB.velocity, mainTransform.forward);
                    Vector3 vP = vDNorm * dot;
                    float vel = actualSpeed - vP.magnitude;
                    vel = Mathf.Clamp(vel, 0.0f, 1000.0f);

                    RB.AddForce((1 + transform.forward.y) * 100.0f * vel * transform.forward);  
                    //Vector3 v3 = mainTransform.forward * actualSpeed;
                    //v3.y = RB.velocity.y;
                    //RB.velocity = v3;
                }

                if (keysScript.Jump && ground && oneSecTime > 1.0f && !fly)
                {   //skok moze byc wykonany tylko z ziemi raz na sekunde
                    RB.AddForce(Vector3.up * 5.5f, ForceMode.Impulse);
                    oneSecTime = 0.0f;
                    noJumpTime = 0.0f;
                }
                else
                {
                    if (keysScript.Jump && !ground && noJumpTime > 0.1f && race == 2 && noFlyTime == 0.0f)
                    {//jezeli postac skoczy w powietrzu 0.1 sek po puszczeniu skoku to moze leciec jezeli jest pegazem i nie uderzyl w sciane
                        fly = true;
                        noJumpTime = 0.0f;
                    }
                }
            }
            else
            {   //jezeli lata
                Vector3 v3 = mainTransform.forward * actualSpeed;
                RB.velocity = v3;

                if (keysScript.Jump && noJumpTime > 0.1f)
                {
                    noJumpTime2 = 0.0f;
                }

                if (keysScript.Jump && noJumpTime2 < 0.1f)
                    fly = false;

                if (VerMv != 0 || HorMv != 0)   //przy locie postac nachyla sie do kierunku ruchu
                {
                    if (endVector.y < 0.0f) //jest to potrzebne, bo inaczej obliczenia sie rypia
                        endVector.y = endVector.y + 360.0f;
                    float sideRot = RB.rotation.eulerAngles.y - endVector.y;

                    sideRot = Mathf.Clamp(sideRot, -realSpeed * 2, realSpeed * 2);  //im szybciej postac leci tym bardziej sie nachyla
                    RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(RB.rotation.eulerAngles.x, RB.rotation.eulerAngles.y, sideRot), 20.0f * Time.fixedDeltaTime);
                }//powrot do pionu
                else RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(0.0f, RB.rotation.eulerAngles.y, 0.0f), 50.0f * Time.fixedDeltaTime);
            }

            if (keysScript.Fire2 && (ground || fly) && actualSpeed <= walkSpeed)
            {   //jezeli chce obracac sie przy pomocy PPM to musze wytracic predkosc i inaczej poruszac postacia (moge chodzic na boki)
                RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(endVector), rotationAngle / 2 * Time.fixedDeltaTime);

                Vector3 datDirection = (mainTransform.forward * VerMv + mainTransform.right * HorMv).normalized;
                Vector3 vDNorm = datDirection;
                float dot = Vector3.Dot(RB.velocity, (datDirection));
                Vector3 vP = vDNorm * dot;
                float vel = actualSpeed - vP.magnitude;
                vel = Mathf.Clamp(vel, 0.0f, 1000.0f);

                RB.AddForce(vel * new Vector3(datDirection.x, Mathf.Clamp(datDirection.y, -1.0f, 0.0f), datDirection.z) * 100.0f);

                //Vector3 v3 = mainTransform.right * actualSpeed * HorMv + mainTransform.forward * actualSpeed * VerMv;
               // v3.y = RB.velocity.y;
                //RB.velocity = v3;
            }
            else
            {   //tu zachowanie normalne
                if ((VerMv != 0.0f || HorMv != 0.0f) && ground)
                {   //obrot postaci wzgledem ukierunkowania ruchu na ziemi
                    RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(endVector.x, endVector.y, RB.rotation.eulerAngles.z), rotationAngle * Time.fixedDeltaTime);
                }
                if ((VerMv != 0.0f || HorMv != 0.0f) && fly)
                {   //w locie
                    RB.rotation = Quaternion.RotateTowards(RB.rotation, Quaternion.Euler(endVector.x, endVector.y, RB.rotation.eulerAngles.z), rotationAngle / 1.5f * Time.fixedDeltaTime);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (Dead == false)
        {

            float locCamRotHor = RB.rotation.eulerAngles.y - keysScript.shootRot.y;    //info o obrocie kamery wzgledem postaci Y
            if (camRot.z > 170.0f)  //obrot glowy przy przejsciu kamery przez prog 90 stopni tez trzeba zmienic, bo sie bugowalo jak jasna cholera
                locCamRotHor -= 180.0f;
            if (locCamRotHor < 0.0f) locCamRotHor += 360.0f;
            locCamRotHor -= 180.0f;

            float locCamRotVer = RB.rotation.eulerAngles.x - keysScript.shootRot.x;    //info o obrocie kamery wzgledem postaci X
            if (locCamRotVer < 0.0f) locCamRotVer += 360.0f;
            locCamRotVer -= 180.0f;

            if ((HorMv != 0 || VerMv != 0) && ground && !keysScript.Fire2 && Mathf.Round(endVector.y) != Mathf.Round(RB.rotation.eulerAngles.y) && Mathf.Abs(RB.rotation.eulerAngles.y - lastRotation.y) > 0.0f)
            {   //jezeli postac sie porusza z wlasnej woli, jest uziemiona, ale nie celuje lub biegnie prosto
                if ((RB.rotation.eulerAngles.y - lastRotation.y < 1.0f && spine2rot > -20.0f))
                {   //jezeli kamera jest po prawej to postac obraca sie w lewo
                    spine2rot -= 110.0f * Time.deltaTime;
                }
                if ((RB.rotation.eulerAngles.y - lastRotation.y > -1.0f && spine2rot < 20.0f))
                {   //jezeli kamera jest po lewej to postac obraca sie w prawo
                    spine2rot += 110.0f * Time.deltaTime;
                }
            }
            else
            {   //powrot pozycji postaci przy postoju lub biegu prostym
                if (spine2rot > 0.0f)
                {
                    spine2rot -= 110.0f * Time.deltaTime;
                }
                if (spine2rot < 0.0f)
                {
                    spine2rot += 110.0f * Time.deltaTime;
                }
                if (Mathf.Abs(spine2rot) <= 2.0f && (HorMv != 0 || VerMv != 0))
                {
                    spine2rot = 0.0f;
                }
            }

            spine2.transform.eulerAngles = spine2.transform.eulerAngles + new Vector3(0.0f, 0.0f, spine2rot);   // obrot spine2 o max 20 stopni wzgledem wlasnego Z
            lastRotation = RB.rotation.eulerAngles;       //ostatnia rotacja w pionie
                                                                 //=====================================================================================================================================================

            //tu zobacze czy zmienic        if (true)
            //           {   //obrot glowy w poziomie  35 stopni w obie strony
            if (locCamRotHor > 0.0f && 180.0f - headRot >= 130.0f && 180.0f - headRot >= locCamRotHor + 4.0f)  //w prawo - to 4.0f sprawia, ze sie nie trzesie
                headRot += 220.0f * Time.deltaTime;
            else if (locCamRotHor > 0.0f && 180.0f - headRot < locCamRotHor && 180.0f - headRot > 0.0f) //w lewo od prawej
                headRot -= 220.0f * Time.deltaTime;
            if (locCamRotHor < 0.0f && 180.0f + headRot >= 130.0f && -(180.0f + headRot) <= locCamRotHor - 4.0f)  //w lewo - to 4.0f sprawia, ze sie nie trzesie
                headRot -= 220.0f * Time.deltaTime;
            else if (locCamRotHor < 0.0f && -(180.0f + headRot) > locCamRotHor && 180.0f + headRot > 0.0f) //w prawo od lewej
                headRot += 220.0f * Time.deltaTime;
            //==========
            //obrot glowy w pionie  35 stopni do przodu i 20 stopni do tylu
            if (locCamRotVer > 0.0f && 180.0f - headRotVer >= 145.0f && 180.0f - headRotVer >= locCamRotVer + 4.0f)  //w dol - to 4.0f sprawia, ze sie nie trzesie
                headRotVer += 110.0f * Time.deltaTime;
            else if (locCamRotVer > 0.0f && 180.0f - headRotVer < locCamRotVer && 180.0f - headRotVer > 0.0f) //w gore od dolu
                headRotVer -= 110.0f * Time.deltaTime;
            if (locCamRotVer < 0.0f && 180.0f + headRotVer >= 160.0f && -(180.0f + headRotVer) <= locCamRotVer - 4.0f)  //w gore - to 4.0f sprawia, ze sie nie trzesie
                headRotVer -= 110.0f * Time.deltaTime;
            else if (locCamRotVer < 0.0f && -(180.0f + headRotVer) > locCamRotVer && 180.0f + headRotVer > 0.0f) //w dol od gory
                headRotVer += 110.0f * Time.deltaTime;
            //            }
            //            else
            //           {   //powrot glowy w poziomie
            //               if (headRot > 1.0f)
            //                    headRot -= 110.0f * Time.deltaTime;
            //                if (headRot < -1.0f)
            //                   headRot += 110.0f * Time.deltaTime;
            //==========
            //powrot glowy w pionie
            //                if (headRotVer > 1.0f)
            //                   headRotVer -= 110.0f * Time.deltaTime;
            //               if (headRotVer < -1.0f)
            //                    headRotVer += 110.0f * Time.deltaTime;
            //            }
            if (fly && realSpeed > walkSpeed * 1.7f)
            {   //przy locie glowa troszku inaczej
                head.transform.eulerAngles = head.transform.eulerAngles + new Vector3(headRotVer + 10.0f, headRot);     //obrot glowy o caly kat i pochylenie jej
            }
            else
            {
                head.transform.eulerAngles = head.transform.eulerAngles + new Vector3(headRotVer / 2, headRot / 2);   //obrot glowy o pół kata
                neck.transform.eulerAngles = neck.transform.eulerAngles + new Vector3(headRotVer / 2, headRot / 2);   //obrot szyji o pół kata
            }
            //=========================================================================================================================================

            float eyeLrotHor = 0.0f;
            float eyeRrotHor = 0.0f;
               //obrot oczu
                if (Mathf.Abs(locCamRotHor) < 130.0f)
                {
                    if (locCamRotHor > 0.0f && headRot > 0.0f)
                    {   //w prawo
                        eyeLrotHor = -locCamRotHor + 130.0f;
                        eyeRrotHor = eyeLrotHor;
                        if (eyeLrotHor > 10.0f)
                            eyeLrotHor = 10.0f;
                        if (eyeRrotHor > 50.0f)
                            eyeRrotHor = 50.0f;
                    }
                    if (locCamRotHor < 0.0f && headRot < 0.0f)
                    {   //w lewo
                        eyeLrotHor = -locCamRotHor - 130.0f;
                        eyeRrotHor = eyeLrotHor;
                        if (eyeLrotHor < -50.0f)
                            eyeLrotHor = -50.0f;
                        if (eyeRrotHor < -10.0f)
                            eyeRrotHor = -10.0f;
                    }
                }
                eyeBoneL.transform.eulerAngles = eyeBoneL.transform.eulerAngles + new Vector3(0.0f, 0.0f, eyeLrotHor); //obrot lewego oka
                eyeBoneR.transform.eulerAngles = eyeBoneR.transform.eulerAngles + new Vector3(0.0f, 0.0f, eyeRrotHor); //obrot prawego oka
            }
        }
    }
