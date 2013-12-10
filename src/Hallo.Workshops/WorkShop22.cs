using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Hallo.Sip;
using Hallo.Sip.Stack.Transactions;

namespace Hallo.Workshops
{
    public class WorkShop22 : WorkShopBase
    {
        private IDisposable _observer1;
        private IDisposable _observer2;

        public override void Start()
        {
            Console.WriteLine("Observing the stack for transaction events....");
            Console.WriteLine(String.Format("+{0,12}+{1,20}+{2,12}+{3,12}", "timestamp","type","request", "state"));
            Enumerable.Range(0, (12+20+12+12+4)).ToList().ForEach( c=> Console.Write("+"));
            Console.WriteLine();

            var listener = new WorkShop22Listener(_receiverProvider);
            _receiverProvider.AddSipListener(listener);
            _senderProvider.AddSipListener(listener);

            _observer1 =  _senderProvider.ObserveTxDiagnosticsInfo().Subscribe(WriteTxInfoToLog);
            _observer2 = _receiverProvider.ObserveTxDiagnosticsInfo().Subscribe(WriteTxInfoToLog);
            
            SipRequest request = CreateRequest(SipMethods.Register);

            var clientTransaction = _senderProvider.CreateClientTransaction(request);

            clientTransaction.SendRequest();
        }

        public override void Stop()
        {
            base.Stop();
            _observer1.Dispose();
            _observer2.Dispose();
        }


        private void WriteTxInfoToLog(Timestamped<SipTransactionStateInfo> info)
        {
            Console.WriteLine(String.Format("+{0,12}+{1,20}+{2,12}+{3,12}", info.Timestamp.ToString("hh:mm:ss"), info.Value.TransactionType, info.Value.Request.CSeq.Command,info.Value.CurrentState));
        }
    }
}