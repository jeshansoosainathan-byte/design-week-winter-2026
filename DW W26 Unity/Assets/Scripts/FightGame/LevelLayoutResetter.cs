// LevelLayoutResetter.cs
// Attach this to the parent level layout GameObject.
// When the parent is disabled, it resets ALL DestructiblePlatform children back to default.
// This ensures the level is clean when it gets selected again in a future round.

using UnityEngine;

public class LevelLayoutResetter : MonoBehaviour
{
    // Cache all platforms on startup so we don't search every time
    private DestructiblePlatform[] platforms;

    void Awake()
    {
        platforms = GetComponentsInChildren<DestructiblePlatform>(true);
    }

    void OnDisable()
    {
        // Reset every platform in this layout
        if (platforms != null)
        {
            foreach (var platform in platforms)
            {
                if (platform != null)
                {
                    platform.ResetPlatform();
                }
            }
        }
    }

    /// <summary>
    /// Call this manually if you need to refresh the platform cache
    /// (e.g. if platforms are added/removed at runtime).
    /// </summary>
    public void RefreshPlatformCache()
    {
        platforms = GetComponentsInChildren<DestructiblePlatform>(true);
    }
}
