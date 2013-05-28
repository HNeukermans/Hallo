using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;

namespace Hallo.Sip.Headers
{
    public class SipParameterCollection : IStringFormatable, IEquatable<SipParameterCollection>
    {
        Dictionary<string, SipNameValue> _collection;

        public SipParameterCollection()
        {
            _collection = new Dictionary<string, SipNameValue>();
        }

        public void Add(SipNameValue nameValue)
        {
            _collection.Add(nameValue.Name, nameValue);
        }

        public void Remove(string name)
        {
            if (_collection.ContainsKey(name))
            {
                _collection.Remove(name);
            }
        }

        public void Set(string name, string value)
        {
            if (!_collection.ContainsKey(name))
            {
                _collection.Add(name, new SipNameValue(name, value));
            }
            else
            {
                _collection[name].Value = value;
            }
        }

        public SipNameValue this[string name]
        {
            get
            {
                if (!_collection.ContainsKey(name)) return null;

                return _collection[name];
            }
        }

        public SipNameValue this[int index]
        {
            get 
            {
                var nv = _collection.ElementAtOrDefault(index);
                return default(KeyValuePair<string, SipNameValue>).Equals(nv) ? null : nv.Value;
            }

        }

        public string FormatToString()
        {
            string value = "";
            foreach (var keyValue in this._collection.Values)
            {
                value += keyValue.Name;
                if (!string.IsNullOrWhiteSpace(keyValue.Value)) value += "=" + keyValue.Value + ";";
            }
            value = value.TrimEnd(';');
            return value;
        }

        internal bool IsNotEmpty()
        {
            return _collection.Keys.Count > 0;
        }

        internal SipParameterCollection Clone()
        {
            var p = new SipParameterCollectionParser();
            return p.Parse(FormatToString());
        }

       
        public int GetHashCode(SipParameterCollection obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(SipParameterCollection other)
        {
            if (other == null) return false;
            if (this.Count != other.Count);

            for(int i = 0;i< this.Count;i++)
            {
                if (this[i] != other[i]) return false;
            }
            return true;
        }

        public int Count 
        { 
            get { return _collection.Count; }
        }

    }
}
