using System;
using System.Collections.Generic;
using System.Linq;
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
		var adjacentFields = exception.GetAdjacentFields();
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



	public static void CountAdjacentMines(Field[,] fields)
	{
		foreach (var field in fields)
        {
			if (field.IsMine())
				return;

			field.SetAdjacentMines(field.GetAdjacentFields()
				.Count(field => field.IsMine()));
        }
	}
}
