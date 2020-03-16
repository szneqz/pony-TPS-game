using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private const float Y_ANGLE_MIN = -30.0f;   //maksymalne odchylenie w gore
    private const float Y_ANGLE_MAX = 35.0f;    //maksymalne odchylenie w dol

    public GameObject player;
    private Transform lookAt;    //pozycja playera
    private Transform camTransform; //pozycja kamery
    private SphereCollider col; //kolizja

    private float distance = 2.5f; //srednia odleglosc od postaci
    private float height = 1.8f;   //wysokosc kamery nad ziemia
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public float sensivityX = 4.0f; //czulosc w poziomie
    public float sensivityY = 2.0f; //czulosc w pionie

    private float actDistance = 5.0f;

    private bool camCollision = false;  //czy jest kolizja kamery z czyms
    private float camSpeed1 = 0.0f;

    public GameObject datCanvas;
    public InputField IField;

    private int layerMask = ~(1 << 8 | 1 << 2);  //rayCast bedzie obijal sie o wszystko procz dwoch z layerow (layer 2 i layer 8)

    private void Start()
    {
        camTransform = transform;
        //player = GameObject.Find("pony3_lowPoly");
        lookAt = player.transform;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, GetComponent<Collider>()); //ignoruj kolizje z playerem
       // datCanvas = GameObject.Find("GameController/Canvas").gameObject;
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag != "Player")
            camCollision = true;   //wejscie w obiekt oznacza kolizję
        if(target.tag == "Bullet")
            Physics.IgnoreCollision(target.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void OnTriggerStay(Collider target)
    {
        if (target.tag != "Player")
            camCollision = true;   //pozostanie w obiekcie 
    }

    void OnTriggerExit(Collider target)
    {
        camCollision = false;  //opuszczenie obiektu 
    }

    private void Update()
    {
            if (lookAt == null)
            {   //jezeli nie moze znalesc playera(bo go jeszcze nie ma)
                //player = GameObject.Find("pony3_lowPoly");
                lookAt = player.transform;
            }

            float fakeDif = camTransform.rotation.eulerAngles.y - lookAt.rotation.eulerAngles.y;
            if (fakeDif > 180.0f)   //obrot postaci wzgledem kamery
                fakeDif -= 360.0f;
            if (fakeDif < -180.0f)
                fakeDif += 360.0f;
            fakeDif = Mathf.Abs(fakeDif);   //tylko dodatnie

            float fakeRotatinx = lookAt.rotation.eulerAngles.x;
            if (fakeRotatinx > 180.0f)  //bo ja chce od -180 do 180, a to podaje od 0 do 360
                fakeRotatinx -= 360.0f;

            float min;
            float max;

            if (camTransform.rotation.eulerAngles.z > 170.0f)
            {   //sa tu dwie mozliwosci, po progu 90 stopni w kamerze jest jedna mozliwosc...
                min = (Y_ANGLE_MIN + fakeRotatinx / 2) - (2 * (fakeRotatinx / 2) * Mathf.Abs(fakeDif - 180.0f) / 180.0f);
                max = (Y_ANGLE_MAX + fakeRotatinx / 2) - (2 * (fakeRotatinx / 2) * Mathf.Abs(fakeDif - 180.0f) / 180.0f);
            }
            else
            {   //...a tu gdy jest przed progiem
                min = (Y_ANGLE_MIN + fakeRotatinx / 2) - (2 * (fakeRotatinx / 2) * fakeDif / 180.0f);
                max = (Y_ANGLE_MAX + fakeRotatinx / 2) - (2 * (fakeRotatinx / 2) * fakeDif / 180.0f);
            }

        if (!datCanvas.activeInHierarchy && !IField.isFocused)  //jezeli jest aktywna pauza lub chat
        {                                                       //to nie ma ruszania mysza
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");
        }

            if (currentY * sensivityY < -180)
                currentY = currentY + 360.0f / sensivityY;
            if (currentY * sensivityY > 180)
                currentY = currentY - 360.0f / sensivityY;

            currentY = Mathf.Clamp(currentY, min, max);     //Y musi byc zamkniety miedzy min, a max

            Vector3 dir = new Vector3(0, 0, -actDistance); //ustawienie kamery przy podlodze
            Quaternion rotation = Quaternion.Euler(currentY * sensivityY, currentX * sensivityX, 0);    //ruch kamery razem z playerem
            camTransform.position = lookAt.position + rotation * dir;   //ruch kamery po orbicie
            camTransform.LookAt(lookAt.position);   //obracanie kamery w strone playera
            camTransform.position += lookAt.up * height;    //ustawienie kamery na odpowiedniej wysokosci

            if ((camTransform.rotation.eulerAngles.x < 90 && (currentY * sensivityY) > 90.0f) || (camTransform.rotation.eulerAngles.x > 270 && (currentY * sensivityY) < -90.0f))
            {   //obrot kamery w Z o 180 stopni, gdy przekroczony jest ten glupi prog 90 stopni w X
                camTransform.Rotate(0.0f, 0.0f, 180.0f);
            }
    }

 //   private void LateUpdate()
//    {
   //         Vector3 dir = new Vector3(0, 0, -actDistance); //ustawienie kamery przy podlodze
   //         Quaternion rotation = Quaternion.Euler(currentY * sensivityY, currentX * sensivityX, 0);    //ruch kamery razem z playerem
   //         camTransform.position = lookAt.position + rotation * dir;   //ruch kamery po orbicie
   //         camTransform.LookAt(lookAt.position);   //obracanie kamery w strone playera
  //          camTransform.position += lookAt.up * height;    //ustawienie kamery na odpowiedniej wysokosci
//
  //          if ((camTransform.rotation.eulerAngles.x < 90 && (currentY * sensivityY) > 90.0f) || (camTransform.rotation.eulerAngles.x > 270 && (currentY * sensivityY) < -90.0f))
  //          {   //obrot kamery w Z o 180 stopni, gdy przekroczony jest ten glupi prog 90 stopni w X
  //              camTransform.Rotate(0.0f, 0.0f, 180.0f);
  //          }
 //   }

    private void FixedUpdate()
    {
            if (((Physics.Raycast(transform.position + transform.forward * actDistance, -transform.forward, actDistance + 0.15f, layerMask))) && actDistance > 1.3f)
            {   //jezeli raycast odwrocony w strone kamery natrafi na przeszkode to kamera ma jechac w strone postaci
                actDistance -= distance * Time.fixedDeltaTime * camSpeed1;
                camSpeed1 += Time.fixedDeltaTime * 10.0f;
                if (camSpeed1 > 5.0f) camSpeed1 = 5.0f;
            }
            else
            {   //jezeli nie natrafi na przeszkode i jednoczesnie kamera nie jest w zadnym obiekcie to kamera jedzie do tylu z dala od postaci
                if (camCollision == false)
                {
                    actDistance += distance * Time.fixedDeltaTime * 2.0f;
                    camSpeed1 = 0.0f;
                }

            }   //jezeli kamera pojedzie za daleko to ma sie ogarnac i wrocic na maksymalna odleglosc
            if (actDistance > distance)
                actDistance = distance;
    }
}
