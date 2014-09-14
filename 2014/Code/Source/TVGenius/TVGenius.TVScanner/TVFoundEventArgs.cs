using System;
using TVGenius.Model;

namespace TVGenius.TVScanner
{
    public class TVFoundEventArgs : EventArgs
    {
        public MockTV TV
        {
            get;
            private set;
        }

        public TVFoundEventArgs(MockTV tv)
        {
            TV = tv;
        }
    }
}
