using Hallo.Util;

namespace Hallo.Parsers
{
    public interface IParser<out T>
    {
        T Parse(StringReader r);
        T Parse(string r);
    }
}