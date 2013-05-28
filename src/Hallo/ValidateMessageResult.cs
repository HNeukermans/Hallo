using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo
{
    public class ValidateMessageResult
    {
        public virtual bool IsValid 
        {
            get { return (!HasRequiredHeadersMissing &&
                !HasUnSupportedSipVersion && 
                !HasUnSupportedSipMethod &&
                !InvalidRegisterRequestUri); }
        }

        public bool HasRequiredHeadersMissing
        {
            get { return MissingRequiredHeader != null; }
        }

        public bool HasUnSupportedSipMethod
        {
            get { return UnSupportedSipMethod != null; }
        }

        public string MissingRequiredHeader { get; set; }
        
        public bool HasUnSupportedSipVersion { get; set; }
        
        public string UnSupportedSipMethod { get; set; }
        public bool InvalidRegisterRequestUri { get; set; }
    }

    public class ValidateRequestResult : ValidateMessageResult
    {
        public ValidateRequestResult(ValidateMessageResult result)
        {
            MissingRequiredHeader = result.MissingRequiredHeader;
            HasUnSupportedSipVersion = result.HasUnSupportedSipVersion;
        }

        public bool HasInvalidSCeqMethod { get; set; }

        public bool InviteHasNoContactHeader { get; set; }

        public override bool IsValid
        {
            get
            {
                return base.IsValid && !HasInvalidSCeqMethod && !InviteHasNoContactHeader;
            }
        }

        public ValidateRequestResult()
        {
        }
    }


}
