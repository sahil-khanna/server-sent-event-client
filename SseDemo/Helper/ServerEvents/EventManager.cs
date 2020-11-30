using System.Collections.Generic;
using ServiceStack;
using System;
using Newtonsoft.Json;

namespace SseDemo.Helper.ServerEvents
{
    public class EventManager
    {
        public static readonly EventManager Instance = new();

        private readonly Dictionary<EventType, List<IServerEventObserver>> Subscribers;

        public EventManager()
        {
            Subscribers = new();

            Connect();
        }

        private void Connect()
        {
            ServerEventsClient eventClient = new ServerEventsClient("http://localhost:4000/api/1.0/register")
            {
                OnMessage = OnMessage,
                OnException = (ex) =>
                {
                    Console.WriteLine("OnException: " + ex.Message);
                }
            }.Start();
        }

        public void Subscribe(IServerEventObserver subscriber, EventType[] eventTypes)
        {
            foreach (EventType eventType in eventTypes)
            {
                if (Subscribers.ContainsKey(eventType) == false)
                {
                    Subscribers.Add(eventType, new List<IServerEventObserver>() { subscriber });
                }
                else
                {
                    List<IServerEventObserver> subscribers = Subscribers[eventType];

                    if (subscribers?.Contains(subscriber) == false)
                    {
                        subscribers?.Add(subscriber);
                    }
                }
            }
        }

        public void Unsubscribe(IServerEventObserver subscriber, EventType[] eventTypes)
        {
            foreach (EventType eventType in eventTypes)
            {
                List<IServerEventObserver> subscribers = Subscribers[eventType];

                if (subscribers.Contains(subscriber))
                {
                    _ = subscribers.Remove(subscriber);
                }
            }
        }

        private void OnMessage(ServerEventMessage message)
        {
            byte[] byteArray = Convert.FromBase64String(message.Data.ToString());
            string jsonString = System.Text.Encoding.ASCII.GetString(byteArray);

            ServerEvent serverEvent = JsonConvert.DeserializeObject<ServerEvent>(jsonString);

            // Ignore Connect and Heartbeat messages
            if (serverEvent.Type == EventType.Connect || serverEvent.Type == EventType.HeartBeat)
            {
                return;
            }

            NotifySubscribers(serverEvent);
        }

        private void NotifySubscribers(ServerEvent serverEvent)
        {
            _ = Subscribers.TryGetValue(serverEvent.Type, out List<IServerEventObserver> subscribers);

            if (subscribers is null)
            {
                return;
            }

            foreach (IServerEventObserver subscriber in subscribers)
            {
                subscriber.OnEventReceived(serverEvent);
            }
        }
    }
}
