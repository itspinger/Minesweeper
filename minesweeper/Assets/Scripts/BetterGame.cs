using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetterGame : MonoBehaviour
{
    public static BetterGame instance;

    // The field prefab
    public GameObject fieldPrefab;
    public RectTransform fieldTranfsorm;

    // Game Settings
    private int rows = 10;
    private int columns = 10;
    private int mines = 10;

    private bool _started, _finished;
    private BetterField[,] fields;

    public event System.Action OnInit;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        // Check for instance
        if (instance == null)
        {
            instance = this; 
        } else  
        {
            Destroy(this.gameObject);
        }

        fields = new BetterField[rows, columns];
    }

    private void Start()
    {
        // Initialize the stuff
        StartCoroutine(CreateGame());
    }

    private IEnumerator CreateGame()
    {
        // Just delay the start a second
        yield return new WaitForEndOfFrame();

        // Now initalize the important stuff
        InitField();

        // Check for resize
        if (OnInit != null)
        {
            OnInit();
        }
    }

    private void InitField()
    {
        foreach (var field in fields)
        {
            if (field == null)
                continue;

            Destroy(field.gameObject);
        }

        // Initialize the fields again
        fields = new BetterField[rows, columns];

        // Get the grid
        // In order to fill out the game
        GridLayoutGroup grid = fieldTranfsorm.GetComponent<GridLayoutGroup>();
        fieldTranfsorm.sizeDelta = new Vector2(rows * grid.cellSize.x, columns * grid.cellSize.y);

        // Create new fields
        // For the game
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                GameObject field = Instantiate(fieldPrefab, fieldTranfsorm);

                // Add its script to the fields
                // And set the parity
                fields[i, j] = field.GetComponent<BetterField>();
                fields[i, j].setPosition(new Vector2Int(i, j));
                fields[i, j].setOdd((i + j) % 2 == 0);
            }
        }
    }

    public void HandleLeftClick(BetterField field)
    {
        if (_started)
        {
            // Start the game if it hasn't started already
            FieldGenerator.CreateMines(fields, mines, field);
            FieldGenerator.CountAdjacentMines(fields);

            // Update each field
            return;
        }
    }
}
