using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PriestMovement : MonoBehaviour
{
    [Header("Scripts")]
    public UI uiScript;
    public Cinematic cinematicScript;

    [Header("Priest")]
    public GameObject agentObject;
    public NavMeshAgent agent;
    public Animator popeAnimator;
    public Transform centrePoint; //centre of the area the agent moves around in

    public float range = 10; //radius of sphere
    private float startSpeed;
    private bool isLooking;
    private bool pointFound;


    void Start()
    {
        agentObject = this.gameObject;
        agent = GetComponent<NavMeshAgent>();
        startSpeed = agent.speed;
    }


    void Update()
    {
        //if agent gets near to point, stop and start coroutine for animation
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isLooking)
            {
                StartCoroutine(LookUp());
            }
            //once coroutine done, look for new point to move to in area
            if (pointFound)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    //gizmos
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
        //stop agent in cinematic, doesnt reach point so can continue if no
        if (uiScript.cinematicMode == true)
        {
            agent.speed = 0;
        } else
        {
            agent.speed = startSpeed;
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //pick a random point in the area, if one if found set it
        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    //play animation when priest reaches desination
    public IEnumerator LookUp()
    {
        isLooking = true;
        popeAnimator.SetBool("IsLooking", true);
        pointFound = false;
        yield return new WaitForSeconds(4.5f);
        pointFound = true;
        popeAnimator.SetBool("IsLooking", false);
        yield return new WaitForSeconds(1f);
        isLooking = false;
    }
}