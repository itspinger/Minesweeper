using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldController : MonoBehaviour
{
    private BetterField field;

    // Controller objects
    public TMP_Text adjacentText;
    public Button fieldButton;

    private void Awake()
    {
        // Get the field from the game object
        field = GetComponent<BetterField>();
    }

    private void Update()
    {
        ApplyBackground();
    }

    public BetterField GetField()
    {
        return field;
    }

    public void ApplyBackground()
    {
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

        // This means that the field is revealed
        // Check if the field is odd
        // And get the revealed color depending on it
        if (field.IsOdd())
        {
            ApplyNormalColor(new Color32(229, 194, 159, 255));
            return;
        }

        ApplyNormalColor(new Color32(215, 184, 153, 255));
    }

    public void ApplyNormalColor(Color32 color)
    {
        ColorBlock block = fieldButton.colors;

        // Set the color
        block.normalColor = color;
        //block.pressedColor = color;
        //block.selectedColor = color;
        //block.disabledColor = color;

        // Assign the fieldButton struct
        fieldButton.colors = block;
    }

    public class FieldText
    {
        private static readonly List<FieldText> fields = new List<FieldText>();

        static FieldText()
        {
            // Color for number 1
            new FieldText(new Color32(25, 118, 210, 255));

            // Color for number 2
            new FieldText(new Color32(56, 142, 60, 255));

            // Color for number 3
            new FieldText(new Color32(211, 48, 48, 255));

            // Color for number 4
            new FieldText(new Color32(123, 31, 162, 255));

            // Color for number 5
            new FieldText(new Color32(123, 31, 162, 255));

            // Color for number 6
            new FieldText(new Color32(123, 31, 162, 255));

            // Color for number 7
            new FieldText(new Color32(123, 31, 162, 255));

            // Color for number 8
            new FieldText(new Color32(123, 31, 162, 255));
        }

        private readonly Color32 color;

        public FieldText(Color32 color)
        {
            this.color = color;

            // Add to the fields
            fields.Add(this);
        }

        /**
         * This method returns the color
         * of this field text;
         */

        public Color32 GetColor()
        {
            return color;
        }

        public static void Apply(FieldController controller)
        {
            // Get the number of adjacent mines
            // From this field
            int mines = controller.GetField().GetAdjacentMines();

            // Edge case
            // In case the adjacent amount is 0
            if (controller.GetField().GetAdjacentMines() == 0)
            {
                return;
            }

            // Get the correct color
            FieldText text = fields[mines - 1];

            // Safe check
            // In case the color is null
            if (text == null)
            {
                return;
            }

            controller.adjacentText.text = mines.ToString();
            controller.adjacentText.color = text.GetColor();
        }
    }
}
