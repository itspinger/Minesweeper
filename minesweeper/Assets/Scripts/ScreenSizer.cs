using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSizer : MonoBehaviour
{
    private Vector2 aspectRatio = new Vector2(700, 1100);
    private Vector2Int previousResolution;
    private bool changeable = true;

    public RectTransform field;
    public CanvasScaler gameScaler;
    public Vector2Int padding = new Vector2Int(0, 0);

    private void Start()
    {
        // Set the previous resolution
        // When the application starts
        previousResolution = new Vector2Int(Screen.width, Screen.height);
    }

    private void Update()
    {
        // Retrieve the current
        // Resolution of the game
        var resolution = new Vector2Int(Screen.width, Screen.height);

        // Get the aspect ratio from the field size
        aspectRatio = new Vector2(field.sizeDelta.x + padding.x, field.sizeDelta.y + padding.y);

        // Check 
        if (!changeable)
        {
            return;
        }

        // Check the if the resolution
        // Matches the aspect ratio
        if (previousResolution.x != resolution.x)
        {
            // Get the new height 
            float height = resolution.x * (aspectRatio.y / aspectRatio.x);

            // Update the resolution
            // Within a method
            SetResolution(resolution.x, (int) height);

            // Don't go through the other loop if this is applicable
            return;
        }

        // Check if the two resolutions
        // Match by the height
        if (previousResolution.y != resolution.y)
        {
            // Get the new width
            float width = resolution.y * (aspectRatio.x / aspectRatio.y);

            // Update the resolution
            // With the new width
            StartCoroutine(SetResolution((int) width, resolution.y));
        }
    }

    private IEnumerator SetResolution(int width, int height)
    {
        // Change the changeable to false
        this.changeable = false;

        // Change the canvas scaler
        gameScaler.referenceResolution = new Vector2(field.sizeDelta.x + padding.x, field.sizeDelta.y + padding.y);

        // Set the resolution
        // To the given width and height
        Screen.SetResolution(width, height, false);

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Change the last resolution to this resolution
        this.previousResolution = new Vector2Int(width, height);

        // Change the changeable to true
        this.changeable = true;
    }
}
