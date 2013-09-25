using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics.Contracts;
using Hallo.Parsers;
using Hallo.Sip.Headers;
using System.Net;
using Hallo.Util;
using NLog;
using Hallo.Util;

namespace Hallo.Sip.Stack
{
    public class SipHeaderFactory
    {
        private readonly Logger _logger =  NLog.LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, IParser<SipHeaderBase>> _parsers =
            new Dictionary<string, IParser<SipHeaderBase>>(StringComparer.CurrentCultureIgnoreCase);

        private readonly Dictionary<string, Type> _headers =
            new Dictionary<string, Type>(StringComparer.CurrentCultureIgnoreCase);
        
        public SipHeaderFactory()
        {
            _logger.Trace("Constructor called.");

            var assemblyTypes = Assembly.GetAssembly(typeof(SipHeaderFactory)).GetTypes();

            CreateParsers(assemblyTypes);
            LoadSuppportedHeaderNames(assemblyTypes);
        }

        private void LoadSuppportedHeaderNames(IEnumerable<Type> assemblyTypes)
        {
            _logger.Trace("loading all supported headers ...");

            var headerForTypes = assemblyTypes
                .Where(t => t.GetCustomAttributes(false).OfType<HeaderForAttribute>().Any()).ToList();

            foreach (Type type in headerForTypes)
            {
                var name = type.GetCustomAttributes(true).OfType<HeaderForAttribute>().First().Name;
                _headers.Add(name, type);
            }

            _logger.Trace("All supported headers loaded...");
        }

        private void CreateParsers(IEnumerable<Type> assemblyTypes)
        {
            _logger.Trace("Creating all supported parsers ...");

            var parserForTypes = assemblyTypes
                .Where(t => t.GetCustomAttributes(false).OfType<ParserForAttribute>().Any()).ToList();

            foreach (Type type in parserForTypes)
            {
                var instance = (IParser<SipHeaderBase>)Activator.CreateInstance(type);
                var name = type.GetCustomAttributes(true).OfType<ParserForAttribute>().First().HeaderName;
                _parsers.Add(name, instance);
            }

             _logger.Trace("All supported parsers created. # parsers : {0}", _parsers.Count);
        }

        public SipCSeqHeader CreateSCeqHeader(string command, int sequence)
        {
            Check.NotNullOrEmpty(command, "command");

            SipCSeqHeader h = new SipCSeqHeader();
            h.Command = command;
            h.Sequence = sequence;

            return h;
        }

        public SipFromHeader CreateFromHeader(SipAddress address, string tag)
        {
            Check.Require(address, "address");

            SipFromHeader h = new SipFromHeader();
            h.DisplayInfo = address.DisplayInfo;
            h.SipUri = address.Uri;
            h.Tag = tag;
            return h;
        }

        public SipToHeader CreateToHeader(SipAddress address, string tag = null)
        {
            Check.Require(address, "address");

            SipToHeader h = new SipToHeader();
            h.DisplayInfo = address.DisplayInfo;
            h.SipUri = address.Uri;
            h.Tag = tag;
            return h;
        }

        public SipViaHeader CreateViaHeader(IPAddress host, int? port, string transport, string branch)
        {
            Check.Require(host, "host");
            Check.NotNullOrEmpty(transport, "transport");
            Check.NotNullOrEmpty(branch, "branch");

            SipViaHeader h = new SipViaHeader();
            if(branch != null) h.Branch = branch;
            h.SentBy = new System.Net.IPEndPoint(host, port.HasValue ? port.Value : 5060);
            h.Transport = transport;
            return h;
        }

        public SipViaHeader CreateViaHeader(IPEndPoint sentBy, string transport, string branch)
        {
            return CreateViaHeader(sentBy.Address, sentBy.Port, transport, branch);
        }

        public SipCallIdHeader CreateCallIdHeader(String callId)
        {
            Check.NotNullOrEmpty(callId, "callId");

            SipCallIdHeader h = new SipCallIdHeader();
            h.Value = callId;
            return h;
        }

        internal SipMaxForwardsHeader CreatMaxForwardsHeader(int maxForwards = 70)
        {
            var h = new SipMaxForwardsHeader();
            h.Value = maxForwards;
            return h;
        }

        public SipHeaderBase CreateHeader(string name, string value)
        {
            Check.NotNullOrEmpty(name, "name");

            if(_logger.IsDebugEnabled)
                _logger.Debug("Create SipHeader for name '{0}' via parsing text '{1}' ...", name, value);

            if(!_parsers.ContainsKey(name))
            {
                throw new ParseException(string.Format(ExceptionMessage.CouldNotFindHeaderParserFormatString, name));
            }
           
            return _parsers[name].Parse(value);
        }

        public SipMaxForwardsHeader CreateMaxForwardsHeader(int hops = 70)
        {
            return new SipMaxForwardsHeader(hops);
        }
 
        public SipRouteHeader CreateRouteHeader(SipUri uri)
        {
            Check.Require(uri, "uri");

            return new SipRouteHeader() { SipUri = uri};
        }

        public SipRecordRouteHeader CreateRecordRouteHeader(SipUri uri)
        {
            Check.Require(uri, "uri");

            return new SipRecordRouteHeader() { SipUri = uri};
        }

        public SipContactHeader CreateContactHeader(SipUri uri)
        {
            Check.Require(uri, "uri");

            return new SipContactHeader() { SipUri = uri };
        }

        public SipExpiresHeader CreateExpiresHeader(int value)
        {
            return new SipExpiresHeader() { Value = value };
        }

        public Dictionary<string, Type> GetSupportedHeaders()
        {
            //TODO return a copy;
            return _headers;
        }
    }
}
