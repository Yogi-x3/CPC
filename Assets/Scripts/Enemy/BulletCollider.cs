using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public Movement movement;
    public PipeWeapon pipeWeapon;
    private GameObject player;
    private float distToPlayer;
   
    // Start is called before the first frame update
    void Awake()
    {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        pipeWeapon = GameObject.Find("PipeHolder").GetComponent<PipeWeapon>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DestroyBullet());
    }

    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Parry();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stealth"))
        {
            movement.playerKilled = true;
        }
        if (collision.gameObject.CompareTag("Guard"))
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    void Parry()
    {
        if (pipeWeapon.weaponActive == true)
        {
            if (distToPlayer > 0.1 && distToPlayer < 3)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
                    rb.AddForce(this.gameObject.transform.forward * -20f, ForceMode.Impulse);
                }
            }
        }
    }
}
