using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;

    // The field prefab
    public GameObject fieldPrefab;
    public RectTransform fieldTranfsorm;
    //public GameDifficulty difficulty;
    public event System.Action OnInit;

    // Game Settings
    private int rows = 8;
    private int columns = 10;
    private int mines = 20;

    private bool _started, _finished;
    private Field[,] fields;

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

        fields = new Field[rows, columns];
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
        fields = new Field[rows, columns];

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
                fields[i, j] = field.GetComponent<Field>();
                fields[i, j].setPosition(new Vector2Int(i, j));
                fields[i, j].setOdd((i + j) % 2 == 0);
            }
        }
    }

    public void HandleRightClick(Field field)
    {
        // Cancel the event if the game is finished
        if (_finished)
        {
            return;
        }

        // Perform the field statement
        field.HandleRightClick();
    }

    public void HandleLeftClick(Field field)
    {
        // Check the state of the field
        if (field.GetState() == Field.FieldState.Flagged || field.GetState() == Field.FieldState.Revealed)
            return;

        if (!_started)
        {
            // Start the game if it hasn't started already
            FieldGenerator.CreateMines(fields, mines, field);
            FieldGenerator.CountAdjacentMines(fields);

            // Update each field
            _started = true;

            // Start the timer
            TimerManager.GetInstance().StartTimer();
        }

        if (_finished)
        {
            return;
        }

        // Check if the field is a mine
        // If it is, end the game
        if (field.IsMine())
        {
            StartCoroutine(EndGame(field));
            return;
        }

        if (field.GetAdjacentMines() != 0)
        {
            field.Reveal();

            // Check for winning condition here
            return;
        }

        StartCoroutine(FloodFill(field));
        // Check win condition
    }

    public IEnumerator EndGame(Field field)
    {
        if (!field.IsMine())
        {
            yield break;
        }

        Debug.Log("Game has finished");
        _finished = true;

        // Revealed state
        field.Reveal();

        // Stop the timer
        TimerManager.GetInstance().StopTimer();

        // Reveal all others
        foreach (var f in fields)
        {
            if (f.IsMine())
            {
                f.Reveal();

                // Wait for this
                yield return new WaitForSeconds(0.15f);
            }
        }

    }

    private IEnumerator FloodFill(Field field)
    {
        if (field.GetState() == Field.FieldState.Revealed)
            yield break;

        if (field.IsMine())
            yield break;

        field.Reveal();

        // Check if field has 0 adjacent; if so, flood again to 4 corners
        if (field.GetAdjacentMines() != 0)
            yield break;

        yield return new WaitForEndOfFrame();

        foreach (var adjacentField in GetAdjacentFields(field))
        {
            StartCoroutine(FloodFill(adjacentField));
        }
    }

    public int GetMineCount()
    {
        return this.mines;
    }

    public int GetFlaggedMines()
    {
        if (fields == null)
        {
            return 0;
        }

        int count = 0;

        // Loop through each field
        foreach (var field in fields)
        {
            // We need to check for null
            // Since the game hasn't started yet
            if (field == null)
            {
                continue;
            }

            if (field.GetState() == Field.FieldState.Flagged)
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerable<Field> GetAdjacentFields(Field field)
    {
        var adjacent = new List<Field>();
        var pos = field.GetPosition();

        if (pos.x + 1 < rows)
        {
            adjacent.Add(fields[pos.x + 1, pos.y]);
        }

        if (pos.x - 1 >= 0)
        {
            adjacent.Add(fields[pos.x - 1, pos.y]);
        }

        if (pos.y + 1 < columns)
        {
            adjacent.Add(fields[pos.x, pos.y + 1]);
        }

        if (pos.y - 1 >= 0)
        {
            adjacent.Add(fields[pos.x, pos.y - 1]);
        }

        return adjacent;
    }
}
