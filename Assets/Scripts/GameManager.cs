using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IDamageable
{
    // Events and Delegates
    public delegate void ScoreChangedDelegate(int newScore);
    public event ScoreChangedDelegate OnScoreChanged;

    public delegate void GameOverDelegate(int finalScore);
    public event GameOverDelegate OnGameOver;

    // Prefab references
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    // Player settings
    private GameObject player;
    private Rigidbody playerRb;
    private float playerSpeed = 10f;
    private Vector3 playerMovement = Vector3.zero;
    private float playerHealth = 100f;
    private float maxPlayerHealth = 100f;

    // Enemy settings
    private List<GameObject> enemies = new List<GameObject>();
    private float spawnInterval = 1.5f;
    private float spawnTimer = 0f;
    private float enemyFallSpeed = 5f;
    private float enemySpeedIncrement = 0.5f;

    // Game state
    private int score = 0;
    private bool isGameOver = false;
    private float scoreTimer = 0f;

    // Camera settings
    private Camera mainCamera;

    void Start()
    {
        // Load prefabs if not assigned
        if (playerPrefab == null)
            playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerPrefab");
        if (enemyPrefab == null)
            enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyPrefab");

        // Setup camera
        SetupCamera();
        CreatePlayer();
        
        // Find player by tag (alternative method)
        FindPlayerByTag();
        
        // Find all enemies (will be called periodically)
        FindAllEnemies();
    }

    void Update()
    {
        if (isGameOver)
            return;

        HandlePlayerInput();
        HandleEnemySpawning();
        HandleScore();
        UpdateEnemyPositions();
        CheckCollisions();
    }

    // ========== CAMERA SETUP ==========

    void SetupCamera()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraGO = new GameObject("Main Camera");
            mainCamera = cameraGO.AddComponent<Camera>();
            cameraGO.AddComponent<AudioListener>();
            cameraGO.tag = "MainCamera";
        }

        // Position camera for 3D view
        mainCamera.transform.position = new Vector3(0, 1f, -15f);
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        mainCamera.orthographic = false;
        mainCamera.fieldOfView = 60f;
    }

    // ========== PLAYER LOGIC ==========

    void CreatePlayer()
    {
        // Instantiate player from prefab
        player = Instantiate(playerPrefab);
        player.name = "Player";
        player.transform.position = new Vector3(0, 0.5f, 0);

        // Remove 2D components and add 3D components
        RemoveBoxCollider2D(player);
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            Destroy(rb2d);

        // Add 3D Rigidbody
        playerRb = player.GetComponent<Rigidbody>();
        if (playerRb == null)
            playerRb = player.AddComponent<Rigidbody>();

        playerRb.useGravity = false;
        playerRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        // Add 3D BoxCollider
        BoxCollider collider = player.GetComponent<BoxCollider>();
        if (collider == null)
            collider = player.AddComponent<BoxCollider>();
        collider.size = new Vector3(1f, 1f, 1f);

        // Set tag
        player.tag = "Player";
    }

    void HandlePlayerInput()
    {
        float inputX = 0f;

        // Yeni Input System - Keyboard kontrolü
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                inputX = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                inputX = 1f;
        }

        playerMovement = new Vector3(inputX * playerSpeed, 0f, 0f);
        playerRb.linearVelocity = playerMovement;

        // Clamp player position to screen bounds
        Vector3 pos = player.transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        player.transform.position = pos;
    }

    // ========== ENEMY LOGIC ==========

    void HandleEnemySpawning()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Instantiate enemy from prefab
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.name = "Enemy";
        float randomX = Random.Range(-8f, 8f);
        enemy.transform.position = new Vector3(randomX, 8f, 0);

        // Remove 2D components and add 3D components
        RemoveBoxCollider2D(enemy);
        Rigidbody2D rb2d = enemy.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            Destroy(rb2d);

        // Add 3D Rigidbody
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (rb == null)
            rb = enemy.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        // Add 3D BoxCollider
        BoxCollider collider = enemy.GetComponent<BoxCollider>();
        if (collider == null)
            collider = enemy.AddComponent<BoxCollider>();
        collider.size = new Vector3(1f, 1f, 1f);

        // Add collision detector script
        if (enemy.GetComponent<EnemyCollisionDetector>() == null)
            enemy.AddComponent<EnemyCollisionDetector>();

        // Set tag
        enemy.tag = "Enemy";

        // Add to list
        enemies.Add(enemy);
    }

    void UpdateEnemyPositions()
    {
        // Calculate current fall speed (increases over time)
        float currentFallSpeed = enemyFallSpeed + (Time.timeSinceLevelLoad * enemySpeedIncrement);

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemies[i];

            if (enemy == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            // Move enemy down
            enemy.transform.position += Vector3.down * currentFallSpeed * Time.deltaTime;

            // Destroy enemy if it goes below screen
            if (enemy.transform.position.y < -5f)
            {
                Destroy(enemy);
                enemies.RemoveAt(i);
            }
        }
    }

    // ========== COLLISION LOGIC ==========

    void CheckCollisions()
    {
        if (player == null)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemies[i];

            if (enemy == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            // 3D distance-based collision
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

            if (distance < 1f)
            {
                GameOver();
                return;
            }
        }
    }

    // ========== SCORE LOGIC ==========

    void HandleScore()
    {
        scoreTimer += Time.deltaTime;

        if (scoreTimer >= 1f)
        {
            score++;
            scoreTimer = 0f;
            Debug.Log("Score: " + score);
            
            // Invoke the OnScoreChanged event
            OnScoreChanged?.Invoke(score);
        }
    }

    // ========== GAME OVER LOGIC ==========

    void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        Debug.Log("Game Over! Final Score: " + score);

        // Stop all enemy movement
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.linearVelocity = Vector3.zero;
            }
        }

        // Stop player movement
        if (playerRb != null)
            playerRb.linearVelocity = Vector3.zero;

        // Invoke the OnGameOver event
        OnGameOver?.Invoke(score);
    }

    // ========== IDAMAGEABLE IMPLEMENTATION ==========

    /// <summary>
    /// Implementation of IDamageable interface.
    /// Reduces player health when taking damage.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (isGameOver)
            return;

        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage! Health: {playerHealth}/{maxPlayerHealth}");

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            GameOver();
        }
    }

    /// <summary>
    /// Get current player health.
    /// </summary>
    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    /// <summary>
    /// Get max player health.
    /// </summary>
    public float GetMaxPlayerHealth()
    {
        return maxPlayerHealth;
    }

    // ========== PUBLIC ACCESSORS ==========

    // Get current score
    public int GetScore()
    {
        return score;
    }

    // Check if game is over
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // Public method to trigger game over (called from EnemyCollisionDetector)
    public void TriggerGameOver()
    {
        GameOver();
    }

    // ========== FINDING METHODS ==========

    // Find player by tag
    void FindPlayerByTag()
    {
        GameObject foundPlayer = GameObject.FindWithTag("Player");
        if (foundPlayer != null && foundPlayer == player)
        {
            Debug.Log("Player found by tag: " + foundPlayer.name);
        }
    }

    // Find all enemies by tag and log them
    void FindAllEnemies()
    {
        // Find all enemies in the scene using tag
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Found " + allEnemies.Length + " enemies in the scene");
        
        // Compare with our managed list
        Debug.Log("Enemies in list: " + enemies.Count);
    }

    // ========== HELPER METHODS ==========

    void RemoveBoxCollider2D(GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        if (collider != null)
            Destroy(collider);
    }
}
