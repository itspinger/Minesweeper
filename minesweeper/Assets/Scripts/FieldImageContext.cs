public class FieldImageContext : FieldContext
{
    public FieldImageContext(FieldController controller) : base(controller)
    {
    }

    public override void Apply()
    {
        // Get the field from the controller
        Field field = controller.GetField();

        // Check if the field has been flagged
        if (field.GetState() == Field.FieldState.Flagged)
        {
            if (!Game.GetInstance().HasEnded())
            {
                controller.flagImage.gameObject.SetActive(true);
                return;
            }

            // The game has ended
            // We need to check for the x image
            if (!controller.flagImage.gameObject.activeSelf)
            {
                return;
            }

            controller.flagImage.gameObject.SetActive(false);
            controller.xImage.gameObject.SetActive(true);
            return;
        }

        // If it's hidden
        // Make sure to disable the flag image
        // Because it may be enabled from the previous click
        if (field.GetState() == Field.FieldState.Hidden)
        {
            controller.flagImage.gameObject.SetActive(false);
            return;
        }

        // The state is revealed 
        // Check for other images
        if (field.IsMine())
        {
            // Check if the flag image is enabled
            // If so, we shouldn't remove the flag
            if (controller.flagImage.gameObject.activeSelf)
            {
                return;
            }

            controller.mineImage.color = RandomColorPicker.GetRandomColor();
            controller.mineImage.gameObject.SetActive(true);
            return;
        }
    }
}
