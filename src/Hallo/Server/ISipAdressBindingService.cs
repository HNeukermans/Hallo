using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Hallo.Server
{
   public interface ISipAdressBindingService
   {
       void AddOrUpdate(SipAddressBinding addressBinding);

       void Remove(SipAddressBinding addressBinding);

       List<SipAddressBinding> GetByAddressOfRecord(string addressOfRecord);
   }
}