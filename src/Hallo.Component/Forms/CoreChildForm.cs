namespace Hallo.Component.Forms
{
    /// <summary>
    /// Serves as the childform that can be extended within this project. 
    /// </summary>
    /// <remarks>
    /// In windows forms a form can only extend a non generic typed form. 
    /// This form is designer compatible bc it is not generic.
    /// </remarks>
    public class CoreChildForm : ChildForm<CoreMainForm>
    {
    }
}