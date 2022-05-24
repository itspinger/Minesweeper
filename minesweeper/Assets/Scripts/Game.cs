using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game instance;

    // The field prefab
    public GameObject fieldPrefab;
    public RectTransform fieldTranfsorm;

    // Events for different calls
    public UnityEvent OnCreate;
    public UnityEvent OnWin;
    public UnityEvent OnEnd;

    // Game Settings
    private int rows = 8;
    private int columns = 10;
    private int mines = 10;

    private bool started, finished;
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

    /**
     * This method returns a field which is next to this field
     * by a certain vector.
     */

    public Field GetField(Field field, Vector2Int position)
    {
        // Get the position of the field
        var pos = field.GetPosition(); 

        // Check if it exceeds the row count
        if (pos.x + position.x >= rows || pos.x + position.x < 0)
        {
            return null;
        }

        // Check if it exceeds the column count
        if (pos.y + position.y >= columns || pos.y + position.y < 0)
        {
            return null;
        }

        // Return the field
        return fields[pos.x + position.x, pos.y + position.y];
    }

    public IEnumerator CreateGame()
    {
        // Just delay the start a second
        yield return new WaitForEndOfFrame();

        // We need to have a new method created
        // Because the inspector doesn't recognize
        // IEnumerators as methods
        InitGame();
    }

    public void InitGame() {
        // Invoke the event
        OnCreate.Invoke();

        // Now initalize the important stuff
        InitField();

        // Set the game to not finished and not started
        started = false;
        finished = false;

        // Reset the timer
        GameManager.GetInstance().ResetTimer();
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
        fieldTranfsorm.sizeDelta = new Vector2(columns * grid.cellSize.x, rows * grid.cellSize.y);

        // Create new fields
        // For the game
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                GameObject field = Instantiate(fieldPrefab, fieldTranfsorm);

                fields[i, j] = field.GetComponent<Field>();
                fields[i, j].setPosition(new Vector2Int(i, j));
                fields[i, j].setOdd((i + j) % 2 == 0);
            }
        }
    }

    public void SetDifficulty(TMP_Dropdown dropdown)
    {
        // Get the value of the dropdown
        int value = dropdown.value;

        if (value == 0)
        {
            SetDifficulty(8, 10, 10);
            return;
        }

        if (value == 1)
        {
            SetDifficulty(14, 18, 40);
            return;
        }

        SetDifficulty(20, 24, 99);
    }

    public void SetDifficulty(int rows, int columns, int mines)
    {
        this.rows = rows;
        this.columns = columns;
        this.mines = mines;

        // Start the delayed coroutine
        StartCoroutine(CreateGame());
    }

    public void HandleRightClick(Field field)
    {
        // Cancel the event if the game is finished
        if (finished)
        {
            return;
        }

        // Perform the field statement
        field.Flag();
    }

    public void HandleLeftClick(Field field)
    {
        // Check the state of the field
        if (field.GetState() != Field.FieldState.Hidden)
            return;

        if (!started)
        {
            // Generate the fields
            // Based on the first clicked field
            FieldGenerator.CreateMines(fields, mines, field);
            FieldGenerator.CountAdjacentMines(fields);

            // Update each field
            started = true;

            // Unflag all items
            // That were flagged for no reason
            foreach (var f in fields)
            {
                if (f.GetState() != Field.FieldState.Flagged)
                    continue;

                // Unflag
                f.Flag();
            }

            // Start the timer
            GameManager.GetInstance().StartTimer();
        }

        if (finished)
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
            TryWinGame();
            return;
        }

        StartCoroutine(FloodFill(field));
        TryWinGame();
    }

    public IEnumerator EndGame(Field field)
    {
        if (!field.IsMine())
        {
            yield break;
        }

        finished = true;

        // Revealed state
        field.Reveal();

        // Stop the timer
        GameManager.GetInstance().StopTimer();

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

        // Change for flagged
        foreach (var f in fields)
        {
            if (!f.IsMine() && f.GetState() == Field.FieldState.Flagged)
            {
                // Reveal the x image
                // As the guess is not correct
                f.Flag();

                // Wait just a tiny bit
                yield return new WaitForSeconds(0.05f);
            }
        }

        // Invoke end event
        OnEnd.Invoke();
    }

    private IEnumerator FloodFill(Field field)
    {
        if (field.GetState() != Field.FieldState.Hidden || field.IsMine())
            yield break;

        // Reveal the field
        field.Reveal();

        // Check if field has 0 adjacent; if so, flood again to 4 corners
        if (field.GetAdjacentMines() != 0)
            yield break;

        yield return new WaitForEndOfFrame();
        foreach (var adjacentField in field.GetAdjacentFields())
        {
            StartCoroutine(FloodFill(adjacentField));
        }
    }

    private void TryWinGame()
    {
        // Search for any fields
        // Which are not mines and uncovered
        foreach (var field in fields)
        {
            // Try to find a field that isn't a mine
            // And isn't revealed
            if (!field.IsMine() && field.GetState() != Field.FieldState.Revealed)
            {
                return;
            }
        }

        // The game is finished
        OnWin.Invoke();

        // Stop the timer
        GameManager.GetInstance().StopTimer();

        // Set the flag to finished
        finished = true;
    }

    public bool HasEnded()
    {
        return this.finished;
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
            // Since the game might not have started yet
            if (field == null)
            {
                continue;
            }

            // Check for fields
            // That are flagged
            if (field.GetState() == Field.FieldState.Flagged)
            {
                count++;
            }
        }

        return count;
    }

    public static Game GetInstance()
    {
        return instance;
    }
}
