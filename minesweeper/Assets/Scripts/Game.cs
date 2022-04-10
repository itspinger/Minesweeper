using System;
using System.Diagnostics;
using UnityEngine;
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
    }

    public void Start()
    {
        // Load all the necessary game data
        _width = 32;
        _height = 16;
        _mines = new Random().Next((_width - 1) * (_height - 1));

        Camera.main.transform.position = new Vector3(_width / 2, _height * 0.6f, -10);
        Camera.main.orthographicSize = Math.Max(_height, _width) / 2f * 1.43f;

        _fields = new Field[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _fields[i, j] = new Field(Field.FieldType.Default, new Vector3Int(i, j, 0), 4);
                _tileManager.UpdateField(_fields[i, j]);
            }
        }
    }
    
    
}
