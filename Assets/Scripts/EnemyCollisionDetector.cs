using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // Find GameManager by type
        gameManager = FindObjectOfType<GameManager>();
        
        // Make collider a trigger for OnTrigger events
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    // Trigger enter - when something enters the enemy collider
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy collision detected with player!");
            if (gameManager != null)
            {
                gameManager.TriggerGameOver();
            }
        }
    }
}
