using Hallo.Util;
using KellermanSoftware.CompareNetObjects;

namespace Hallo.UnitTest.Helpers
{
    public class ObjectComparer
    {
         public static CompareObjects Create()
         {
             var comparer = new CompareObjects();
             comparer.AttributesToIgnore.Add(typeof(ExcludeFromEquality));
             return comparer;
         }
    }
}