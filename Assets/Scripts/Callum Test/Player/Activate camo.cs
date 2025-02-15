using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatecamo : MonoBehaviour
{
    public FloatSO scoreSO;
    public GameObject player;
    private bool stealthActive = false;
    private Renderer renderer;

    public Material stealthCamo;
    public Material defaultMaterial;
    public Material xRayMaterial;
    public Renderer[] xRayObjects;
    private bool xRayActive;
    public Material[] initialMaterials;
    public Movement movement;

    public bool energyDraining;
    public bool energyRecharging;
    public float energy = 100;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        renderer = GetComponent<Renderer>();
        xRayObjects = UnityEngine.Object.FindObjectsOfType<Renderer>();
        xRayActive = false;

        initialMaterials = new Material[xRayObjects.Length];

        for (int i = 0; i < xRayObjects.Length; i++)
        {
            initialMaterials[i] = xRayObjects[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && scoreSO.StealthPurchased == true)
        {
            StealthMode();
        }
        EnergyManagement();

        if (Input.GetKeyDown(KeyCode.Q) && movement.camMode == true)
        {
            ActivateXRay();
        }
    }

    public void StealthMode()
    {
        stealthActive = !stealthActive;
    }

    public IEnumerator CamoEnergy()
    {
        energyDraining = true;
        energyRecharging = false;
        yield return new WaitForSeconds(0.2f);
        energy -= 2;
        energyDraining = false;
    }

    public IEnumerator EnergyRecharge()
    {
        energyDraining = false;
        energyRecharging = true;
        yield return new WaitForSeconds(0.5f);
        energy += 1;
        energyRecharging = false;
    }

    public void EnergyManagement()
    {
        if (energy == 0)
        {
            stealthActive = false;
        }

        if (energy < 0)
        {
            energy = 0;
        }

        if (stealthActive == true)
        {
            renderer.material = stealthCamo;
            player.tag = ("Stealth");
        }
        else
        {
            renderer.material = defaultMaterial;
            player.tag = ("Player");
        }

        if (stealthActive == true && energy > 0 && energyDraining != true)
        {
            StartCoroutine(CamoEnergy());
            Debug.Log("DRAINING");
        }

        if (stealthActive == false && energy < 100 && energyRecharging == false)
        {
            StartCoroutine(EnergyRecharge());
            Debug.Log("Recharging");
        }
    }

    void ActivateXRay()
    {
        xRayActive = !xRayActive;
        if (xRayActive)
        {
            foreach (Renderer mesh in xRayObjects)
            {
                mesh.material = xRayMaterial;
            }
        }
        else 
        {

            for (int i = 0; i < xRayObjects.Length; i++)
            {
                xRayObjects[i].material = initialMaterials[i];
            }
        }
    }
   
}
