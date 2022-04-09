using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = System.Random;

public class Game : MonoBehaviour
{
    private int _height;
    private int _width;
    private int _mines;
    
    private readonly Field[,] _fields;
    
    // The time the game has started
    private readonly Stopwatch _stopwatch = new Stopwatch();

    public void Start()
    {
        // Load all the necessary game data
        _width = 16;
        _height = 16;
        _mines = new Random().Next((_width - 1) * (_height - 1));
        
        // Start the stopwatch
        _stopwatch.Start();
    }
    
    
}
