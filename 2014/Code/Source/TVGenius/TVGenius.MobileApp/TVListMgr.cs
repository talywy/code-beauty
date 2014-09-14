using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;
using TVGenius.Model;

namespace TVGenius.MobileApp
{
    class TVListMgr
    {
        private Timer _stateCheckTimer;
        private const int STATE_CHECK_TIMEER_INTERVAL = 300;
        private const int TV_MAX_IDLE_TIME = 5;
        private const string LOCAL_TV_LIST_FILE = "tvlist.json";

        public ObservableCollection<MockTV> MockTVs
        {
            get;
            set;
        }

        public TVListMgr()
        {
            MockTVs = new ObservableCollection<MockTV>();
            LoadLocalTVs();
            _stateCheckTimer = new Timer(STATE_CHECK_TIMEER_INTERVAL);
            _stateCheckTimer.Elapsed += StateCheckTimerOnElapsed;
            _stateCheckTimer.Start();
        }

        private void StateCheckTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var now = DateTime.Now;
            for (int index = MockTVs.Count - 1; index > -1; index--)
            {
                var tv = MockTVs[index];
                tv.State = (now - tv.LastConnectTime).TotalSeconds > TV_MAX_IDLE_TIME ? TVState.Offline : TVState.PowerOn;
            }
        }

        /// <summary>
        /// 加载电视列表
        /// </summary>
        private void LoadLocalTVs()
        {
            if (File.Exists(LOCAL_TV_LIST_FILE))
            {
                var tvLists = File.ReadAllText(LOCAL_TV_LIST_FILE);
                var tvListArr = JsonConvert.DeserializeObject<MockTV[]>(tvLists);
                foreach (var mockTV in tvListArr)
                {
                    mockTV.State = TVState.Offline;
                    MockTVs.Add(mockTV);
                }
            }
        }

        /// <summary>
        /// 保存电视列表
        /// </summary>
        public void SaveTVList()
        {
            var tvLists =  JsonConvert.SerializeObject(MockTVs.ToArray());
            File.WriteAllText(LOCAL_TV_LIST_FILE, tvLists);
        }

        /// <summary>
        /// 添加电视
        /// </summary>
        /// <param name="tv">电视信息</param>
        public void AddTV(MockTV tv)
        {
            if (MockTVs.Any(mockTV => mockTV.Sn.Equals(tv.Sn)))
            {
                return;
            }

            MockTVs.Add(tv);
        }

        /// <summary>
        /// 是否已经有同样SN号的电视
        /// </summary>
        /// <param name="tvSN">电视序列号</param>
        /// <returns></returns>
        public bool ContainTV(string tvSN)
        {
            return MockTVs.Any(mockTV => mockTV.Sn.Equals(tvSN));
        }

        /// <summary>
        /// 刷新电视
        /// </summary>
        /// <param name="tvSN">电视序列号</param>
        public void RefreshTV(string tvSN)
        {
            foreach (var tV in MockTVs)
            {
                if (tV.Sn.Equals(tvSN))
                {
                    tV.LastConnectTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 移除电视
        /// </summary>
        /// <param name="tvSN">电视序列号</param>
        public void RemoveTV(string tvSN)
        {
            for (int index = MockTVs.Count - 1; index > -1; index--)
            {
                if (MockTVs[index].Sn.Equals(tvSN))
                {
                    MockTVs.RemoveAt(index);
                }
            }
        }
    }
}
