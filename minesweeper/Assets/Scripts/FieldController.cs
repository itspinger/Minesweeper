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
    private FieldBackgroundContext backgroundContext;
    private FieldImageContext imageContext;

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
        this.backgroundContext = new FieldBackgroundContext(this); 
        this.imageContext = new FieldImageContext(this);
    }

    private void Update()
    {
        backgroundContext.Apply();
        textContext.Apply();
        imageContext.Apply();
    }

    public BetterField GetField()
    {
        return field;
    }

    internal void Reveal()
    {

    }
}
