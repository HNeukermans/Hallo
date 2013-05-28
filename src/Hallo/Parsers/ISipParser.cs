using Hallo.Util;

namespace Hallo.Parsers
{
    public interface ISipParser<out T>
    {
        T Parse(StringReader r);
        T Parse(string r);
    }
}