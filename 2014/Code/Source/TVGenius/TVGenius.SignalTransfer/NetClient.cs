﻿using System;
using System.Collections.Generic;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using TVGenius.SignalTransfer.Events;

namespace TVGenius.SignalTransfer
{
    public class NetClient
    {
        public event EventHandler<NetClinetMessageEventArgs> MessageReceived;
            
        private readonly Queue<string> _msgQueue = new Queue<string>(); 
        private readonly RequestSocket _req;
        private bool _isRunning;
        private readonly object _locker = new object();
        private Thread _t;

        public NetClient(RequestSocket req)
        {
            _req = req;
        }

        public void SendMessage(string msg)
        {
            lock (_locker)
            {
                _msgQueue.Enqueue(msg);
            }
        }

        public void Start()
        {
            _isRunning = true;

            _t = new Thread(() =>
            {
                while (_isRunning)
                {
                    lock (_locker)
                    {
                        if (_msgQueue.Count > 0)
                        {
                            var msg = _msgQueue.Dequeue();
                            _req.Send(msg);
                            var response = _req.ReceiveString();
                            MessageReceived(this, new NetClinetMessageEventArgs(response));
                        }
                    }

                    Thread.Sleep(100);
                }

            }) { IsBackground = true };
            _t.Start();
        }

        public void Stop()
        {
            _isRunning = false;

            lock (_locker)
            {
                _msgQueue.Clear();
            }

            if (_t.IsAlive)
            {
                _t.Abort();
            }
        }
    }
}
