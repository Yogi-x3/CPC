using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldOfView : MonoBehaviour
{
    public GameObject Player;
    public float radius;
    public float angle;

    public LayerMask targetMask;
    public bool canSeePlayer;
    public Vector3 positionLastSeen;

    private Transform firePoint;
    public GameObject bulletPrefab;
    private bool isShooting;
    public float fireRate;

    public float bulletForce = 20f;
    public Vents vents;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        canSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {

        //Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        Collider hit = Physics.OverlapSphere(transform.position, radius, targetMask).FirstOrDefault(collider => collider.CompareTag("Player"));

        if (hit !=null)
        {
            Transform target = hit.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, targetMask))
                {
                    canSeePlayer = true;
                    if (this.gameObject.tag == ("Guard") && vents.inVent == false)
                    {
                        GameObject shootingGuard = this.gameObject;
                        firePoint = shootingGuard.transform; 
                        firePoint.rotation = Quaternion.LookRotation(Player.transform.position - firePoint.position);

                        if (isShooting != true)
                        {
                            StartCoroutine(Shoot());
                        }
                        
                    }
                    positionLastSeen = target.position;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer == true)
        {
            canSeePlayer = false;
        }

    }

    IEnumerator Shoot()
    {
        isShooting = true;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.transform.forward * 1.2f, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;


    }
}
