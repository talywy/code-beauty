using System;
using TVGenius.Model;

namespace TVGenius.MobileApp.Events
{
    public class MockTVSelectedEventArgs : EventArgs
    {
        public MockTV TV
        {
            get;
            private set;
        }

        public MockTVSelectedEventArgs(MockTV tv)
        {
            TV = tv;
        }
    }
}
