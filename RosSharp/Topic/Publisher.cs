﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using RosSharp.Message;

namespace RosSharp.Topic
{
    public sealed class Publisher<TDataType> : IPublisher, IObserver<TDataType> 
        where TDataType : IMessage, new()
    {
        private List<RosTopicClient<TDataType>> _rosTopics = new List<RosTopicClient<TDataType>>();

        public Publisher(string name, string nodeId)
        {
            var dummy = new TDataType();

            Name = name;
            Type = dummy.MessageType;

            NodeId = nodeId;
        }

        public string NodeId { get; private set; }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public void OnNext(TDataType value)
        {
            //TODO: SendAsyncは別スレッドで動かすほうが良い
            _rosTopics.ForEach(x => x.SendTaskAsync(value).Wait());
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnCompleted()
        {
            
        }

        public event Action Connected;

        internal void AddTopic(RosTopicClient<TDataType> rosTopicClient)
        {
            _rosTopics.Add(rosTopicClient);
        }

        internal void UpdateSubscriber(List<Uri> uris)
        {

            var handler = Connected;
            if(handler != null)
            {
                handler();
            }
        }
    }
}