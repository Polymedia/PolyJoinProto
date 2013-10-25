using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Common
{
    public class SimpleEventArgs<T> : EventArgs
    {
        public SimpleEventArgs() { }

        public SimpleEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
