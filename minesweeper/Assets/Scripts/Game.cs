using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Game : MonoBehaviour
{
    private int _height;
    private int _width;
    private int _mines;
    
    private readonly Field[,] _fields;

    public void Start()
    {
        // Load all the necessary game data
        _width = 16;
        _height = 16;
        _mines = new Random().Next((_width - 1) * (_height - 1));
    }
}
