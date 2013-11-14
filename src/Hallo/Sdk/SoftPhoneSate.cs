namespace Hallo.Sdk
{

    //TODO: remove
    /// <summary> 
    /// </summary>
    /// <remarks>
    /// lists all possible states to api user.
    /// </remarks>
    public enum SoftPhoneState
    {
        Idle,
        
        /// <summary>
        /// State that represents the callee, when the callee's phone is alerting.
        /// </summary>
        Ringing,

        Established,

        /// <summary>
        /// State that represents the caller, waiting for the callee to pick up the phone.
        /// </summary>
        Waiting 
    }
}