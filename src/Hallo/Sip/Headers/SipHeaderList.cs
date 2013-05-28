using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hallo.Parsers;
using Hallo.Sip.Headers;

namespace Hallo.Sip
{
    public class SipHeaderList<T> : SipHeaderBase, IEnumerable<T> where T : SipHeader
    {
        protected List<SipHeader> _list;

        public SipHeaderList(string name):this()
        {
            this.Name = name;
        }

        public SipHeaderList():base(true)
        {
            this.Name = SipHeaderNames.GetNameByType(typeof(T));
            _list = new List<SipHeader>(); 
        }
       
        public void SetTopMost(T value)
        {
            this._list.Insert(0, value);
        }

        public T GetTopMost()
        {
            return (T)_list.FirstOrDefault();
        }

        public void RemoveTopMost()
        {
            _list.RemoveAt(0);
        }

        public int IndexOf(T item)
        {
            return this._list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return (T)_list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.Cast<T>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public List<T> ToList()
        {
            var result = new List<T>();
            _list.ForEach(i => result.Add((T)i));
            return result;
        }

        public SipHeaderList<T> Clone()
        {
            var cloned = new SipHeaderList<T>();

            foreach (T h in _list)
            {
                cloned.Add((T)h.Clone());
            };

            return cloned;
        }
    }
}
