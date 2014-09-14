using System;

namespace TVGenius.TVScanner
{
    public class TVHeartBeatEventArgs : EventArgs
    {
        public string TVSn
        {
            get;
            private set;
        }

        public TVHeartBeatEventArgs(string tvSN)
        {
            TVSn = tvSN;
        }
    }
}
