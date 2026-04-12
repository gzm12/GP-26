using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : BaseUIManager
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (scoreText == null)
            scoreText = GetComponentInChildren<TextMeshProUGUI>();

        // Subscribe to GameManager events
        if (gameManager != null)
        {
            gameManager.OnScoreChanged += OnScoreChanged_Handler;
            gameManager.OnGameOver += OnGameOver_Handler;
            Debug.Log("UIManager subscribed to GameManager events");
        }

        // Initialize UI
        InitializeUI();
    }

    void Update()
    {
        // Events handle UI updates now, no need for manual checking
    }

    // ========== ABSTRACT METHOD IMPLEMENTATIONS ==========

    /// <summary>
    /// Initialize UI elements at start.
    /// </summary>
    protected override void InitializeUI()
    {
        UpdateScoreDisplay();
        
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update score display on screen.
    /// </summary>
    public override void UpdateScoreDisplay()
    {
        if (gameManager != null && scoreText != null)
        {
            int currentScore = gameManager.GetScore();
            scoreText.text = "Score: " + currentScore;
        }
    }

    /// <summary>
    /// Handle game over event and display game over UI.
    /// </summary>
    public override void OnGameOver(int finalScore)
    {
        hasGameEnded = true;
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER!\nFinal Score: " + finalScore;
            gameOverText.gameObject.SetActive(true);
        }

        InvokeGameOverEvent();
    }

    /// <summary>
    /// Handle score change event.
    /// </summary>
    public override void OnScoreChanged(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + newScore;
    }

    // ========== EVENT HANDLERS ==========

    /// <summary>
    /// Handler called when score changes from GameManager.
    /// </summary>
    private void OnScoreChanged_Handler(int newScore)
    {
        OnScoreChanged(newScore);
    }

    /// <summary>
    /// Handler called when game over event fires from GameManager.
    /// </summary>
    private void OnGameOver_Handler(int finalScore)
    {
        OnGameOver(finalScore);
    }
}
