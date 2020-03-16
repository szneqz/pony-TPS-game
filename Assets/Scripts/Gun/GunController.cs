using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{

    private Rigidbody gunRigidbody;
    private Player2 playerScript;
    private BulletSpawn bulletScript;
    private ContrMovem contrScript;
    private GameObject player;
    private Transform lookAt;    //pozycja playera
    private Transform gunTransform;  //pozycja broni
    private BoxCollider col; //kolizja
    public GameObject particles;

    public bool ifScope = false;    //jezeli jest przycelowanie to info idzie do skryptu o sniperce
    public bool ifScopeBot = false;
    private bool isBoolTrue = false;

    private Transform mainCamera;
    private Camera mainCameraCam;

    private float fieldOfView;

    private float distance = 0.6f; //srednia odleglosc od postaci
    private float height = 1.0f;   //wysokosc broni nad ziemia

    public float sensivityX = 4.0f; //czulosc w poziomie
    public float sensivityY = 2.0f; //czulosc w pionie

    private float actDistance = 0.0f;

    private bool camCollision = false;  //czy jest kolizja broni z czyms
    private float camSpeed1 = 0.0f;

    //private Vector3 particlesPos;

    private int layerMask = ~(1 << 8 | 1 << 2);  //rayCast bedzie obijal sie o wszystko procz dwoch z layerow (layer 2 i layer 8)

    private void Start()
    {
        col = transform.GetComponent<BoxCollider>();
        gunRigidbody = transform.GetComponent<Rigidbody>();
        gunTransform = transform;
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<Player2>();
        contrScript = player.GetComponent<ContrMovem>();
        bulletScript = transform.GetComponent<BulletSpawn>();
        lookAt = player.transform;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, col); //ignoruj kolizje z playerem
        Physics.IgnoreCollision(player.GetComponent<Collider>(), col);
        Physics.IgnoreCollision(Camera.main.GetComponent<Collider>(), col);

        mainCamera = Camera.main.transform;
        mainCameraCam = Camera.main;
        fieldOfView = mainCameraCam.fieldOfView;

       // particlesPos = particles.transform.localPosition;
    }

    void OnTriggerEnter(Collider target)
    {
        camCollision = true;   //wejscie w obiekt oznacza kolizję
    }

    void OnTriggerStay(Collider target)
    {
        camCollision = true;   //pozostanie w obiekcie 
    }

    void OnTriggerExit(Collider target)
    {
        camCollision = false;  //opuszczenie obiektu 
    }

    private void Update()
    {
        if (playerScript.Dead == false)
        {
            particles.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        if (playerScript.Dead == false)
        {
            if (bulletScript && bulletScript.ifSniper && contrScript.Fire2 && Vector3.Angle(new Vector3 (lookAt.forward.x, 0.0f, lookAt.forward.z), new Vector3((Quaternion.Euler(contrScript.camRot) * Vector3.forward).x, 0.0f, (Quaternion.Euler(contrScript.camRot) * Vector3.forward).z)) < 30.0f)
                {   //taki skrypt jest jeszcze w sniperspawn.cs - dotyczy on przyblizenia kamery przy strzale ze sniperki
                isBoolTrue = false;
                transform.rotation = (Quaternion.Euler(player.transform.rotation.eulerAngles.x + Mathf.Clamp(Vector3.Angle(lookAt.forward, new Vector3(lookAt.forward.x, (Quaternion.Euler(contrScript.camRot) * Vector3.forward).y, lookAt.forward.z)) * Mathf.Sign(lookAt.forward.y - (Quaternion.Euler(contrScript.camRot) * Vector3.forward).y), -35.0f, 35.0f), Quaternion.Euler(contrScript.camRot).eulerAngles.y, Quaternion.Euler(contrScript.camRot).eulerAngles.z/*mainCamera.rotation.eulerAngles.y, mainCamera.rotation.eulerAngles.z*/));
                if (playerScript.headRotVer > -20.0f && playerScript.headRotVer < 34.0f)
                    {
                    transform.position = lookAt.position + 0.2f * lookAt.forward + 0.8f * lookAt.up + gunTransform.forward * 0.4f + gunTransform.up * 0.15f + gunTransform.right * 0.15f;   //ruch kamery po orbicie
                    }
                    else
                    {
                    if (!contrScript.ifBot && bulletScript.SniperShoot && playerScript.isLocalPlayer)
                    {
                        ifScope = true;
                    }
                    }
                if (!contrScript.ifBot && playerScript.isLocalPlayer)  //jezeli nie bot to sterujemy kamera
                {
                    if (bulletScript.SniperShoot)
                    {
                        mainCamera.position = gunRigidbody.position + gunTransform.forward * 0.5f;
                        mainCamera.rotation = gunRigidbody.rotation;
                        mainCameraCam.fieldOfView = fieldOfView * 0.5f;
                        ifScope = true;
                    }
                    else ifScope = false;
                }
                else
                    ifScopeBot = true;
                }
            else
            {
                isBoolTrue = true;
                ifScope = false;
                ifScopeBot = false;
               //Vector3 dir = new Vector3(actDistance, 0.0f, 0.0f); //ustawienie broni przy podlodze

                float rotationx = contrScript.shootRot.x;
                if (rotationx < 180.0f && rotationx > 70.0f)
                    rotationx = 70.0f;
                if (rotationx >= 180.0f && rotationx < 300.0f)
                    rotationx = 300.0f;
                //Quaternion rotation = Quaternion.Euler(rotationx, contrScript.shootRot.y, 0.0f);
                //transform.position = (lookAt.position + lookAt.forward * (actDistance * 0.8f)) + rotation * dir + lookAt.up * height;   //ruch broni po orbicie
                //gunTransform.position += lookAt.up * height;    //ustawienie broni na odpowiedniej wysokosci
                //transform.rotation = rotation;                //<-------------------------------------TO BYŁ BŁĄD JEDNOCZESNIE W LATEUPDATE I FIXEDUPDATE SOBIE PRZYPISAŁO
            }

            //particles.transform.rotation = gunTransform.rotation;
            particles.transform.localPosition = Vector3.zero;//gunTransform.position + particlesPos;
        }
    }

    private void FixedUpdate()
    {
        if (playerScript.Dead == false) //jezeli postac zyje
        {
            //gunRigidbody.isKinematic = true;
            gunRigidbody.useGravity = false;
            //gunRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            col.isTrigger = true;

            if (((Physics.Raycast(transform.position + (-transform.right) * actDistance, transform.right, actDistance + 0.25f, layerMask))) || actDistance > distance)
            {   //jezeli raycast odwrocony w strone broni natrafi na przeszkode to kamera ma jechac w strone postaci
                actDistance -= distance * Time.fixedDeltaTime * camSpeed1;
                camSpeed1 += Time.fixedDeltaTime * 20.0f;
                if (camSpeed1 > distance) camSpeed1 = distance;
            }
            else
            {   //jezeli nie natrafi na przeszkode i jednoczesnie bron nie jest w zadnym obiekcie to bron jedzie do tylu z dala od postaci
                if (camCollision == false)
                {
                    actDistance += distance * Time.fixedDeltaTime * 2.0f;
                    camSpeed1 = 0.0f;
                }

            }   //jezeli bron pojedzie za daleko to ma sie ogarnac i wrocic na maksymalna odleglosc
            if (actDistance > distance)
                actDistance = distance;

            if (isBoolTrue)
            {
                Vector3 dir = new Vector3(actDistance, 0.0f, 0.0f); //ustawienie broni przy podlodze

                float rotationx = contrScript.shootRot.x;
                if (rotationx < 180.0f && rotationx > 70.0f)
                    rotationx = 70.0f;
                if (rotationx >= 180.0f && rotationx < 300.0f)
                    rotationx = 300.0f;
                Quaternion rotation = Quaternion.Euler(rotationx, contrScript.shootRot.y, 0.0f);
                transform.position = (lookAt.position + lookAt.forward * (actDistance * 0.8f)) + rotation * dir + lookAt.up * height;   //ruch broni po orbicie
                                                                                                                                        //gunTransform.position += lookAt.up * height;    //ustawienie broni na odpowiedniej wysokosci
                transform.rotation = rotation;
            }

        }
        else        //jezeli postac nie zyje to bron ma uruchomic swoje umiejetnosci dzialania wedle praw grawitacji
        {
            //gunRigidbody.isKinematic = false;
            gunRigidbody.useGravity = true;
            //gunRigidbody.constraints = 0;
            col.isTrigger = false;
            particles.SetActive(false);
        }
    }
}
