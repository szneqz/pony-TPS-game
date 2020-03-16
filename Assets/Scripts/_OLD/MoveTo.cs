using UnityEngine;
using System.Collections;


public class MoveTo : MonoBehaviour
{

    public Transform goal;
    public Vector3 corner;
    public bool notMatch = false;
    public UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();    //wywolanie agenta
        goal = GameObject.Find("pony3_lowPoly").transform;
    }
    private void Update()
    {
        if (agent.remainingDistance > 0.1f)    //wyznaczenie miesjca kata, do ktorego ma isc
            corner = agent.path.corners[1];
        transform.position = transform.parent.position;
        notMatch = new Vector3(Mathf.Round(agent.nextPosition.x), Mathf.Round(agent.nextPosition.y), Mathf.Round(agent.nextPosition.z)) == new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 0.1f), Mathf.Round(transform.position.z));
        agent.destination = goal.position;                      //gdzie ma sie udac
    }
}