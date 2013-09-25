using System;

namespace Hallo.Sdk
{
    public class VoipEventArgs<T> : EventArgs
    {
        public T Item { get; set; }

        public VoipEventArgs(T item)
        {
            Item = item;
        }
    }
}