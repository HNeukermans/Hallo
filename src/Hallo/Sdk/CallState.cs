namespace Hallo.Sdk
{
    public enum CallState
    {
        /// <summary>
        ///  the call parties are talking
        /// </summary>
        InCall,

        /// <summary>
        /// the phone was hanged up
        /// </summary>
        Completed,

        /// <summary>
        /// an error occurred during the call initialization
        /// </summary>
        Error,

        /// <summary>
        /// the softphone is ringing (incoming call)
        /// </summary>
        Ringing,

        /// <summary>
        /// the callee's phone is ringing (outgoing call)
        /// </summary>
        Ringback,

        /// <summary>
        /// the call has started
        /// </summary>
        Setup,
        BusyHere,

        /// <summary>
        /// the call was cancelled
        /// </summary>
        Cancelled
    }
}