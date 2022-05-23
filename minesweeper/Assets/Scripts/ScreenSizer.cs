using UnityEngine;
using UnityEngine.UI;

public class ScreenSizer : MonoBehaviour
{
    public Vector2Int padding = Vector2Int.zero;
    public RectTransform fieldTransform;
    public CanvasScaler gameScaler;
    public CanvasScaler menuScaler;

    // This represents the current ratio of the game
    private Vector2 currentRatio = new Vector2(700, 660);

    void Update()
    {
        var resolution = new Vector2Int(Screen.width, Screen.height);

        // Get the current aspect ratio
        // From the game controller
        currentRatio = new Vector2Int((int) fieldTransform.sizeDelta.x + padding.x, (int) fieldTransform.sizeDelta.y + padding.y);

        // Check if the resolution already equals
        // To the ratio
        if (resolution.Equals(currentRatio))
        {
            return;
        }

        // Set the resolution from the aspect
        gameScaler.referenceResolution = currentRatio;
        menuScaler.referenceResolution = currentRatio;

        if (currentRatio.x > 1500 || currentRatio.y > 1500)
        {
            // Update the resolution
            Screen.SetResolution((int) (currentRatio.x / 2), (int) (currentRatio.y / 2), false);
            return;
        }

        if (currentRatio.x > 1200 || currentRatio.y > 1200)
        {
            Screen.SetResolution((int) (currentRatio.x / 1.5), (int) (currentRatio.y / 1.5), false);
            return;
        }

        // Update the resolution
        Screen.SetResolution((int) currentRatio.x, (int) currentRatio.y, false);
    }
}
