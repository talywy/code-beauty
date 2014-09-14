using TVGenius.Model;

namespace TVGenius.SignalTransfer.Dispatcher
{
    public class DispatcherFactory
    {
        public static SingalDispatcherBase GetDispatcher(MockTV tv)
        {
            // TODO： 根据电视机的品牌和型号找到对应的命令分发器
            return null;
        }
    }
}
