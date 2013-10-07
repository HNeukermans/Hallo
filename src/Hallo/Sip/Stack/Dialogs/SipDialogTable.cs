using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Hallo.Sip.Stack.Dialogs
{
    public class SipDialogTable : ConcurrentDictionary<string, SipAbstractDialog>
    {
        public List<SipAbstractDialog> ToList() 
        {
            var output = new List<SipAbstractDialog>(this.Count);

            foreach (var item in this.Values)
            {
                output.Add(item);
            }         

            return output;
        }
    }
}