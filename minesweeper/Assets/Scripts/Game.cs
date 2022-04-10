using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class Game : MonoBehaviour
{
    private int _height;
    private int _width;
    private int _mines;
    
    private Field[,] _fields;
    private TileManager _tileManager;
    
    // The time the game has started
    private readonly Stopwatch _stopwatch = new Stopwatch();

    private void Awake()
    {
        _tileManager = GetComponentInChildren<TileManager>();
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        // Load all the necessary game data
        _width = 9;
        _height = 9;
        _mines = 10;
        
        Camera.main.transform.position = new Vector3(_width / 2, _height * 0.6f, -10);
        Camera.main.orthographicSize = Math.Max(_height, _width) / 2f * 1.43f;

        _fields = new Field[_width, _height];
        FieldGenerator.CreateDefaultFieldTable(_fields, _tileManager);
    }

    public void Update()
    {
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
        if (field == null)
            return;
        
        // We generate mines after first click
        // Because we don't want the chance of the player
        // Losing within the first click
        var fieldGenerator = new FieldGenerator(_fields, _mines, field);
        _tileManager.UpdateFields(_fields);
    }
    
    private void HandleRightClick()
    {
        var field = GetFieldFromMouse();

        // This means that the field is invalid
        if (field == null)
            return;
        
        // These states are not updated within the right click
        if (field.GetState() == Field.FieldState.Revealed || field.GetState() == Field.FieldState.Unknown)
            return;
        
        field.SetState(field.GetState() == Field.FieldState.Flagged ? Field.FieldState.Hidden : Field.FieldState.Flagged);
        _tileManager.UpdateField(field);
    }
    
}
