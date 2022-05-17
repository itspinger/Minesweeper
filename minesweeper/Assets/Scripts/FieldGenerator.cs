using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class FieldGenerator
{

	/**
	 * This method is used to generate a definite amount of mines withi the 
	 * fields matrix and creating an exception for a specific field which is the first
	 * field that the player clicks. 
	 */

	public static void CreateMines(Field[,] fields, int mines, Field exception)
	{
		var width = fields.GetLength(0);
		var height = fields.GetLength(1);

		// Loop through the number of mines needed
		for (var i = 0; i < mines; i++)
		{
			var m = Random.Range(0, width);
			var n = Random.Range(0, height);

			// Check if the field 
			// Can be set to a mine
			while (IsInvalid(fields, exception, m, n))
			{
				m++;

				if (m < width) 
					continue;
				
				m = 0;
				n++;

				if (n >= height)
				{
					n = 0;
				}
			}
			
			// Set the type to a mine
			fields[m, n].SetType(Field.FieldType.Mine);
		}
	}

	/**
	 * This method checks if a position of a field is valid for it to be changed to a mine.
	 * <p>
	 * The specified field is an exception, so every field adjacent to it is also an exception
	 * and cannot be set to a mine.
	 */
 
	private static bool IsInvalid(Field[,] fields, Field exception, int x, int y)
	{
		var pos = exception.GetPosition();

		// Check if it's the same field
		if (pos.x == x && pos.y == y)
		{
			return true;
		}

		// Loop through each adjacent field
		// And check if the position
		// Is equal to the adjacent field
		var adjacentFields = GetAdjacentFields(fields, pos.x, pos.y);
		foreach (var v in adjacentFields)
		{
			pos = v.GetPosition();

			// Check if it's the same position
			// If so it is invalid
			if (pos.x == x && pos.y == y)
			{
				return true;
			}
		}

		return fields[x, y].IsMine();
	}

	/**
	 * 
	 */

	public static void CountAdjacentMines(Field[,] fields)
	{
		for (var i = 0; i < fields.GetLength(0); i++)
		{
			for (var j = 0; j < fields.GetLength(1); j++)
			{
				var field = fields[i, j];
				
				// Check if it's a mine
				if (field.GetFieldType() == Field.FieldType.Mine)
					continue;

				var count = CountAdjacent(fields, i, j);
				fields[i, j].SetAdjacentMines(count);
			}
		}
	}

	private static int CountAdjacent(Field[,] fields, int x, int y)
	{
		return GetAdjacentFields(fields, x, y).Count(field => field.GetFieldType() == Field.FieldType.Mine);
	}
	
	private static IEnumerable<Field> GetAdjacentFields(Field[,] fields, int x, int y)
	{
		var adjacent = new List<Field>();
		
		var width = fields.GetLength(0);
		var height = fields.GetLength(1);
		
		for (var i = Math.Max(0, x - 1); i <= Math.Min(x + 1, width - 1); i++)
		{
			for (var j = Math.Max(0, y - 1); j <= Math.Min(y + 1, height - 1); j++)
			{
				adjacent.Add(fields[i, j]);
			}
		}

		return adjacent;
	}
}
