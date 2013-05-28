using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Sip.Headers;
using Hallo.Sip.Util;
using Hallo.Util;

namespace Hallo.Sip
{
    public abstract class SipMessage
    {
        private SipFromHeader _from;
        private SipCallIdHeader _callId;
        private SipCSeqHeader _sceq;
        private SipContentLengthHeader _contentLenght;
        private SipExpiresHeader _expires;
        private SipContentTypeHeader _contentType;
        private SipToHeader _to;
        private SipHeaderList<SipViaHeader> _vias;
        private SipHeaderList<SipRouteHeader> _routes;
        private SipHeaderList<SipRecordRouteHeader> _recordRoutes;
        private SipHeaderList<SipContactHeader> _contacts;
        private SipMaxForwardsHeader _maxForwards;
        private SipSubjectHeader _subject;
        private SipAllowHeader _allow;
        private SipUserAgentHeader _userAgent;
        private Byte[] _body;
        
        public Byte[] Body
        {
            get { return _body; }
            set { _body = value; }
        }


        protected SipMessage()
        {
            _vias = new SipHeaderList<SipViaHeader>();
            _routes = new SipHeaderList<SipRouteHeader>();
            _contacts = new SipHeaderList<SipContactHeader>();
            _recordRoutes = new SipHeaderList<SipRecordRouteHeader>();
        }

        public void SetHeader(SipHeaderBase header)
        {
            Check.Require(header, "header");

            switch (header.Name)
            {
                case SipHeaderNames.CSeq: _sceq = (SipCSeqHeader)header;
                    break;
                case SipHeaderNames.CallId: _callId = (SipCallIdHeader)header;
                    break;
                case SipHeaderNames.Contact: _contacts.Add((SipContactHeader)header);
                    break;
                case SipHeaderNames.ContentLength: _contentLenght = (SipContentLengthHeader)header;
                    break;
                case SipHeaderNames.ContentType: _contentType = (SipContentTypeHeader)header;
                    break;
                case SipHeaderNames.From: _from = (SipFromHeader)header;
                    break;
                case SipHeaderNames.MaxForwards: _maxForwards = (SipMaxForwardsHeader)header;
                    break;
                case SipHeaderNames.To: _to = (SipToHeader)header;
                    break;
                case SipHeaderNames.Via: _vias.Add((SipViaHeader) header);
                    break;
                case SipHeaderNames.Subject: _subject = (SipSubjectHeader)header;
                    break;
                case SipHeaderNames.Route: _routes.Add((SipRouteHeader)header);
                    break;
                case SipHeaderNames.Expires: _expires = (SipExpiresHeader)header;
                    break;
                case SipHeaderNames.RecordRoute: _recordRoutes.Add((SipRecordRouteHeader)header);
                    break;
                case SipHeaderNames.Allow: _allow = (SipAllowHeader)header;
                    break;
                case SipHeaderNames.UserAgent: _userAgent = (SipUserAgentHeader)header;
                    break;
                default:
                    throw new NotSupportedException(string.Format("The header with name {0} is not supported.", header.Name));
            }
        }
        
        public void RemoveHeader(string header)
        {
            Check.Require(header, "header");

            switch (header)
            {
                case SipHeaderNames.CSeq:
                    _sceq = null;
                    break;
                case SipHeaderNames.CallId:
                    _callId = null;
                    break;
                case SipHeaderNames.Contact:
                    _contacts.Clear();
                    break;
                case SipHeaderNames.ContentLength:
                    _contentLenght = null;
                    break;
                case SipHeaderNames.ContentType:
                    _contentType = null;
                    break;
                case SipHeaderNames.From:
                    _from = null;
                    break;
                case SipHeaderNames.MaxForwards:
                    _maxForwards = null;
                    break;
                case SipHeaderNames.To:
                    _to = null;
                    break;
                case SipHeaderNames.Via:
                    _vias.Clear();
                    break;
                case SipHeaderNames.Subject:
                    _subject =  null;;
                    break;
                case SipHeaderNames.Route:
                    _routes.Clear();
                    break;
                case SipHeaderNames.Expires:
                    _expires = null;
                    break;
                case SipHeaderNames.RecordRoute:
                    _recordRoutes.Clear();
                    break;
                case SipHeaderNames.Allow:
                    _allow = null;
                    break;
                case SipHeaderNames.UserAgent:
                    _userAgent = null;
                    break;
                default:
                    throw new NotSupportedException(string.Format("The header with name {0} is not supported.", header));
            }
        }

