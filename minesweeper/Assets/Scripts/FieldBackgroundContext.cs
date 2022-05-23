using UnityEngine;
using UnityEngine.UI;

public class FieldBackgroundContext : FieldContext
{
    public FieldBackgroundContext(FieldController controller) : base(controller)
    {

    }

    public override void Apply()
    {
        // Get the field from the controller
        Field field = controller.GetField();

        // Initialize the highlighted color
        Color32 color = new Color32(193, 214, 152, 255);

        // Check if the field is odd and if the field is not revealed
        // Then the color should be (170, 215, 81, 255)
        if (field.IsOdd() && field.GetState() != Field.FieldState.Revealed)
        {
            ApplyNormalColor(new Color32(170, 215, 81, 255), color);
            return;
        }

        // Check if the field is not odd and if the field is not revealed
        // Then the color should be (162, 209, 73, 255)
        if (!field.IsOdd() && field.GetState() != Field.FieldState.Revealed)
        {
            ApplyNormalColor(new Color32(162, 209, 73, 255), color);
            return;
        }

        // Update the color when it's revealed
        color = new Color32(236, 209, 183, 255);

        if (field.IsOdd())
        {
            ApplyNormalColor(new Color32(229, 194, 159, 255), color);
            return;
        }

        ApplyNormalColor(new Color32(215, 184, 153, 255), color);
    }
    
    private void ApplyNormalColor(Color32 color, Color32 higlighted)
    {
        ColorBlock block = this.controller.fieldButton.colors;

        // Set the color
        block.normalColor = color;
        block.highlightedColor = higlighted;
        block.pressedColor = color;
        block.selectedColor = color;
        block.disabledColor = color;

        // Assign the fieldButton struct
        this.controller.fieldButton.colors = block;
    }
}
