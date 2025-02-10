using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;

    public Transform centrePoint;

    public FieldOfView fov;
    public List<GameObject> guards;
    public GameObject activeGuard;
    public float guardDistance;
    private bool guardSeeking;
    public float enemySpeed = 5f;
    private bool getHelp = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        guards = new List<GameObject>(GameObject.FindGameObjectsWithTag("Guard"));
    }

    // Update is called once per frame
    void Update()
    {
        if (getHelp != true)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(centrePoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
        PlayerDetected();

        for (int i = guards.Count - 1; i >= 0; i--)
        {
            GameObject guard = guards[i];
            if (guard == null || guard.Equals(null))
            {
                guards.RemoveAt(i); // Remove destroyed objects safely
            }
        }


            if (guardSeeking == true)
        {
            GuardActive();
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
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

    void PlayerDetected()
    {
        if (fov.canSeePlayer == true)
        {
            agent.enabled = false;
            FindClosestGuard();
            getHelp = true;
        }

        if (getHelp == true)
        {
            var step = enemySpeed * Time.deltaTime;
            Vector3 targetDirection = activeGuard.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, activeGuard.transform.position, step);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 6f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            if (Vector3.Distance(transform.position, activeGuard.transform.position) < 3f)
            {
                agent.enabled = true;
                guardSeeking = true;
                getHelp = false;
            }
        }
        else
        {
            agent.enabled = true;
        }
    }

    void FindClosestGuard()
    {
        float closestDistance = Mathf.Infinity;

        foreach (GameObject guard in guards)
        {
            if (guard != null)
            {
                float distanceToGuard = Vector3.Distance(transform.position, guard.transform.position);
                if (distanceToGuard < closestDistance)
                {
                    closestDistance = distanceToGuard;
                    activeGuard = guard.gameObject;
                }
            }
        }

        guardDistance = closestDistance;
    }

    void GuardActive()
    {
        var step = enemySpeed * 1.5f * Time.deltaTime;
        Vector3 targetDirection = fov.positionLastSeen - activeGuard.transform.position;
        activeGuard.transform.position = Vector3.MoveTowards(activeGuard.transform.position, fov.positionLastSeen, step);
        Vector3 newDirection = Vector3.RotateTowards(activeGuard.transform.forward, targetDirection, step, 6f);
        activeGuard.transform.rotation = Quaternion.LookRotation(newDirection);
        if (Vector3.Distance(activeGuard.transform.position, fov.positionLastSeen) < 0.01f)
        {
            guardSeeking = false;
        }
    }
}
