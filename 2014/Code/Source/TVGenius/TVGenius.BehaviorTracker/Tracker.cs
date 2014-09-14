using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVGenius.BehaviorTracker
{
    public class Tracker
    {
        private static Tracker _instance;

        private Tracker()
        {
            // TODO： Get mobile phone brand and model
        }

        public static Tracker Instance
        {
            get
            {
                return _instance ?? (_instance = new Tracker());
            }
        }

        public void Track()
        {
            // TODO: do the track asynchronous
        }
    }
}
