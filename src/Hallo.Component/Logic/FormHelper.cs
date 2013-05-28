using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace Hallo.Component.Logic
{
    public class FormHelper
    {
        public static void ValidateIsNotEmpty(string value, string field)
        {
            ValidateCondition(String.IsNullOrWhiteSpace(value), field);
        }

        public static void ValidateCondition(bool condition, string field)
        {
            if (!condition)
            {
                throw new ValidationException(field + " is not valid.");
            }
        }

        
        public void InvokeOnUiThread(Control control, Action<Control> action)
        {
            if(control.InvokeRequired)
            {
                control.Invoke(action);
            }
        }
    }
}
