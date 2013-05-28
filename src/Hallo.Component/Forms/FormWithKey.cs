using System.Windows.Forms;

namespace Hallo.Component.Forms
{
    /// <summary>
    /// The base form. T respresent the type of its parent form.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FormWithKey : Form 
    {
        public string Key { get; set; }
        
    }
}