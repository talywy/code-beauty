using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TVGenius.Utils;

namespace TVGenius.MockTV
{
    internal class MockTVConfig
    {
        private static MockTVConfig _instance;

        private MockTVConfig()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public static MockTVConfig Instance
        {
            get { return _instance ?? (_instance = new MockTVConfig()); }
        }

        /// <summary>
        /// MockTV
        /// </summary>
        public string TVJson
        {
            get
            {
                var mockTV = new Model.MockTV
                {
                    Brand = Brand,
                    Icon = Icon,
                    Model = Model,
                    Sn = Sn,
                    Bind = Bind
                };

                return JsonConvert.SerializeObject(mockTV);
            }
        }

        /// <summary>
        /// ICO
        /// </summary>
        public string Icon
        {
            get;
            private set;
        }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型
        /// </summary>
        public string Model
        {
            get;
            private set;
        }

        /// <summary>
        /// 通信信道
        /// </summary>
        public string Bind
        {
            get;
            private set;
        }

        /// <summary>
        /// 唯一序列号
        /// </summary>
        public string Sn
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前频道
        /// </summary>
        public int Channel
        {
            get;
            private set;
        }

        /// <summary>
        /// 频道最大数目
        /// </summary>
        public int MaxChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前音量
        /// </summary>
        public int Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// 最大音量
        /// </summary>
        public int MaxVolume
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configFile">配置文件路径</param>
        public void Init(string configFile)
        {
            var configStr = File.ReadAllText(configFile);
            var configJson = JsonConvert.DeserializeObject<JObject>(configStr);

            Icon = Path.Combine(AppHelper.IconBase, configJson["icon"].Value<string>());
            Brand = configJson["brand"].Value<string>();
            Model = configJson["model"].Value<string>();
            Bind = configJson["bind"].Value<string>();
            Sn = configJson["sn"].Value<string>();
            Channel = configJson["channel"].Value<int>();
            MaxChannel = configJson["maxchannel"].Value<int>(); 
            Volume = configJson["volume"].Value<int>();
            MaxVolume = configJson["maxvolume"].Value<int>(); 
        }
    }
}
