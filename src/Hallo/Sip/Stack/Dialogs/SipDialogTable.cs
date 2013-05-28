using System.Collections.Concurrent;

namespace Hallo.Sip.Stack.Dialogs
{
    public class SipDialogTable : ConcurrentDictionary<string, SipAbstractDialog>
    {
         
    }
}