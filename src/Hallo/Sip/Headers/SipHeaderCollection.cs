using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hallo.Sip
{
    public class SipHeaderCollection : IEnumerable<SipHeaderBase>
    {
        readonly Dictionary<string, SipHeaderBase> _collection;

        public SipHeaderCollection()
        {
            _collection = new Dictionary<string, SipHeaderBase>();
        }

        public void Add(SipHeaderBase header)
        {
            _collection.Add(header.Name, header);
        }

        public void Remove(string name)
        {
            if (_collection.ContainsKey(name))
            {
                _collection.Remove(name);
            }
        }

        //public void Set(SipHeaderBase header)
        //{
        //    if (_collection.ContainsKey(header.Name))
        //    {
        //        //the existing header is a headerlist 
        //        if (_collection[header.Name].IsList)
        //        {
        //            var existingList = _collection[header.Name] as SipHeaderList;

        //            //the header to set, is also a headerlist
        //            if(header.IsList)
        //            {
        //                var newList = header as SipHeaderList;
        //                existingList.AddList(newList);
        //            }
        //            else
        //            {
        //                //the header to set is a single value
        //                existingList.Add(header);
        //            }
        //        }
        //        //the existing header is a single value 
        //        else 
        //        {
        //            //this case is not possible. A header that is listable can add:
        //            //1) single value //2) list // to its list.
        //            //A header that is single valued can only be replaced be a new single valued header.
                        
        //            if (header.IsList)
        //            {
        //                throw new NotSupportedException(ExceptionMessage.TheHeaderToSetCanNotBeHeaderList);
        //            }
        //            _collection[header.Name] = header;
        //        }                
        //    }
        //    else
        //    {
        //        _collection[header.Name] = header;
        //    }
        //}

        public SipHeaderBase this[string name]
        {
            get
            {
                if (!_collection.ContainsKey(name)) return null;

                return _collection[name];
            }
        }

        public bool ContainsKey(string name)
        {
            return _collection.ContainsKey(name);
        }

        internal bool IsNotEmpty()
        {
            return _collection.Keys.Count > 0;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.Values.GetEnumerator();
        }

        IEnumerator<SipHeaderBase> IEnumerable<SipHeaderBase>.GetEnumerator()
        {
            return _collection.Values.ToList().GetEnumerator();
        }
    }
}
