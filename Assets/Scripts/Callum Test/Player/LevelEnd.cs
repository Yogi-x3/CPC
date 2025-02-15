using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{

    //public FloatSO scoreSO;
    public ScoreManager scoreManager;
    public PipeWeapon pipeWeapon;

    public bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        triggered = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggered == false)
        {
            triggered=true;
            
            int totalValue = 0;

            // Iterate through the pipes list to calculate total value
            for (int i = 0; i < pipeWeapon.pipes.Count; i++)
            {
                if (pipeWeapon.pipes[i] != null)  // Check for null pipe
                {
                    totalValue += pipeWeapon.pipes[i].value;  // Add value of non-null pipe
                }
            }
            SceneManager.LoadScene("Shop");
            scoreManager.UpdateMoney(totalValue);
        }
        
    }
}
