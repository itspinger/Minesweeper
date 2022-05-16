using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FieldController : MonoBehaviour
{
    private BetterField field;
    private FieldTextContext textContext;

    // Controller objects
    public TMP_Text adjacentText;
    public Button fieldButton;
    public Image flagImage;
    public Image mineImage;
    public Image xImage;

    private void Awake()
    {
        // Get the field from the game object
        field = GetComponent<BetterField>();

        // Update the components
        this.textContext = new FieldTextContext(this);
    }

    private void Update()
    {
        ApplyBackground();
        textContext.Apply();
        ApplyFlag();
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

        if (field.IsOdd())
        {
            ApplyNormalColor(new Color32(229, 194, 159, 255));
            return;
        }

        ApplyNormalColor(new Color32(215, 184, 153, 255));
    }

    internal void Reveal()
    {
        
    }

    public void ApplyFlag()
    {
        // Make sure it is not revealed
        if (field.GetState() == BetterField.FieldState.Revealed)
            return;

        // Check for flag
        if (field.GetState() == BetterField.FieldState.Flagged)
        {
            flagImage.gameObject.SetActive(true);
            return;
        }

        flagImage.gameObject.SetActive(false);
    }

    public void ApplyNormalColor(Color32 color)
    {
        ColorBlock block = fieldButton.colors;

        // Set the color
        block.normalColor = color;
        block.pressedColor = color;
        block.selectedColor = color;
        block.disabledColor = color;

        // Assign the fieldButton struct
        fieldButton.colors = block;
    }
}
