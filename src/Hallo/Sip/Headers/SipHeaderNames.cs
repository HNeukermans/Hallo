using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Hallo.Parsers;

namespace Hallo.Sip.Headers
{
    public class SipHeaderNames
    {
        public const string Via = "Via";
        public const string To = "To";
        public const string From = "From";
        public const string CallId = "Call-ID";
        public const string CSeq = "CSeq";
        public const string ContentLength = "Content-Length";
        public const string Contact = "Contact";
        public const string ContentType = "Content-Type";
        public const string TimeStamp = "Timestamp";
        public const string RecordRoute = "Record-Route";
        public const string MaxForwards = "Max-Forwards";
        public const string Subject = "Subject";
        public const string Route = "Route";
        public const string Expires = "Expires";
        public const string Allow = "Allow";
        public const string UserAgent = "User-Agent";

        private static List<NameTypePair> _dictionary; 

        static SipHeaderNames()
        {                     
            _dictionary = new List<NameTypePair>();

            var types = Assembly.GetAssembly(typeof (SipHeaderNames)).GetTypes().Where(t => t.GetCustomAttributes(false).OfType<HeaderForAttribute>().Any()).
                ToList();

            foreach (var type in types)
            {
                _dictionary.Add(new NameTypePair()
                {
                   Name = type.GetCustomAttributes(false).OfType<HeaderForAttribute>().First().Name,
                   Type = type
                });
            }
        }

        internal static string GetNameByType(Type type)
        {
            var item = _dictionary.FirstOrDefault(i => i.Type == type);
            if (item == null) throw new ArgumentException(string.Format(ExceptionMessage.CanNotFindNameForTypeFormatString, type.Name));
            return item.Name;
        }

        internal static string GetTypeByName(string name)
        {
            var item = _dictionary.FirstOrDefault(i => i.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (item == null) throw new ArgumentException(string.Format(ExceptionMessage.CanNotFindTypeForNameFormatString, name));
            return item.Name;
        }
   
        private class NameTypePair 
        {    
            public string Name { get; set; }
            public Type Type { get; set; }

            public NameTypePair(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public NameTypePair()
            {
            }
        }
    }
}
