using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisslePhysics : MonoBehaviour
{
    public GameObject explose;
    public GameObject decal;

    public float damage = 10.0f;
    public int team = 0;
    public Transform player;
    private Transform mainCamera;
    public float radius = 3.0f;
    public float lifeTime = 3.0f;

    private LayerMask layer = (1 << 10);

 //   private Player2 playerScript;
    private Rigidbody RB;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }
    void Start()
    {
        RB = transform.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
     //   playerScript = player.GetComponent<Player2>();
        foreach (Collider c in player.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(c, transform.GetComponent<Collider>());   //ignoruje kolizje z samym soba
        Physics.IgnoreCollision(mainCamera.GetComponent<Collider>(), transform.GetComponent<Collider>()); //ignoruje kolizje z kamera
        Invoke("Destroy", lifeTime);
    }

    void Explosion()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in objectsInRange)
        {
            if (col.transform == col.transform.root)
            {
                RaycastHit h;
                Vector3 datTrasform = col.transform.position + col.transform.up * 0.5f - (transform.position - transform.forward * 3.0f);
                Physics.Raycast((datTrasform - col.transform.position - col.transform.up * 0.5f) * -1, (datTrasform).normalized, out h, (datTrasform).magnitude, layer);

                if (!h.collider)
                {

                    Rigidbody colRB = col.GetComponent<Rigidbody>();
                    if (colRB)
                    {
                        colRB.AddExplosionForce(500.0f, transform.position, radius, 0.8f);
                    }

                    Transform col2 = col.transform.Find("Armature/MasterCtrl/hipsCtrl/root");
                    if (col2)
                    {
                        float proximity = (transform.position - col.transform.position).magnitude;
                        float effect = 1 - (proximity / radius);
                        HitBox_Info datCol2 = col2.GetComponent<HitBox_Info>();
                        datCol2.damage = damage * effect;
                        datCol2.team = team;
                        if(col2.parent.root.gameObject == player.gameObject)
                            datCol2.team = 0;
                    }
                }
            }
        }
    }

    void Destroy()
    {

        gameObject.SetActive(false);
        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
        Explosion();

        GameObject exploseObj = Instantiate(explose);
        exploseObj.transform.position = transform.position - transform.forward + Vector3.up;
        Destroy(exploseObj, 2.2f);

        Instantiate(decal, exploseObj.transform.position - 0.8f * Vector3.up, transform.rotation * Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360)));
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Player" && collision.tag != "Missle" && collision.tag != "MainCamera")
        {
            Destroy();
        }
        if (collision.tag == "HitBox")
        {
            collision.GetComponent<HitBox_Info>().damage = damage;
            collision.GetComponent<HitBox_Info>().team = team;
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
