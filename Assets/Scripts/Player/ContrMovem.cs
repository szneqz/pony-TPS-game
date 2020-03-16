using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ContrMovem : NetworkBehaviour {

    public bool ifBot;
    //[SyncVar]
    public bool Fire1;
    //[SyncVar]
    public bool Fire2;
    //[SyncVar]
    public bool Jump;
    //[SyncVar]
    public bool leftAlt;
    //[SyncVar]
    public bool leftShift;
    //[SyncVar]
    public float VerMv;
    //[SyncVar]
    public float HorMv;
    //[SyncVar]
    public int num = 10;    //liczba 10 oznacza nie wciskanie żadnej cyfry na klawiaturze
    //[SyncVar]
    public Vector3 camRot;
    public bool isChangedShootRot = false;
    //[SyncVar]
    public Vector3 shootRot;    //zmienna, ktora zajmuje sie podawanie botowi miejsca gdzie ma strzelac, a nie gdzie sie ruszac
    //[SyncVar]
    public bool reload;
    //[SyncVar]
    public float currentX;
    //[SyncVar]
    public float currentY;
    //[SyncVar]
    public float scroll;
    private KeyCode[] keyCodes = {
         KeyCode.Alpha0,
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    //private MoveTo MT;
    //private Vector3 corner;
    //public GameObject NMA;  //NavMeshAgent
    //private float time = 0.0f;
    private Transform cameras;
    private GameObject datCanvas;
    private InputField IField;


    private void Start()
    {

        //if (ifBot && false)
        //{
        //    NMA = Instantiate(NMA);
        //    NMA.transform.parent = transform;
        //    MT = NMA.GetComponent<MoveTo>();
        //}

        datCanvas = GameObject.Find("GameController/Canvas").gameObject;
        IField = GameObject.Find("GameController/ChatCanvas/InputField").GetComponent<InputField>();
        cameras = Camera.main.transform;

        if (isLocalPlayer)
        {
            cameras.GetComponent<CameraController>().player = this.gameObject;
        }
    }

    //void Update()
    //{
    //if (ifBot)
    //{
    //    if (false)
    //    {
    //        corner = MT.corner; //dodaje miejsce, do ktorego sie udac, gdy jest botem
    //        camRot = (corner - transform.position).normalized;  //wektor do miejsca gdzie postac ma sie dostac
    //        camRot = Quaternion.LookRotation(camRot).eulerAngles;   //obrot pseudo-kamery
    //        if (Vector3.Distance(MT.goal.position, transform.position) > 2.0f)
    //        {   //jezeli odleglosc od celuy wynosi wiecej niz dwie jednostki to postac ma tam dazyc
    //            if (MT.notMatch)
    //            {   //jezeli postac znajduje sie na terenie, po ktorym ma biegac to ma zachowywac sie normalnie
    //                time = 0.0f;
    //                VerMv = 1.0f;   //ruch do przodu
    //                                //ponizej - kat miedzy postacia a jej ostatecznym ruchem
    //                float rotAngle = Vector3.Angle(new Vector3(transform.forward.x, 0.0f, transform.forward.z), new Vector3((corner - transform.position).normalized.x, 0.0f, (corner - transform.position).normalized.z));
    //                if (rotAngle > 35.0f || Vector3.Distance(MT.goal.position, transform.position) < 5.0f)   //jezeli kat miedzy postacia, a wektorem do celu wynosi wiecej niz kat podany to postac ma isc lub odleglosc jest dostatecznie mala
    //                    leftAlt = true;
    //                else leftAlt = false;
    //                if (rotAngle > 120.0f)  //jezeli kat miedzy postacia, a wektorem do celu wynosi wiecej niz kat podany to postac ma sie obracac w miejscu
    //                {
    //                    VerMv = 0.0f;
    //                    Fire2 = true;
    //                }
    //                else Fire2 = false;
    //                if (rotAngle < 10.0f && Vector3.Distance(MT.goal.position, transform.position) > 8.0f)
    //                {   //jezeli kat jest mniejszy niz podany i postac jest w odleglosci wiekszej niz 5 jednostek od celu to ma biegnac sprintem
    //                    leftShift = true;
    //                }
    //                else leftShift = false;
    //            }
    //            else
    //            {   //jezeli postac jest poza terenem ruchu to ma teleportowac swoj bloczek do szukania drogi
    //                leftAlt = false;
    //                Fire2 = false;
    //                MT.agent.Warp(transform.position);
    //                MT.agent.destination = MT.goal.position;
    //                time += Time.deltaTime;
    //                VerMv = 1.0f;
    //                if (time > 5.0f)
    //                {   //po 5 sekundach AI idze w dol gorki
    //                    camRot = transform.up;  //pozwala to na wychwycenie gdzie jest dol gorki (jest to nachylone w ta strone)
    //                    camRot = Quaternion.LookRotation(camRot).eulerAngles - new Vector3(90.0f, 0.0f);    //jako, ze to patrzy w gore to 90 stopni w dol
    //                }
    //                if (time > 30.0f)    //jezeli przez pol minuty postac nie moze sie wydostac to ma byc teleportowana do miejsca navmeshAgenta
    //                {
    //                    transform.position = MT.agent.path.corners[0];
    //                    transform.LookAt(corner); //postac po tej teleportacji patrzy na najblizszy cel ruchu
    //                    transform.position += transform.forward;    //przesuniecie postaci o jednostke do przodu
    //                    VerMv = 1.0f;
    //                    leftShift = false;
    //                }
    //            }
    //        }
    //        else
    //            VerMv = 0.0f;   //jezeli postac jest w odleglosci mniejszej niz 2 jednostki to ma sie zatrzymac

    //        shootRot = camRot;  //<--- to bedzie do zmiany, bot ma patrzec i strzelac gdzie shootRot, a isc gdzie camRot
    //    }
    //}

    [ClientRpc]
    void RpcVars(bool a, bool b, bool c, bool d, bool e, float f, float g, Vector3 h, Vector3 i, float j, int k, bool l, float m, float o, Vector3 transPos, Quaternion transRot)
    {
        if (isLocalPlayer)
            return; //to moj obiekt - mam dane lokalne to po co mam aktualizować sam siebie swoimi danymi

        Fire1 = a;
        Fire2 = b;
        Jump = c;
        leftAlt = d;
        leftShift = e;
        VerMv = f;
        HorMv = g;
        camRot = h;
        shootRot = i;
        scroll = j;
        num = k;
        reload = l;
        currentX = m;
        currentY = o;
        transform.position = transPos;
        transform.rotation = transRot;
    }

    [Command]
    void CmdVars(bool a, bool b, bool c, bool d, bool e, float f, float g, Vector3 h, Vector3 i, float j, int k, bool l, float m, float o, Vector3 transformPos, Quaternion transformRot)
    {
        //Fire1 = a;
        //Fire2 = b;
        //Jump = c;
        //leftAlt = d;
        //leftShift = e;
        //VerMv = f;
        //HorMv = g;
        //camRot = h;
        //shootRot = i;
        //scroll = j;
        //num = k;
        //reload = l;
        //currentX = m;
        //currentY = o;

        RpcVars(a, b, c, d, e, f, g, h, i, j, k, l, m, o, transformPos, transformRot);
    }

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }

        if (datCanvas.activeInHierarchy == false && IField.isFocused == false)
        {
            //*** DZIALANIA LOKALNE ***

            Fire1 = Input.GetButton("Fire1");
            Fire2 = Input.GetButton("Fire2");
            Jump = Input.GetButton("Jump");
            leftAlt = Input.GetKey(KeyCode.LeftAlt);
            leftShift = Input.GetKey(KeyCode.LeftShift);
            VerMv = Input.GetAxisRaw("Vertical");
            HorMv = Input.GetAxisRaw("Horizontal");
            camRot = cameras.rotation.eulerAngles;
            if(!isChangedShootRot)
            shootRot = camRot;
            scroll = Input.GetAxis("Mouse ScrollWheel");
            for (int n = 0; n < 11; n++)
            {
                if (n < 10)
                {
                    if (Input.GetKey(keyCodes[n]))
                    {
                        num = n;
                        break;
                    }
                }
                if (n == 10)    //cyfra 10 jako niewciskanie zadnej liczby
                    num = n;
            }
            reload = Input.GetKey(KeyCode.R);
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");

            isChangedShootRot = false;
        }

    //*** DZIALANIA NA SERWERZE ***

    CmdVars(Fire1,
    Fire2,
    Jump,
    leftAlt,
    leftShift,
    VerMv,
    HorMv,
    camRot,
    shootRot,
    scroll,
    num,
    reload,
    currentX,
    currentY,
    transform.position,
    transform.rotation
    );

    }
}
