using System.Collections;
using System.Collections.Generic;
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
    //private Vector2 previousResolution;

    private void Start()
    {
        //previousResolution = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        var resolution = new Vector2Int(Screen.width, Screen.height);

        // Get the current aspect ratio
        // From the game controller
        currentRatio = new Vector2Int((int) fieldTransform.sizeDelta.x + padding.x, (int) fieldTransform.sizeDelta.y + padding.y); 

        // Set the resolution from the aspect
        if (resolution != currentRatio)
        {
            gameScaler.referenceResolution = currentRatio;
            menuScaler.referenceResolution = currentRatio;

            if (currentRatio.x > 900 || currentRatio.y > 900)
            {
                // Update the resolution
                Screen.SetResolution((int) (currentRatio.x / 1.5), (int) (currentRatio.y / 1.5), false);
                return;
            }

            // Update the resolution
            Screen.SetResolution((int) currentRatio.x, (int) currentRatio.y, false);
        }
    }
}
