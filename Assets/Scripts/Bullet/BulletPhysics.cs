using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour {

    public float damage = 10.0f;
    public float noChangDmgTime = 0.0f;
    public int team = 0;
    public Transform player;
    private Transform mainCamera;
    private float zSize;

    public GameObject decal;

    private Rigidbody RB;
    private float time = 0.0f;

    private BoxCollider collid;

    private bool Stay = true;
    private Vector3 startpos = Vector3.zero;

    private bool ifDestroy = false;

   // private RaycastHit hit;
  //  public LayerMask layer;


    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }
    void Start()
    {
        RB = transform.GetComponent<Rigidbody>();
        collid = transform.GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        ifDestroy = false;

        time = 0.0f;
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, transform.GetComponent<Collider>());   //ignoruje kolizje z samym soba
        Physics.IgnoreCollision(mainCamera.GetComponent<Collider>(), transform.GetComponent<Collider>()); //ignoruje kolizje z kamera
        Invoke("Destroy", 3.0f);

        Stay = true;                        //SUPERSKRYPT - przez pierwsza klatke pocisk jest ciagle w tym samym miejscu
        startpos = Vector3.zero;            //SUPERSKRYPT - info o polozeniu poczatkowego
    }

    void FixedUpdate()
    {
        if(ifDestroy)
        {
            Destroy();
        }

        if(Stay && startpos != Vector3.zero)    //SUPERSKRYPT - wdrozenie informacji o poczatkowej pozycji
        {
            RB.position = startpos;
            Stay = false;
        }

        time += Time.fixedDeltaTime;
        if (time > noChangDmgTime)
            damage -= ((time - noChangDmgTime) / (1.5f - noChangDmgTime)) * damage;

        zSize = RB.velocity.magnitude * Time.fixedDeltaTime * 1.0f;
        float zCenter = (zSize / 2 * 0.8f);

        collid.size = new Vector3(0.1f, 0.1f, zSize);
        collid.center = new Vector3(0.0f, 0.0f, zCenter);
        //Debug.Log(collid.size.z);

        if(Stay)                                //SUPERSKRYPT - zapisanie informacji o poczatkowej pozycji
        {
            startpos = RB.position;
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false);
        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;

        RaycastHit h;

        if (Physics.Raycast(transform.position - transform.forward * zSize * 1.2f, transform.forward, out h, zSize * 2.0f))
        {
            Instantiate(decal, h.point, transform.rotation * Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360)));
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet" && collision.tag != "MainCamera" && collision.tag != "HitBox")
        {
            ifDestroy = true;
        }
        if (collision.tag == "HitBox")
        {
            if (collision.transform.root.gameObject != player.gameObject)   //zabezpieczenie przed strzelaniem w siebie
            {
                collision.GetComponent<HitBox_Info>().damage = damage;
                collision.GetComponent<HitBox_Info>().team = team;
            }
            Destroy();
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet")
        {
            Destroy();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Bullet")
        {
            Destroy();
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
