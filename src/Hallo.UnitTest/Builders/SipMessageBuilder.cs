using System;
using System.Collections.Generic;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.UnitTest.Helpers;

namespace Hallo.UnitTest.Builders
{
    internal class SipMessageBuilder<T> : ObjectBuilder<T> where T : SipMessage, new ()
    {
        private SipFromHeader _from;
        private SipToHeader _to;
        private SipCSeqHeader _cSeq;
        private SipCallIdHeader _callId;
        private SipHeaderList<SipViaHeader> _vias;
        private SipMaxForwardsHeader _maxForwards;
        private SipContentLengthHeader _contentLength;
        private SipContentTypeHeader _contentType;
        private SipSubjectHeader _subject;
        private Byte[] _body;
        private SipHeaderList<SipContactHeader> _contacts;
        private SipHeaderList<SipRecordRouteHeader> _recordRoutes;

        public SipMessageBuilder()
        {
            _from = new SipFromHeaderBuilder().Build();
            _to = new SipToHeaderBuilder().Build();
            _cSeq = new SipCSeqHeaderBuilder().Build();
            _callId = new SipCallIdHeaderBuilder().Build(); ;
            _vias = new SipViaHeaderListBuilder()
                .Add(new SipViaHeaderBuilder().Build())
                .Add(new SipViaHeaderBuilder().WithSentBy(TestConstants.IpEndPoint2).Build())
                .Build();
            _maxForwards = new SipMaxForwardsHeaderBuilder().Build();
            _contacts = new SipContactHeaderListBuilder()
                .Add(new SipContactHeaderBuilder().WithExpires(360).Build())
                .Add(new SipContactHeaderBuilder().WithExpires(360).Build())
                .Build();
            _recordRoutes = new SipRecordRouteHeaderListBuilder()
               .Add(new SipRecordRouteHeaderBuilder().WithSipUri(TestConstants.AliceProxyUri).Build())
               .Add(new SipRecordRouteHeaderBuilder().WithSipUri(TestConstants.BobProxyUri).Build())
               .Build();
            //_contentLength = new SipContentLengthHeaderBuilder().Build();
            //_contentType = new SipContentTypeHeaderBuilder().Build();
        }

        public SipMessageBuilder<T> WithFrom(SipFromHeader value)
        {
            _from = value;
            return this;
        }

        public SipMessageBuilder<T> WithTo(SipToHeader value)
        {
            _to = value;
            return this;
        }

        public SipMessageBuilder<T> WithNoHeaders()
        {
            _from = null;
            _to = null;
            _cSeq = null;
            _callId = null;
            _vias = null;
            _maxForwards = null;
            return this;
        }

        public SipMessageBuilder<T> WithCSeq(SipCSeqHeader value)
        {
            _cSeq = value;
            return this;
        }

        public SipMessageBuilder<T> WithCallId(SipCallIdHeader value)
        {
            _callId = value;
            return this;
        }

        public SipMessageBuilder<T> WithSubject(SipSubjectHeader value)
        {
            _subject = value;
            return this;
        }

        public SipMessageBuilder<T> WithBody(byte[] value)
        {
            _body = value;
            return this;
        }

        public SipMessageBuilder<T> WithVias(SipHeaderList<SipViaHeader> value)
        {
            _vias = value;
            return this;
        }

        public SipMessageBuilder<T> WithContacts(SipHeaderList<SipContactHeader> value)
        {
            _contacts = value;
            return this;
        }


        public SipMessageBuilder<T> WithRecordRoutes(SipHeaderList<SipRecordRouteHeader> value)
        {
            _recordRoutes = value;
            return this;
        }


        public SipMessageBuilder<T> WithMaxForwards(SipMaxForwardsHeader value)
        {
            _maxForwards = value;
            return this;
        }

        public SipMessageBuilder<T> WithContentLength(int value)
        {
            _contentLength = new SipContentLengthHeader {Value = value};
            return this;
        }

        public SipMessageBuilder<T> WithContentType(SipContentTypeHeader value)
        {
            _contentType = value;
            return this;
        }

	
        public override T Build()
        {
            T item = new T
                         {
                             From = _from,
                             To = _to,
                             CSeq = _cSeq,
                             CallId = _callId,
                             MaxForwards = _maxForwards,
                             ContentLength = _contentLength,
                             ContentType = _contentType,
                             Subject = _subject,
                             Body = _body,
                         };

            _contacts.ToList().ForEach(item.Contacts.Add);
            _vias.ToList().ForEach(item.Vias.Add);
            _recordRoutes.ToList().ForEach(item.RecordRoutes.Add);

            return item;
        }

        
    }
}