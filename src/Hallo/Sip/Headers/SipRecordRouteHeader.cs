namespace Hallo.Sip.Headers
{
    [HeaderFor(SipHeaderNames.RecordRoute)]
    public class SipRecordRouteHeader : SipRouteHeaderBase<SipRecordRouteHeader>
    {
        internal SipRecordRouteHeader():base()
        {
            Name = SipHeaderNames.RecordRoute;
        }
    }
}