using System;
using System.Collections.Generic;

namespace WowSuite.Launcher
{
    public class WellknownKeyNotFoundException : KeyNotFoundException
    {
        public WellknownKeyNotFoundException(object key, string message)
            : this(key, message, null) { }

        public WellknownKeyNotFoundException(object key, string message, Exception innerException)
            : base(message, innerException)
        {
            Key = key;
        }

        public object Key { get; private set; }
    }
}