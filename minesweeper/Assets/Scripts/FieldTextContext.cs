using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This type is an implementation of the FieldContext type.
 * 
 * It is used when the state of a field is changed from the Hidden state to the Revealed state.
 * Depending on the amount of mines adjacent to a field, we use this class to set a specific text and text color.
 */

public class FieldTextContext : FieldContext
{
    public FieldTextContext(FieldController controller) : base(controller)
    {

    }

    public override void Apply()
    {
        // Get the field instance itself
        Field field = controller.GetField();

        // Get the number of adjacent mines
        int mines = field.GetAdjacentMines();

        // Edge case
        // In case the adjacent amount is 0
        if (field.GetAdjacentMines() == 0)
        {
            return;
        }

        // Get the correct color
        FieldText text = FieldText.fields[mines - 1];

        // Safe check
        // In case the color is null
        if (text == null)
        {
            return;
        }

        controller.adjacentText.text = mines.ToString();
        controller.adjacentText.color = text.GetColor();
    }

    public class FieldText
    {
        public static readonly List<FieldText> fields = new List<FieldText>();

        /**
         * This block adds the correct colors 
         * to this field block.
         */

        static FieldText()
        {
            // Create contexts for all 8 number
            Create(new Color32[] { 
                        new Color32(25, 118, 210, 255), new Color32(56, 142, 60, 255),
                        new Color32(211, 48, 48, 255), new Color32(123, 31, 162, 255),
                        new Color32(123, 31, 162, 255), new Color32(123, 31, 162, 255),
                        new Color32(123, 31, 162, 255), new Color32(123, 31, 162, 255)
                  });
        }

        private readonly Color32 color;

        /**
         * This method represents the only constructor for the FieldText class.
         * <p>
         * A single instance of a color is needed, which gets stored
         * into the colors list.
         * Based on this instance, the colors for the numbers inside a field is set.
         * <param name="color">the color to add to the list</param>
         */

        public FieldText(Color32 color)
        {
            this.color = color;

            // Add to the fields
            fields.Add(this);
        }

        /**
         * This method creates a new instance of the FieldText type
         * from a static context.
         */

        public static void Create(Color32[] color)
        {
            foreach (Color c in color)
            {
                new FieldText(c);
            }   
        }

        /**
         * This method returns the color
         * of this field text.
         */

        public Color32 GetColor()
        {
            return color;
        }
    }
}
