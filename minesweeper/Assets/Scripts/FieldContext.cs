public abstract class FieldContext
{
    protected readonly FieldController controller;

    public FieldContext(FieldController controller)
    {
        this.controller = controller;
    }

    /**
     * This method is used to apply the context used from this class
     * to the field specified.
     * 
     * <param name="field">the field controller to apply the context to</param>
     */

    public abstract void Apply();
}
