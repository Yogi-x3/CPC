using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject[] enemies;
    //public GameObject enemy;
    public GameObject player;
    public float maxRange = 100;

    private bool CCTVCam;
    private bool isCCTVrunning;
    public bool enemyShown;

    public ScoreManager scoreManager;
    public FloatSO scoreSO;

    // Start is called before the first frame update
    void Start()
    {
        CCTVCam = scoreSO.CamPurchased;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        isCCTVrunning = false;
        enemyShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        Detection();
        //Debug.DrawRay(player.transform.position, (enemy.transform.position - player.transform.position), Color.green);

        if (CCTVCam == true && isCCTVrunning == false)
        {
            StartCoroutine(CCTV());
        }
    }

    public void Detection()
    {
        // Loop through each enemy in the array
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue; // Skip any null enemies (in case they are destroyed)

            RaycastHit hit;
            Vector3 directionToEnemy = enemy.transform.position - player.transform.position;

            if (Physics.Raycast(player.transform.position, directionToEnemy, out hit, maxRange))
            {
                // Check if the ray hit the enemy
                if (hit.collider.gameObject == enemy)
                {
                    enemy.layer = 3; // Set the layer to "seen" (or whatever layer 3 represents)
                }
                else if (enemyShown == false)
                {
                    enemy.layer = 7; // Set the layer to "hidden" (or whatever layer 7 represents)
                }
            }
        }
    }

    public IEnumerator CCTV()
    {
        isCCTVrunning = true;
        yield return new WaitForSeconds(5f);
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            enemy.layer = 3;
            enemyShown = true;
        }
        yield return new WaitForSeconds(2f);
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            enemy.layer = 7;
            enemyShown = false;
        }
        isCCTVrunning = false;
    }
}
