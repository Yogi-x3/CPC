using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PipeWeapon : MonoBehaviour
{
    public GameObject pipeWeapon;
    public bool weaponActive;
    public PipePuzzle pipePuzzle;
    public Weapon activePipe;  // Changed from GameObject to Weapon
    public Animator pipeAnimation;
    public AnimationClip swingAnimation;
    private bool isSwinging;
    public Movement movement;
    public bool enemyHit;
    public ScoreManager scoreManager;

    public List<Weapon> pipes = new List<Weapon>();  // Store list of Weapon instances

    public float lastHitTime = -1f;
    public float hitCooldown;

    private int pipeValue;
    public int degradedPipeValue;
    private int pipeMaxHealth = 3;

    public InventoryUI inventoryUI;
    private int nextAvailableSlot = 0;



    void Start()
    {
        pipeWeapon.SetActive(false);
        weaponActive = false;
        isSwinging = false;
        hitCooldown = swingAnimation.length;
    }

    void Update()
    {
        // Check if pipes list is empty
        if (pipes.Count == 0)
        {
            activePipe = null;
            Debug.Log("[Update] pipes list is empty. Setting activePipe to null.");
        }

        if (pipePuzzle.inPuzzle == false)
        {
            CheckPipeHover();


        }

        pipeWeapon.SetActive(weaponActive);

        if (activePipe != null)
        {

            SwingPipe();
            PipeDurability();
            SwitchInventory();
        }
        else
        {
            Debug.Log("Null");
        }


    }

    void CheckPipeHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pipePuzzle.interactionDistance))
        {
            if (hit.collider.CompareTag("Copper Pipe"))
            {
                if (Input.GetMouseButtonDown(0)) // Left mouse button click
                {
                    GameObject pipeObject = hit.collider.gameObject;
                    PickUpPipe(pipeObject);
                }
            }
        }
    }

    void PickUpPipe(GameObject pipeObject)
    {
        int durability = 3;
        int value = Random.Range(300, 600);
        // Create a unique Weapon instance with a random durability
        Weapon newPipe = new Weapon(pipeObject.name, durability, value);
        activePipe = newPipe;

        pipes.Add(newPipe);
        weaponActive = true;
        pipeObject.SetActive(false);
        //scoreManager.UpdateBool(weaponActive);

    }

    void SwingPipe()
    {
        if (weaponActive && Input.GetMouseButtonDown(0) && !isSwinging && !movement.camMode)
        {
            StartCoroutine(SwingAnimation());
        }
    }

    IEnumerator SwingAnimation()
    {
        isSwinging = true;
        pipeAnimation.SetBool("isSwinging", true);
        yield return new WaitForSeconds(1f);
        pipeAnimation.SetBool("isSwinging", false);
        isSwinging = false;
    }

    void PipeDurability()
    {
        if (isSwinging && enemyHit)
        {
            if (Time.time - lastHitTime > hitCooldown + 0.4f)
            {
                activePipe.durability -= 1;  // Reduce active weapon's durability
                lastHitTime = Time.time;
                degradedPipeValue = (int)(activePipe.value * ((float)activePipe.durability / pipeMaxHealth));
                activePipe.value = degradedPipeValue;
                Debug.Log(activePipe.value);
                Debug.Log($"Durability left: {activePipe.durability}, Degraded value: {degradedPipeValue}");
            }
        }

        if (activePipe.durability <= 0 && activePipe != null)
        {
            weaponActive = false;
            //pipeWeapon.SetActive(false);  // Deactivate if durability is depleted
            Debug.Log($"{activePipe.name} has broken!");
            pipes.Remove(activePipe);
            //activePipe = null;  // Remove active weapon
        }
    }
    void SwitchInventory()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && pipes.Count > 0)
        {
            activePipe = pipes[0];
            if (weaponActive == false)
            {
                weaponActive = true;
            }
            Debug.Log($":{activePipe.value}, {activePipe.durability}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && pipes.Count > 1)
        {
            activePipe = pipes[1];
            if (weaponActive == false)
            {
                weaponActive = true;
            }
            Debug.Log($":{activePipe.value}, {activePipe.durability}");
        }
    }

}