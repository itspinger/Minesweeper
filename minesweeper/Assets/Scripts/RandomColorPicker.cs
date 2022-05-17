using System.Collections.Generic;
using UnityEngine;

public class RandomColorPicker
{
    private readonly static List<Color32> colors = new List<Color32>();
    private readonly static System.Random random = new System.Random();

    static RandomColorPicker() {
        colors.AddRange(new[] {
            // Dark Green, Light Yellow
            new Color32(0, 135, 68, 255), new Color32(250, 228, 147, 255),
            // Aqua, Dark Blue
            new Color32(72, 230, 241, 255), new Color32(72, 133, 237, 255),
            // Purple, Pink
            new Color32(237, 68, 181, 255), new Color32(182, 72, 242, 255),
            // Red, Orange
            new Color32(219, 50, 54, 255), new Color32(244, 132, 13, 255),
            // Lighter Blue and Yellow
            new Color32(170, 199, 247, 255), new Color32(244, 194, 13, 255)
        }); 
    }

    /**
     * This method returns a random color from the colors list.
     */

    public static Color32 GetRandomColor()
    {
        return colors[random.Next(colors.Count)];
    }
}
