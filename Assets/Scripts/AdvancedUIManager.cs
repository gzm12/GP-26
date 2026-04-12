using UnityEngine;
using TMPro;

/// <summary>
/// Alternative UI Manager implementation extending BaseUIManager.
/// Demonstrates how to extend the abstract class with custom behavior.
/// Example: Shows score with animation or different styling.
/// </summary>
public class AdvancedUIManager : BaseUIManager
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    
    // Animation settings
    [SerializeField] private float scorePopDuration = 0.3f;
    private float scorePopTimer = 0f;
    private int currentScore = 0;

    void Start()
    {
        if (scoreText == null)
            scoreText = GetComponentInChildren<TextMeshProUGUI>();

        // Initialize UI
        InitializeUI();
    }

    void Update()
    {
        // Handle score pop animation
        if (scorePopTimer > 0)
        {
            scorePopTimer -= Time.deltaTime;
            if (scoreText != null)
            {
                // Animate scale
                float scale = 1f + (scorePopTimer / scorePopDuration) * 0.2f;
                scoreText.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }

    // ========== ABSTRACT METHOD IMPLEMENTATIONS ==========

    /// <summary>
    /// Initialize UI elements.
    /// </summary>
    protected override void InitializeUI()
    {
        UpdateScoreDisplay();

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (scoreText != null)
            scoreText.color = Color.white;
    }

    /// <summary>
    /// Update score display with styling.
    /// </summary>
    public override void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"<b>SCORE: {currentScore}</b>";
        }
    }

    /// <summary>
    /// Handle game over with custom styling.
    /// </summary>
    public override void OnGameOver(int finalScore)
    {
        hasGameEnded = true;
        if (gameOverText != null)
        {
            gameOverText.text = $"<color=red><b>GAME OVER!</b></color>\n<size=80%>Final Score: {finalScore}</size>";
            gameOverText.gameObject.SetActive(true);
        }

        InvokeGameOverEvent();
    }

    /// <summary>
    /// Handle score change with animation pop effect.
    /// </summary>
    public override void OnScoreChanged(int newScore)
    {
        currentScore = newScore;
        
        if (scoreText != null)
        {
            scoreText.text = $"<b>SCORE: {newScore}</b>";
            // Trigger pop animation
            scorePopTimer = scorePopDuration;
        }
    }
}
