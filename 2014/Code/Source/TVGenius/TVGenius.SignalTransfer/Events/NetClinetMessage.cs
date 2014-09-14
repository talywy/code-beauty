using System;

namespace TVGenius.SignalTransfer.Events
{
    public class NetClinetMessageEventArgs: EventArgs
    {
        public string Message
        {
            get;
            private set;
        }

        public NetClinetMessageEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
