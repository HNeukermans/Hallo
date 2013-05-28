using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.UnitTest.Builders
{
    public abstract class ObjectBuilder<T>
    {
        public abstract T Build();
    }
}
