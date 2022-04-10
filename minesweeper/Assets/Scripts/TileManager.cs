using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
	private Tilemap _tilemap;
	public List<Tile> tiles;

	public void Awake()
	{
		_tilemap = GetComponent<Tilemap>();
	}
	
	/**
	 * This method updates the tile for a certain game field
	 * by looking at the certain properties of the object.
	 */

	public void UpdateField(Field field)
	{
		_tilemap.SetTile(field.GetPosition(), GetTile(field));
	}
	
	/**
	 * This method updates a 2d array of fields by changing the tile
	 * displayed.
	 */

	public void UpdateFields(Field[,] fields)
	{
		foreach (var field in fields)
		{
			UpdateField(field);
		}
	}
	
	/**
	 * This method returns the appropriate tile for the specified
	 * field given it's properties.
	 */
	
	private Tile GetTile(Field field)
	{
		if (field.GetState() == Field.FieldState.Flagged)
		{
			return GetTile("Flag");
		}

		if (field.GetState() == Field.FieldState.Hidden)
		{
			return GetTile("Hidden");
		}

		// This means that the state is revealed
		if (field.GetFieldType() == Field.FieldType.Default)
		{
			return GetTile(field.GetAdjacentMines());
		}

		return field.HasExploded() ? GetTile("ExplodedMine") : GetTile("Mine");
	}
	
	/**
	 * This method returns a tile with a specific name. If the tile
	 * was not found, this method will return null.
	 */

	private Tile GetTile(string tileName)
	{
		return tiles.FirstOrDefault(tile => tile.name.Equals(tileName));
	}
	
	/// This method returns a tile with it's integer representation.
	private Tile GetTile(int tile)
	{
		return GetTile(tile.ToString());
	}
	
	/**
	 * This method returns the tilemap
	 * of this manager.
	 */

	public Tilemap GetTilemap()
	{
		return _tilemap;
	}
}