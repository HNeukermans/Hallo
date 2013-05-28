using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hallo.Util
{
    public class ObjectUtil
    {
        public static bool ComparePrimitiveProperties<T>(T o1, T o2) where T : class
        {
            if (object.ReferenceEquals(o1, o2)) return true; 
            if (o1 != null && o2 == null) return false;
            if (o2 != null && o1 == null) return false;

            foreach (PropertyInfo property in typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && (p.PropertyType.IsPrimitive || p.PropertyType.IsValueType)))
            {
                var value1 = property.GetValue(o1, null);
                var value2 = property.GetValue(o2, null);

                if (!object.Equals(value1, value2)) return false;
            }

            return true;
        }
    }
}