        public List<SipHeaderBase> GetHeaders()
        {
            var collection = new List<SipHeaderBase>();
            if (_vias.Count > 0) collection.Add(_vias);
            if (_maxForwards != null) collection.Add(_maxForwards);
            if (_from != null) collection.Add(_from);
            if (_to != null) collection.Add(_to);
            if (_callId != null) collection.Add(_callId);
            if(_sceq != null) collection.Add(_sceq);
            if (_contacts.Count > 0) collection.Add(_contacts);
            if (_contentType != null) collection.Add(_contentType);
            if (_contentLenght != null) collection.Add(_contentLenght);
            if (_subject != null) collection.Add(_subject);
            if (_routes.Count > 0) collection.Add(_routes);
            if (_expires != null) collection.Add(_expires);
            if (_recordRoutes.Count > 0) collection.Add(_recordRoutes);
            if (_allow != null) collection.Add(_allow);
            if (_userAgent != null) collection.Add(_userAgent);
            return collection;
        }

        public SipFromHeader From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        public SipSubjectHeader Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
       

        public SipToHeader To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
            
        }

        public SipCSeqHeader CSeq
        {
            get
            {
                return _sceq;
            }
            set
            {
                _sceq = value;
            }
        }

        public SipCallIdHeader CallId
        {
            get
            {
                return _callId;
            }
            set
            {
                _callId = value;
            }
        }

        public SipHeaderList<SipViaHeader> Vias
        {
            get
            {
                return _vias;
            }
        }

        public SipHeaderList<SipRouteHeader> Routes
        {
            get
            {
                return _routes;
            }
        }

        public SipHeaderList<SipContactHeader> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public SipHeaderList<SipRecordRouteHeader> RecordRoutes
        {
            get
            {
                return _recordRoutes;
            }
        }

        public SipMaxForwardsHeader MaxForwards
        {
            get { return _maxForwards; }
            set { _maxForwards = value; }
        }

        public SipContentLengthHeader ContentLength
        {
            get
            {
                return _contentLenght;
            }
            set
            {
                _contentLenght = value;
            }
            
        }

        public SipContentTypeHeader ContentType
        {
            get
            {
                return _contentType;
            }
            set
            {
                _contentType = value;
            }

        }

        public SipExpiresHeader Expires
        {
            get
            {
                return _expires;
            }
            set
            {
                _expires = value;
            }
        }

        public SipAllowHeader Allow
        {
            get
            {
                return _allow;
            }
            set
            {
                _allow = value;
            }
        }

        public SipUserAgentHeader UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }


        public static bool IsRequestHeader(SipHeaderBase sipHeader) 
        {        
            return 
                //sipHeader is SipAuthorizationHeader ||
                sipHeader is SipMaxForwardsHeader ||
                //sipHeader is SipUserAgentHeader ||
                //sipHeader is SipProxyAuthorizationHeader ||
                //sipHeader is SipRouteListHeader ||
                false;
        }

        public static bool IsResponseHeader(SipHeaderBase sipHeader)
        {
            return 
                //sipHeader is SipErrorInfoHeader || 
                //sipHeader is SipProxyAuthenticateHeader ||
                //sipHeader is SipWWWAuthenticateHeader ||
                //sipHeader is SipRSeqHeader;
                false;
        }
        
    }
}
