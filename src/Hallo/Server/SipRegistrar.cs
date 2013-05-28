using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using Hallo.Sip;
using Hallo.Sip.Headers;
using Hallo.Sip.Stack;
using Hallo.Sip.Util;
using NLog;

namespace Hallo.Server
{
    public class SipRegistrar : ISipRequestHandler
    {
        public int MinimumExpires { get; set; }
        
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly SipRegistrarSettings _settings;
        private readonly ISipAdressBindingService _addressBindingService;

        public SipRegistrar(SipRegistrarSettings settings, ISipAdressBindingService addressBindingService)
        {
            _settings = settings;
            _addressBindingService = addressBindingService;
        }

        public SipRegistrar(SipRegistrarSettings settings) :this(settings, new InMemoryAddressBindingService())
        {
        }

        public void ProcessRequest(SipRequestEvent requestEvent)
        {
            var request = requestEvent.Request;
            var requestLine = request.RequestLine;
            var toUri = request.To.SipUri;
            var aor = toUri.FormatToString();
            var newContacts = request.Contacts.ToList();

            if (requestLine.Method == SipMethods.Register)
            {
                if (!string.IsNullOrWhiteSpace(requestLine.Uri.User))
                {
                    /*throw invalid exception bc the user portion for a register request must be empty*/
                    throw new SipException(SipResponseCodes.x400_Bad_Request, "The sip uri can not contain a user.");
                }

                /*10.3. 1. The registrar inspects the Request-URI to determine whether it
                 has access to bindings for the domain identified in the
                 Request-URI.  If not, and if the server also acts as a proxy
                 server, the server SHOULD forward the request to the addressed
                 domain, following the general behavior for proxying messages
                 described in Section 16.*/

                /*The registrar inspects the Request-URI to determine whether it
                 has access to bindings for the domain identified in the Request-URI. 
                 Here we give an exception back to the client because we only supports single domain */

                if (requestLine.Uri.Host != _settings.Domain)
                {
                    throw new SipException(SipResponseCodes.x404_Not_Found);
                }

                if (toUri.Host != _settings.Domain)
                {
                    /*If the address-of-record is not valid for the domain in the Request-URI, 
                     * the registrar MUST send a 404 (Not Found) response and skip the remaining steps.*/
                    throw new SipException(SipResponseCodes.x404_Not_Found);
                }

                bool hasWildCard = newContacts.Any(c => c.IsWildCard);
                if (hasWildCard)
                {
                    if (IsInvalidWildCardRequest(request))
                    {
                        throw new SipException(SipResponseCodes.x400_Bad_Request);
                    }

                    /*case: deregister: remove all bindings for the aor */
                    foreach (var binding in _addressBindingService.GetByAddressOfRecord(aor))
                    {
                        if (binding.CallId != request.CallId.Value)
                        {
                            _addressBindingService.Remove(binding);
                            continue;
                        }
                        if (request.CSeq.Sequence > binding.CSeq)
                        {
                            _addressBindingService.Remove(binding);
                        }
                    }
                }
                /*The registrar extracts the address-of-record from the To header
                    field of the request. If the address-of-record is not valid
                    for the domain in the Request-URI, the registrar MUST send a
                    404 (Not Found) response and skip the remaining steps.  The URI
                    MUST then be converted to a canonical form.  To do that, all
                    URI parameters MUST be removed (including the user-param), and
                    any escaped characters MUST be converted to their unescaped
                    form.  The result serves as an index into the list of bindings.*/
                /* Each binding record records the Call-ID and CSeq values from the request.*/
                foreach (var contact in newContacts)
                {
                    if (!SipUtil.IsIPAddress(contact.SipUri.Host))
                    {
                        throw new SipException(SipResponseCodes.x400_Bad_Request, "Contact uri must have numeric IP format.");
                    }
                    var host = new IPEndPoint(SipUtil.ParseIpAddress(contact.SipUri.Host), contact.SipUri.Port);

                    int expires = _settings.DefaultExpires;
                    if (request.Expires != null) expires = request.Expires.Value;
                    if (contact.Expires.HasValue) expires = contact.Expires.Value;

                    var newAddressBinding =
                          new SipAddressBinding(
                              aor,
                              host,
                              request.CSeq.Sequence,
                              request.CallId.Value,
                              expires);

                    if(expires == 0)
                    {
                        _addressBindingService.Remove(newAddressBinding);
                    }
                    else if (expires < _settings.MinimumExpires)
                    {
                        throw new SipException(SipResponseCodes.x423_Interval_Too_Brief);
                    }
                    else
                    {
                        _addressBindingService.AddOrUpdate(newAddressBinding);
                    }
                }
               
                var okResponse = request.CreateResponse(SipResponseCodes.x200_Ok);
                var contactsAddresses = new List<SipContactHeader>();

                foreach(var binding in _addressBindingService.GetByAddressOfRecord(aor))
                {
                    var contactAddress = new SipContactHeader();
                    contactAddress.Expires = (int) (binding.EndTime - DateTime.Now).TotalSeconds;
                    contactAddress.SipUri = new SipUri()
                    {
                        Host = binding.Host.Address.ToString(),
                        Port = binding.Host.Port
                    };
                    contactsAddresses.Add(contactAddress);
                }

                contactsAddresses.ForEach(okResponse.Contacts.Add);
                
                /*set the response*/
                if(requestEvent.ServerTransaction != null)
                {
                    requestEvent.ServerTransaction.SendResponse(okResponse);
                    requestEvent.IsHandled = true;
                    requestEvent.IsSent = true;
                }
                else
                {
                    requestEvent.Response = okResponse;
                    requestEvent.IsHandled = true;
                }
            }
        
    }

        private bool IsInvalidWildCardRequest(SipRequest request)
        {
            if (request.Expires == null || request.Expires.Value > 0) return true;

            if (request.Contacts.Count > 1) return true;

            return false;
        }

        public IObservable<SipAddressBindingDiagnosticsInfo> ObserveDiagnostics()
        {
            var inMemoryAbs = _addressBindingService as InMemoryAddressBindingService;
            
            return inMemoryAbs.ObserveDiagnostics();
        }
    }
}