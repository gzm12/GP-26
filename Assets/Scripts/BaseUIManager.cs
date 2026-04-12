using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract base class for UI management systems.
/// Provides common functionality for score display and game over handling.
/// </summary>
public abstract class BaseUIManager : MonoBehaviour, IUIUpdatable
{
    [SerializeField] protected UnityEvent onGameOverEvent = new UnityEvent();
    
    protected bool hasGameEnded = false;

    /// <summary>
    /// Called when the game starts. Child classes should override this.
    /// </summary>
    protected abstract void InitializeUI();

    /// <summary>
    /// Called to update the score display.
    /// Child classes must implement how the score is displayed.
    /// </summary>
    public abstract void UpdateScoreDisplay();

    /// <summary>
    /// Called when game over event occurs.
    /// Child classes can override to customize game over behavior.
    /// </summary>
    public abstract void OnGameOver(int finalScore);

    /// <summary>
    /// Called when score changes.
    /// Child classes can override for custom handling.
    /// </summary>
    public virtual void OnScoreChanged(int newScore)
    {
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Invokes the game over event callback.
    /// </summary>
    protected void InvokeGameOverEvent()
    {
        onGameOverEvent?.Invoke();
    }

    // ========== IUIUpdatable Implementation ==========

    public virtual void UpdateUI()
    {
        UpdateScoreDisplay();
    }

    public virtual void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideUI()
    {
        gameObject.SetActive(false);
    }
}
