using System;

namespace Hallo.Sip
{
    public class SipProviderDiagnosticInfo
    {
        public int PacketsReceived { get; set; }
        public int BytesReceived { get; set; }
        public int PacketsSent { get; set; }
        public int BytesSent { get; set; }
        public int ActiveThreads { get; set; }
        public int InUseThreads { get; set; }
        public int WorkItemsQueued { get; set; }
        public int WorkItemsProcessed { get; set; }
        public TimeSpan AvgExecutionTime { get; set; }
        public TimeSpan MaximumQueueWaitTime { get; set; }
        public TimeSpan MaximumExecutionTime { get; set; }
    }

    public class SipProviderTxDiagnosticInfo
    {

    }
}