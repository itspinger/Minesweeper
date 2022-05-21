using UnityEngine;
using UnityEngine.UI;

public class FieldBorderContext : FieldContext
{
    private Image[] images;

    private readonly Image leftBorder;
    private readonly Image rightBorder;
    private readonly Image topBorder;
    private readonly Image bottomBorder;

    public FieldBorderContext(FieldController controller, Image[] images) : base(controller)
    {
        this.images = images;

        leftBorder = images[0];
        rightBorder = images[1];
        topBorder = images[2]; 
        bottomBorder = images[3];
    }

    public override void Apply()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }

        // Get the field
        var currentField = controller.GetField();

        // Check if it is a mine
        if (currentField.IsMine())
            return;

        // Check if this field is a flagged
        // Or hidden
        if (currentField.GetState() == Field.FieldState.Flagged || currentField.GetState() == Field.FieldState.Hidden)
        {
            return;
        }

        // Check for adjacent fields
        var fields = Game.GetInstance().GetAdjacentFields(currentField);

        // Loop through each field
        foreach (var field in fields)
        {

            // If the field is revealed already
            // We do not want the border
            if (field.GetState() == Field.FieldState.Revealed)
                continue;

            // Apply the text from the position
            this.ApplyFromPosition(currentField.GetPosition(), field.GetPosition());
        }
    }

    private void ApplyFromPosition(Vector2Int currentPosition, Vector2Int position)
    {
        if (position.x - currentPosition.x == 1)
        {
            bottomBorder.gameObject.SetActive(true);
            return;
        }

        if (position.x - currentPosition.x == -1)
        {
            topBorder.gameObject.SetActive(true);
            return;
        }

        if (position.y - currentPosition.y == 1)
        {
            rightBorder.gameObject.SetActive(true);

            // Set length to 74 if diagonal is 
            Field field = controller.GetField().GetField(new Vector2Int(-1, 1));
            if (field == null)
            {
                return;
            }

            Debug.Log("I am at the postition " + controller.GetField().GetPosition());
            Debug.Log("Looking at field " + field.GetPosition());
            Debug.Log("State of the field: " + field.GetState());

            if (field.GetState() != Field.FieldState.Revealed)
            {
                return;
            }

            if (field.IsMine())
            {
                return;
            }

            rightBorder.rectTransform.localPosition = new Vector3(rightBorder.rectTransform.localPosition.x, 2, 0);
            return;
        }

        leftBorder.gameObject.SetActive(true);
    }
}
