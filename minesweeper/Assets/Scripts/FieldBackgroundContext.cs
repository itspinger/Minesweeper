using System;
using System.Collections.Generic;
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
        BetterField field = controller.GetField();

        // Check if the field is odd and if the field is not revealed
        // Then the color should be (170, 215, 81, 255)
        if (field.IsOdd() && field.GetState() != BetterField.FieldState.Revealed)
        {
            ApplyNormalColor(new Color32(170, 215, 81, 255));
            return;
        }

        // Check if the field is not odd and if the field is not revealed
        // Then the color should be (162, 209, 73, 255)
        if (!field.IsOdd() && field.GetState() != BetterField.FieldState.Revealed)
        {
            ApplyNormalColor(new Color32(162, 209, 73, 255));
            return;
        }

        if (field.IsOdd())
        {
            ApplyNormalColor(new Color32(229, 194, 159, 255));
            return;
        }

        ApplyNormalColor(new Color32(215, 184, 153, 255));
    }
    
    private void ApplyNormalColor(Color32 color)
    {
        ColorBlock block = this.controller.fieldButton.colors;

        // Set the color
        block.normalColor = color;
        block.pressedColor = color;
        block.selectedColor = color;
        block.disabledColor = color;

        // Assign the fieldButton struct
        this.controller.fieldButton.colors = block;
    }
}
