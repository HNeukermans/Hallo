using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Hallo.Component.Logic
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        #region property changed

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        protected void RaisePropertyChanged(Expression<Func<Object>> expression)
        {
            if (expression == null) return;
            var methodExpression = expression.Body as MemberExpression;
            if (methodExpression == null && expression.Body is UnaryExpression)
            {
                methodExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            RaisePropertyChanged(methodExpression.Member.Name);
           
        }

        #endregion
    }
}