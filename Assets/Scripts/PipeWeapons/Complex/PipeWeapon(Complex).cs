using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PipeWeaponComplex : MonoBehaviour
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
        } else
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

        Item item = inventoryUI.itemDatabase.GetItemByTag("Copper Pipe");
        // Create a unique Weapon instance with a random durability
        Weapon newPipe = new Weapon(pipeObject.name, durability, value);
        activePipe = newPipe;

        pipes.Add(newPipe);
        weaponActive = true;
        pipeObject.SetActive(false);
        //scoreManager.UpdateBool(weaponActive);
        PickUpItem("Copper Pipe");

    }

    Sprite GetPipeIcon(GameObject pipeObject)
    {
        // This is a placeholder for how you might get the sprite for the pipe
        // For example, if you're using tags or the name of the object, 
        // you can assign an icon like this:

        if (pipeObject.CompareTag("Copper Pipe"))
        {
            return Resources.Load<Sprite>("Icons/CopperPipeIcon"); // Example path
        }

        // Return a default icon if the pipe type is unknown
        return Resources.Load<Sprite>("Icons/DefaultPipeIcon");
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
            RemovePipeFromInventory(activePipe);
            UpdateInventoryUI();
            //activePipe = null;  // Remove active weapon
        }
    }
    void SwitchInventory()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && pipes.Count > 0)
        {
            activePipe = pipes[0];
            UpdateInventoryUI();
            Debug.Log($":{activePipe.value}, {activePipe.durability}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && pipes.Count > 1)
        {
            activePipe = pipes[1];
            UpdateInventoryUI();
            Debug.Log($":{activePipe.value}, {activePipe.durability}");
        }
    }

    public void PickUpItem(string itemTag)
    {
        if (nextAvailableSlot >= inventoryUI.inventorySlots.Length)
        {
            Debug.LogWarning("Inventory is full!");
            return;
        }

        // Add the item to the next available slot in the UI
        inventoryUI.AddItemToUI(itemTag, nextAvailableSlot);

        // Update the next available slot
        nextAvailableSlot = FindNextAvailableSlot();
    }


    public int FindNextAvailableSlot()
    {
        for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
        {
            // Assuming the slot is available if the Image is disabled
            if (!inventoryUI.inventorySlots[i].enabled)
            {
                return i;  // Found an empty slot
            }
        }
        // No empty slots found, inventory is full
        return inventoryUI.inventorySlots.Length;

    }
    void UpdateInventoryUI()
    {
        // Loop through the inventory slots
        for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
        {
            if (pipes.Count > i)
            {
                // If the pipe in this slot is the active pipe, highlight it (e.g., add a border or change the sprite)
                if (pipes[i] == activePipe)
                {
                    inventoryUI.inventorySlots[i].color = Color.green; // Example of highlighting the active pipe
                }
                else
                {
                    inventoryUI.inventorySlots[i].color = Color.white; // Reset other slots to default color
                }
            }
        }
    }

    void RemovePipeFromInventory(Weapon pipeToRemove)
    {
        // Loop through inventory slots and check for the pipe icon
        for (int i = 0; i < inventoryUI.inventorySlots.Length; i++)
        {
            if (inventoryUI.inventorySlots[i].sprite == pipeToRemove.icon)
            {
                // Clear the inventory slot for this pipe
                inventoryUI.inventorySlots[i].sprite = null;
                inventoryUI.inventorySlots[i].enabled = false;  // Mark the slot as empty
                break;
            }
        }
    }
}
