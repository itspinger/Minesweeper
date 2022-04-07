/**
 * This class represents a field within the game table. Every
 * field has a set FieldType which determines whether the field
 * is a mine or not.
 *
 * Written by Dimitrije Mijailovic
 */

public class Field
{
	private readonly FieldType _type;
	private FieldState _state = FieldState.Hidden;

	public Field(FieldType type)
	{
		_type = type;
	}

	/*
	 * This method returns whether this field
	 * is a mine.
	 */
	
	public bool IsMine()
	{
		return _type == FieldType.Mine;
	}

	public void SetState(FieldState state)
	{
		_state = state;
	}
	
	/*
	 * This method returns the field type of this field.
	 * 
	 * If the type is a mine, upon clicking on the field
	 * the game will be forced to end.
	 */

	public FieldType GetFieldType()
	{
		return _type;
	}

	/*
	 * This method returns the state of this field.
	 */
	
	public FieldState GetState()
	{
		return _state;
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
