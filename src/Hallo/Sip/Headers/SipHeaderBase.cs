
namespace Hallo.Sip
{
    public abstract class SipHeaderBase 
    {
        public string Name { get; protected set; }

        public bool IsList { get; private set; }
        
        protected SipHeaderBase(bool isList)
        {
            IsList = isList;
        }
    }
    
}
