using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TVGenius.TVScanner
{
    class SignalMap
    {
        public static void Init(string signalMap)
        {
            var singalMapContent = File.ReadAllText(signalMap);
            XDocument xdoc = new XDocument();
        }

        public static int GetPort(string brand, string model)
        {
            return 0;
        }

        public static List<string> GetAllBinds()
        {
            return new List<string> { "tcp://127.0.0.1:12000", "tcp://127.0.0.1:13000" };
        }
    }
}
