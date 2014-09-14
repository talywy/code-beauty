using TVGenius.Model;

namespace TVGenius.SignalTransfer.Dispatcher
{
    class TCLDispatcher : SingalDispatcherBase
    {
        public override void ChangeChannel(int channelNum)
        {
            throw new System.NotImplementedException();
        }

        public override void ChangeVolume(VolumeAction action)
        {
            throw new System.NotImplementedException();
        }

        public override void SetState(TVState state)
        {
            throw new System.NotImplementedException();
        }

        public override void SiumKeyPress(string keyCode)
        {
            throw new System.NotImplementedException();
        }

        public override void SiumMouseAction(MouseAction action)
        {
            throw new System.NotImplementedException();
        }

        public override void SiumKeyboard(string keyCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
