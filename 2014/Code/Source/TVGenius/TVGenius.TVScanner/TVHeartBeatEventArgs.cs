using System;
using TVGenius.Model;

namespace TVGenius.TVScanner
{
    public class TVHeartBeatEventArgs : EventArgs
    {
        public string TVSn
        {
            get;
            private set;
        }

        public TVState State
        {
            get;
            private set;
        }

        public TVHeartBeatEventArgs(string tvSN, TVState state)
        {
            TVSn = tvSN;
            State = state;
        }
    }
}
