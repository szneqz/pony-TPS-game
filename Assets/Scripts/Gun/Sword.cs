using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    private Rigidbody gunRigidbody;
    private Player2 playerScript;
    private ContrMovem contrScript;
    private Blade bladeScript;
    private GameObject player;
    private Transform lookAt;    //pozycja playera
    private BoxCollider col; //kolizja
    public GameObject particles;
    private GameObject blade;

    private float distance = 0.6f; //srednia odleglosc od postaci
    private float height = 1.0f;   //wysokosc broni nad ziemia

    public float sensivityX = 4.0f; //czulosc w poziomie
    public float sensivityY = 2.0f; //czulosc w pionie

    private float actDistance = 0.0f;

    private bool camCollision = false;  //czy jest kolizja broni z czyms
    private float camSpeed1 = 0.0f;

    private Quaternion animRot;
    private Quaternion normalRot;
    private Vector3 animPos;
    private bool ifAnim = false;
    private float animTime = 0.0f;
    public float maxAnimTime = 0.3f;

    public Color particleColor;

    private int layerMask = ~(1 << 8 | 1 << 2);  //rayCast bedzie obijal sie o wszystko procz dwoch z layerow (layer 2 i layer 8)

    private AudioSource source;
    public AudioClip swing;
    private bool swinged = false;

    private void Start()
    {
        blade = transform.Find("bladeCollider").gameObject;
        blade.SetActive(false);

        bladeScript = blade.GetComponent<Blade>();

        col = transform.GetComponent<BoxCollider>();
        gunRigidbody = transform.GetComponent<Rigidbody>();
        player = transform.parent.gameObject;
        playerScript = player.GetComponent<Player2>();
        contrScript = player.GetComponent<ContrMovem>();
        lookAt = player.transform;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, col); //ignoruj kolizje z playerem
        Physics.IgnoreCollision(player.GetComponent<Collider>(), col);
        Physics.IgnoreCollision(Camera.main.GetComponent<Collider>(), col);

        particleColor = transform.parent.Find("CombinedMeshes").GetComponent<Merge>().magicColor;
        ParticleSystem gunParticle = transform.Find("Particles").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em2 = gunParticle.emission;
        ParticleSystem.MainModule mainy2 = gunParticle.main;
        mainy2.startColor = new ParticleSystem.MinMaxGradient(particleColor);

        normalRot = Quaternion.Euler(-85.0f, 0.0f, -20.0f);
        animRot = normalRot;
        animPos = Vector3.zero;

        source = GetComponent<AudioSource>();
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
            if (contrScript.Fire1 && ifAnim == false)    //jezeli LPM, ale jednoczesnie nie uderza
            {
                blade.SetActive(true);
                ifAnim = true;
            }

            if (ifAnim == true)
            {
                if (animTime < maxAnimTime)
                {
                    animTime += Time.deltaTime * 2.0f;
                    animRot = normalRot * Quaternion.Euler(180.0f * animTime / maxAnimTime, 0.0f, 90.0f * animTime / maxAnimTime);
                    animPos = new Vector3(-0.5f * actDistance * animTime / maxAnimTime, -0.1f * animTime / maxAnimTime, 1.5f * animTime / maxAnimTime);
                    if (swinged && source && swing && animTime >= (maxAnimTime - swing.length))
                    {
                        source.pitch = Random.Range(0.95f, 1.05f);
                        source.volume = Random.Range(0.9f, 1.0f) * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
                        source.PlayOneShot(swing);
                        swinged = false;
                    }
                }
                else
                {
                    animTime += Time.deltaTime / 1.5f;
                    animRot = normalRot * Quaternion.Euler(180.0f * animTime / maxAnimTime, 0.0f, -90.0f * animTime / maxAnimTime);
                    animPos = new Vector3(-0.5f * actDistance + 0.5f * actDistance * (animTime - maxAnimTime) / maxAnimTime, -0.1f + 0.1f * (animTime - maxAnimTime) / maxAnimTime, 1.5f - 1.5f * (animTime - maxAnimTime) / maxAnimTime);
                }
                if (animTime > 2 * maxAnimTime)
                {
                    ifAnim = false;
                    animRot = normalRot;
                    animPos = Vector3.zero;
                    animTime = 0.0f;
                    bladeScript.Destroy();
                    swinged = true;
                }
            }
        }
        else
        {
            animTime = 0.0f;
            ifAnim = false;
            bladeScript.Destroy();
        }
    }

    //private void LateUpdate()
    //{
    //    if (playerScript.Dead == false)
    //    {
    //        Vector3 dir = new Vector3(actDistance, 0.0f, 0.0f); //ustawienie broni przy podlodze

    //        float rotationx = contrScript.shootRot.x;
    //        if (rotationx < 180.0f && rotationx > 70.0f)
    //            rotationx = 70.0f;
    //        if (rotationx >= 180.0f && rotationx < 300.0f)
    //            rotationx = 300.0f;
    //        Quaternion rotation = Quaternion.Euler(rotationx, contrScript.shootRot.y, 0.0f);
    //        gunRigidbody.MovePosition((lookAt.position + lookAt.forward * (actDistance * 0.8f)) + rotation * (dir + animPos) + lookAt.up * height);   //ruch broni po orbicie
    //        // gunTransform.position += lookAt.up * height;    //ustawienie broni na odpowiedniej wysokosci
    //        gunRigidbody.MoveRotation(rotation * animRot);
    //    }
    //}

    private void FixedUpdate()
    {
        if (playerScript.Dead == false) //jezeli postac zyje
        {
           // gunRigidbody.isKinematic = true;
            gunRigidbody.useGravity = false;
           // gunRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
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

            Vector3 dir = new Vector3(actDistance, 0.0f, 0.0f); //ustawienie broni przy podlodze

            float rotationx = contrScript.shootRot.x;
            if (rotationx < 180.0f && rotationx > 70.0f)
                rotationx = 70.0f;
            if (rotationx >= 180.0f && rotationx < 300.0f)
                rotationx = 300.0f;
            Quaternion rotation = Quaternion.Euler(rotationx, contrScript.shootRot.y, 0.0f);
            gunRigidbody.MovePosition((lookAt.position + lookAt.forward * (actDistance * 0.8f)) + rotation * (dir + animPos) + lookAt.up * height);   //ruch broni po orbicie
            // gunTransform.position += lookAt.up * height;    //ustawienie broni na odpowiedniej wysokosci
            gunRigidbody.MoveRotation(rotation * animRot);
        }
        else        //jezeli postac nie zyje to bron ma uruchomic swoje umiejetnosci dzialania wedle praw grawitacji
        {
            gunRigidbody.isKinematic = false;
            gunRigidbody.useGravity = true;
            gunRigidbody.constraints = 0;
            col.isTrigger = false;
            particles.SetActive(false);
        }


    }
}
