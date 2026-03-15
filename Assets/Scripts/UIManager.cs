using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameManager gameManager;

    private bool hasGameEnded = false;

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
        }

        UpdateScoreDisplay();
    }

    void Update()
    {
        // Events handle UI updates now, no need for manual checking
    }

    // Public method to update score display
    public void UpdateScoreDisplay()
    {
        if (gameManager != null && scoreText != null)
        {
            int currentScore = gameManager.GetScore();
            scoreText.text = "Score: " + currentScore;
        }
    }

    // Event handler for score changes
    private void OnScoreChanged_Handler(int newScore)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + newScore;
    }

    // Event handler for game over
    private void OnGameOver_Handler(int finalScore)
    {
        hasGameEnded = true;
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER!\nFinal Score: " + finalScore;
            gameOverText.gameObject.SetActive(true);
        }
    }

    // Check if game is over and display message
    void CheckGameOver()
    {
        if (gameManager != null && gameManager.IsGameOver())
        {
            hasGameEnded = true;
            if (gameOverText != null)
            {
                gameOverText.text = "GAME OVER!\nFinal Score: " + gameManager.GetScore();
                gameOverText.gameObject.SetActive(true);
            }
        }
    }
}
