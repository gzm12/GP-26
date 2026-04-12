using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private float damageAmount = 10f;

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
            
            // Deal damage to player through IDamageable interface
            IDamageable damageable = gameManager as IDamageable;
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
            
            // Trigger game over as fallback
            if (gameManager != null)
            {
                gameManager.TriggerGameOver();
            }
        }
    }
}
