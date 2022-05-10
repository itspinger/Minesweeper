using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

public class Game : MonoBehaviour
{
    // The general settings of the game 
    private int _height;
    private int _width;
    private int _mines;
    
    private Field[,] _fields;
    private TileManager _tileManager;

    private bool _clicked = false;
    private bool _gameOver = false;
    
    // The time the game has started
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public TMP_Text timer;

    private void Awake()
    {
        _tileManager = GetComponentInChildren<TileManager>();
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        // Start the timer
        _stopwatch.Start();

        // Load all the necessary game data
        _width = 9;
        _height = 9;
        _mines = 10;
        
        // The calls below transform the main camera
        // So that no matter the size of the game
        // The table will be centered
        Camera.main.transform.position = new Vector3(_width / 8f, _height / 8f, -10);
        Camera.main.orthographicSize = Math.Max(_height, _width) / 4f * 1.43f;

        // 
        _fields = new Field[_width, _height];
        FieldGenerator.CreateDefaultFieldTable(_fields, _tileManager);
    }

    public void Update()
    {
        // Set the text
        timer.SetText(Math.Round(_stopwatch.Elapsed.TotalSeconds, 1).ToString());
        _tileManager.UpdateFields(_fields);

        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
    }

    private Field GetFieldFromMouse()
    {
        var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouse = _tileManager.GetTilemap().WorldToCell(world);

        if (mouse.x < 0 || mouse.x >= _width || mouse.y < 0 || mouse.y >= _height)
        {
            return null;
        }

        return _fields[mouse.x, mouse.y];
    }

    private void HandleLeftClick()
    {
        var field = GetFieldFromMouse();

        // This means that the field is invalid
        // Which basically means that it is outside of the scope
        // Of the Grid
        if (field == null)
            return;

        if (!_clicked)
        {
            // We generate mines after first click
            // Because we don't want the chance of the player
            // Losing within the first click
            FieldGenerator.CreateMines(_fields, _mines, field);
            FieldGenerator.CountAdjacentMines(_fields);
            _tileManager.UpdateFields(_fields);

            // Update the click
            _clicked = true;
        }

        // Check if the game already ended
        if (_gameOver)
        {
            return;
        }

        // This event has already been called for this field
        if (field.GetState() == Field.FieldState.Flagged || field.GetState() == Field.FieldState.Revealed)
            return;

        if (field.IsMine())
        {
            StartCoroutine(EndGame(field));
            return;
        }

        if (field.GetAdjacentMines() != 0)
        {
            field.SetState(Field.FieldState.Revealed);
            _tileManager.UpdateField(field);
            
            // Check the winning condition
            return;
        }
        
        // Check win condition
        StartCoroutine(FloodFill(field));
        _tileManager.UpdateFields(_fields);
    }

    private IEnumerator FloodFill(Field field)
    {
        if (field.GetState() == Field.FieldState.Revealed)
            yield break;

        if (field.IsMine())
            yield break;

        field.SetState(Field.FieldState.Revealed);
        var pos = field.GetPosition();
        
        // Check if field has 0 adjacent; if so, flood again to 4 corners
        if (field.GetAdjacentMines() != 0)
            yield break;

        yield return new WaitForEndOfFrame();

        foreach (var adjacentField in GetAdjacentFields(field))
        {
            StartCoroutine(FloodFill(adjacentField));
        }
    }

    private IEnumerable<Field> GetAdjacentFields(Field field)
    {
        var adjacent = new List<Field>();
        var pos = field.GetPosition();

        if (pos.x + 1 < _width)
        {
            adjacent.Add(_fields[pos.x + 1, pos.y]);
        }

        if (pos.x - 1 >= 0)
        {
            adjacent.Add(_fields[pos.x - 1, pos.y]);
        }
        
        if (pos.y + 1 < _height)
        {
            adjacent.Add(_fields[pos.x, pos.y + 1]);
        }

        if (pos.y - 1 >= 0)
        {
            adjacent.Add(_fields[pos.x, pos.y - 1]);
        }

        return adjacent;
    }

    private IEnumerator EndGame(Field field)
    {
        if (!field.IsMine())
        {
            yield break;
        }
        
        Debug.Log("Game has finished");
        _gameOver = true;
        
        // Revealed state
        field.SetState(Field.FieldState.Revealed);
        field.SetExploded(true);
        
        // Reveal all others
        foreach (var f in _fields)
        {
            if (f.GetFieldType() == Field.FieldType.Mine)
            {
                f.SetState(Field.FieldState.Revealed);

                // Wait for this
                yield return new WaitForSeconds(0.15f);
            }
        }
        
        _tileManager.UpdateFields(_fields);
    }
    
    private void HandleRightClick()
    {
        var field = GetFieldFromMouse();

        // This means that the field is invalid
        if (field == null)
            return;

        // Check if the game already ended
        if (_gameOver)
        {
            return;
        }

        // These states are not updated within the right click
        if (field.GetState() == Field.FieldState.Revealed || field.GetState() == Field.FieldState.Unknown)
            return;
        
        field.SetState(field.GetState() == Field.FieldState.Flagged ? Field.FieldState.Hidden : Field.FieldState.Flagged);
        _tileManager.UpdateField(field);
    }
    
}
