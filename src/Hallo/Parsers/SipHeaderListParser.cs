using Hannes.Net.Sip;
using Hannes.Net.Sip.Headers;
using Hannes.Net.Util;

namespace Hannes.Net.Parsers
{
    //public class SipHeaderListParser : SipParser<SipHeaderBase>
    //{
    //    private SipHeaderList _list;
    //    private ISipParser<SipHeaderBase> _realParser;

    //    public SipHeaderListParser(string name, ISipParser<SipHeaderBase> realParser)
    //    {
    //        _realParser = realParser;
    //        _list = new SipHeaderList(name);
    //    }

    //    public  override SipHeaderBase Parse(StringReader r)
    //    {
    //        while (r.Available > 0)
    //        {
    //            r.ReadToFirstChar();
    //            // we have COMMA.
    //            if (r.StartsWith(","))
    //            {
    //                r.ReadSpecifiedLength(1);
    //            }

    //            // Allow xxx-param to pasre 1 value from reader.
    //            SipHeaderBase param = _realParser.Parse(r);
    //            _list.Add(param);
    //        }
    //        return _list;
    //    }
    //}


    //public class SipStackedHeaderListParser<T> : SipParser<SipStackedHeaderList<T>> where T : SipHeaderBase
    //{
    //    private SipStackedHeaderList<T> _list;
    //    private ISipParser<SipHeaderBase> _realParser;

    //    public SipStackedHeaderListParser(ISipParser<SipHeaderBase> realParser)
    //    {
    //        _realParser = realParser;
    //        _list = new SipStackedHeaderList<T>();
    //    }

    //    public override SipStackedHeaderList<T> Parse(StringReader r)
    //    {
    //        while (r.Available > 0)
    //        {
    //            r.ReadToFirstChar();
    //            // we have COMMA.
    //            if (r.StartsWith(","))
    //            {
    //                r.ReadSpecifiedLength(1);
    //            }

    //            // Allow xxx-param to pasre 1 value from reader.
    //            SipHeaderBase param = _realParser.Parse(r);
    //            _list.Add(param);
    //        }
    //        return _list;
    //    }
    //}
}