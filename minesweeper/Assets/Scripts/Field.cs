using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
	private FieldType type = FieldType.Default;
	private FieldState state = FieldState.Hidden;

	private int adjacentMines;
	private bool odd;
	private Vector2Int position;
	private FieldController controller;

	public ClickableButton clickableButton;

    private void Awake()
    {
		// Get the controller from the gameObject
		controller = GetComponent<FieldController>();

		// Attaches the left click and right click listener
		// To the game manager
		clickableButton.OnLeftClick.AddListener(() => Game.GetInstance().HandleLeftClick(this));
        clickableButton.OnRightClick.AddListener(() => Game.GetInstance().HandleRightClick(this));
    }

    /**
	 * This method returns a field adjacent
	 * to this field by a certain vector.
	 */

    public Field GetField(Vector2Int position)
    {
		return Game.GetInstance().GetField(this, position);
    }

	public List<Field> GetAdjacentFields()
    {
        var fields = new List<Field>();

		// Loop through each field
		// And find adjacent fields
        for (int i = -1; i <= 1; i++)
        {
			for (int j = -1; j <= 1; j++)
            {
				Field field = this.GetField(new Vector2Int(i, j));

				// Check for null value
				if (field == null)
                {
					continue;
                }

				// Add to the fields
				fields.Add(field);
            }
        }

		return fields;
    }

	/**
	 * This method reveals the field.
	 */

    public void Reveal()
    {
		if (GetState() != FieldState.Hidden)
			return;

		// Reveal the field
		SetState(Field.FieldState.Revealed);
		controller.Reveal();
    }

	/**
	 * This method flags the field.
	 */

	public void Flag()
	{
		if (GetState() == FieldState.Revealed)
			return;

		if (!Game.GetInstance().HasEnded())
		{
			SetState(GetState() == Field.FieldState.Flagged ? Field.FieldState.Hidden : Field.FieldState.Flagged);
		}

		// Flag from the controller;
		controller.Flag();
	}

	/**
	 * This method checks whether this field
	 * is odd or not.
	 */

	public bool IsOdd()
    {
		return odd;
    }

	/*
	 * This method returns whether this field
	 * is a mine.
	 */

	public bool IsMine()
	{
		return type == FieldType.Mine;
	}

	/**
	 * This method sets whether the field should 
	 * be odd or not.
	 * 
	 * <p>
	 * This type of field is used by the field controller
	 * which controls the look of the prefab.
	 */

	public void setOdd(bool odd)
	{
		this.odd = odd;
	}

	/**
	 * This method changes the current state of this field
	 * to the one specified in the arguments.
	 */

	public void SetState(FieldState state)
	{
		this.state = state;
	}

	/**
	 * This method changes the type of this field.
	 */

	public void SetType(FieldType type)
	{
		this.type = type;
	}

	/**
	 * This method sets the amount of adjacent mines to this field.
	 */

	public void SetAdjacentMines(int adjacentMines)
	{
		this.adjacentMines = adjacentMines;
	}

	/**
	 * This method returns the amount of mines adjacent
	 * to this field
	 */

	public int GetAdjacentMines()
	{
		return adjacentMines;
	}

	/*
	 * This method returns the field type of this field.
	 * 
	 * If the type is a mine, upon clicking on the field
	 * the game will be forced to end.
	 */

	public FieldType GetFieldType()
	{
		return type;
	}

	/**
	 * This method sets the position of this field.
	 * Do note that this position, cannot be changed once already set.
	 */

	public void setPosition(Vector2Int position)
    {
		this.position = position;
    }

	/*
	 * This method returns the state of this field.
	 */

	public FieldState GetState()
	{
		return state;
	}

	/**
	 * This method returns the vector position of this field.
	 */

	public Vector2Int GetPosition()
    {
		return this.position;
    }

	/**
	 * This enum is used to represent the field type.
	 *
	 * There are only 2 allowed states of any field: Default, which corresponds
	 * to the field which isn't a mine, and a Mine field.
	 */
	public enum FieldType
	{
		Default,
		Mine,
		Unknown
	}

	/**
	 * This enum represents the current state of this field,
	 * which is changed when the field is clicked, or when the field
	 * is being flooded as the reason of clicking another field.
	 *
	 * The default state for each field is Hidden.
	 */

	public enum FieldState
	{
		Revealed,
		Hidden,
		Flagged,
		Unknown
	}
}