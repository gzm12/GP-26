using UnityEngine;

/// <summary>
/// Scrolls the material texture vertically to create an infinite moving background effect.
/// Attach to the plane (or any Renderer) and assign the material's texture to scroll.
/// For a bottom-to-top scroll increase `scrollSpeedY` (>0).
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("Scroll speed on X (horizontal) and Y (vertical) in texture UV units per second.")]
    public Vector2 scrollSpeed = new Vector2(0f, 0.5f);

    [Tooltip("If true the script will modify the shared material (affects all objects using it).")]
    public bool useSharedMaterial = false;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("BackgroundScroller requires a Renderer on the same GameObject.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (rend == null) return;

        // Read current offset
        Vector2 offset = useSharedMaterial ? rend.sharedMaterial.mainTextureOffset : rend.material.mainTextureOffset;

        // Move offset. For bottom-to-top scrolling increase Y.
        offset += scrollSpeed * Time.deltaTime;

        // Keep values in 0..1 to avoid drifting precision issues over time
        offset.x = offset.x - Mathf.Floor(offset.x);
        offset.y = offset.y - Mathf.Floor(offset.y);

        // Write back
        if (useSharedMaterial)
            rend.sharedMaterial.mainTextureOffset = offset;
        else
            rend.material.mainTextureOffset = offset;
    }
}
