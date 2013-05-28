using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Sip;

namespace Hallo.Parsers
{
    //public class SipParserFactory
    //{
    //    static Dictionary<string, Type> _parsers = new Dictionary<string, Type>();

    //    static SipParserFactory()
    //    { 
    //        //_parsers = new Dictionary<string, Type>();
    //        //_parsers.Add(SipHeaderNames.To, typeof(SipToHeader));
    //        //_parsers.Add(SipHeaderNames.From, typeof(SipFromHeader));
    //        //_parsers.Add(SipHeaderNames.Via, typeof(SipViaHeader));
    //    }

    //    public ISipParser<SipHeaderBase> CreateHeaderParser(string name)
    //    {
    //        ISipParser<SipHeaderBase> parser = null;
    //        switch (name)
    //        {
    //            case SipHeaderNames.CallId: parser = new SipHeaderNameParser(name, new SipCallIdHeaderParser()); break;
    //            case SipHeaderNames.Contact: parser = new SipHeaderNameParser(name, new SipHeaderListParser(name, new SipContactHeaderParser())); break;
    //            case SipHeaderNames.ContentLength: parser = new SipHeaderNameParser(name, new SipContentLenghtHeaderParser()); break;
    //            case SipHeaderNames.ContentType: parser = new SipHeaderNameParser(name, new SipContentTypeHeaderParser()); break;
    //            case SipHeaderNames.CSeq: parser = new SipHeaderNameParser(name, new SipCSeqHeaderParser()); break;
    //            case SipHeaderNames.From: parser = new SipHeaderNameParser(name, new SipFromHeaderParser()); break;
    //            case SipHeaderNames.To: parser = new SipHeaderNameParser(name, new SipToHeaderParser()); break;
    //            case SipHeaderNames.Via: parser = new SipHeaderNameParser(name, new SipStackedHeaderListParser<SipViaHeader>(new SipViaHeaderParser())); break;
    //            case SipHeaderNames.MaxForwards: parser = new SipHeaderNameParser(name, new SipMaxForwardsHeaderParser()); break;                
                
    //            default: throw new SipParseException(string.Format(ExceptionMessage.CouldNotFindHeaderParser, name));
    //        }
    //        return parser;
    //    }
    //}
}
