using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TVGenius.SignalTransfer
{
    public static class SignalMap
    {
        static readonly Dictionary<string, Dictionary<string, string>> BindStrInfDictionary = new Dictionary<string, Dictionary<string, string>>();
 
        public static void Init(string signalMap)
        {
            var xdoc = XDocument.Load(signalMap);
            var brands = xdoc.Descendants("Brand");
            foreach (var xElement in brands)
            {
                var brandName = xElement.Attribute("Name").Value;
                if (!BindStrInfDictionary.ContainsKey(brandName))
                {
                    BindStrInfDictionary.Add(brandName, new Dictionary<string, string>());
                }
                var tvs = xElement.Descendants("TV");

                foreach (var element in tvs)
                {
                    var model = element.Attribute("Model").Value;
                    var bind = element.Attribute("Bind").Value;

                    if (!BindStrInfDictionary[brandName].ContainsKey(model))
                    {
                        BindStrInfDictionary[brandName].Add(model, bind);
                    }
                }
            }
        }

        public static string GetBind(string brand, string model)
        {
            if (BindStrInfDictionary.ContainsKey(brand))
            {
                if (BindStrInfDictionary[brand].ContainsKey(model))
                {
                    return BindStrInfDictionary[brand][model];
                }
            }

            return "";
        }

        public static List<string> GetAllBinds()
        {
            var lsit = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in BindStrInfDictionary)
            {
                lsit.AddRange(keyValuePair.Value.Keys.Select(key => keyValuePair.Value[key]));
            }
            return lsit;
        }
    }
}
