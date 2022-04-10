using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldGenerator
{

	public FieldGenerator(Field[,] fields, int mines, Field exception)
	{
		CreateMines(fields, mines, exception);
		CountAdjacent(fields);
	}

	public static void CreateDefaultFieldTable(Field[,] fields, TileManager tileManager)
	{
		for (var i = 0; i < fields.GetLength(0); i++)
		{
			for (var j = 0; j < fields.GetLength(1); j++)
			{
				fields[i, j] = new Field(new Vector3Int(i, j, 0));
				tileManager.UpdateField(fields[i, j]);
			}
		}
	}
	
	private void CreateMines(Field[,] fields, int mines, Field exception)
	{
		var width = fields.GetLength(0);
		var height = fields.GetLength(1);

		for (var i = 0; i < mines; i++)
		{
			var m = Random.Range(0, width);
			var n = Random.Range(0, height);

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
			
			fields[m, n].SetType(Field.FieldType.Mine);
		}
	}

	private bool IsInvalid(Field[,] fields, Field exception, int x, int y)
	{
		var pos = exception.GetPosition();
		
		// Check if it's the same field
		if (pos.x == x && pos.y == y)
		{
			return true;
		}
		
		var adjacentFields = GetAdjacentFields(fields, pos.x, pos.y);
		foreach (var v in adjacentFields)
		{
			pos = v.GetPosition();
			
			// Check if it's the same position
			if (pos.x == x && pos.y == y)
			{
				return true;
			}
		}
		
		return fields[x, y].IsMine();
	}

	private void CountAdjacent(Field[,] fields)
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

	private IEnumerable<Field> GetAdjacentFields(Field[,] fields, int x, int y)
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

	private int CountAdjacent(Field[,] fields, int x, int y)
	{
		return GetAdjacentFields(fields, x, y).Count(field => field.GetFieldType() == Field.FieldType.Mine);
	}
	
}
