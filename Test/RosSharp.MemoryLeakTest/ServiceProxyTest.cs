﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RosSharp.Service;

namespace RosSharp.MemoryLeakTest
{
    class ServiceProxyTest : ITest
    {
        private Node _node;
        private IServiceServer _serviceServer;

        public void Initialize()
        {
            _node = Ros.InitNodeAsync("test", enableLogger: false).Result;

            _serviceServer = _node
                .AdvertiseServiceAsync("add_two_ints", new AddTwoInts(req => new AddTwoInts.Response() {sum = req.a + req.b}))
                .Result;
        }

        public void Do(int index)
        {
            var proxy = _node.ServiceProxyAsync<AddTwoInts>("add_two_ints").Result;

            for (int i = 0; i < 10; i++)
            {
                proxy.Invoke(new AddTwoInts.Request() {a = i, b = i*2});
            }

            proxy.Dispose();
        }

        public void Cleanup()
        {
            _serviceServer.Dispose();
            _node.Dispose();
        }
    }
}
