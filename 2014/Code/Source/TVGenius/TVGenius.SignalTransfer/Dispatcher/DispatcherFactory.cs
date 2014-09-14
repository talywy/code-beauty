using System;
using System.Collections.Generic;
using TVGenius.Model;
using TVGenius.Utils;

namespace TVGenius.SignalTransfer.Dispatcher
{
    public class DispatcherFactory
    {
        private const string Haier = "haier";
        private const string TCL = "tcl";

        private static readonly Dictionary<string, SingalDispatcherBase> Dispatchers = new Dictionary<string, SingalDispatcherBase>();

        public static SingalDispatcherBase GetDispatcher(MockTV tv)
        {
            var key = string.Format("{0}-{1}", tv.Brand, tv.Model);

            if (!Dispatchers.ContainsKey(key))
            {
                SingalDispatcherBase dispatcher;
                if (tv.Brand.Equals(Haier, StringComparison.CurrentCultureIgnoreCase))
                {
                    dispatcher = new HaierDispatcher();
                }
                else if (tv.Brand.Equals(TCL, StringComparison.CurrentCultureIgnoreCase))
                {
                    dispatcher = new TCLDispatcher();
                }
                else
                {
                    LogUtil.Log.ErrorFormat("Unsupport brand:{0}", tv.Brand);
                    return null;
                }

                var bindStr = SignalMap.GetBind(tv.Brand, tv.Model);
                if (string.IsNullOrWhiteSpace(bindStr))
                {
                    LogUtil.Log.ErrorFormat("Unsupport model:{0}", tv.Model);
                    return null;
                }

                dispatcher.BuildClient(bindStr);
                Dispatchers.Add(key, dispatcher);
            }

            return Dispatchers[key];
        }
    }
}
