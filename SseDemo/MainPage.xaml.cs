using System;
using SseDemo.Helper.ServerEvents;
using Xamarin.Forms;

namespace SseDemo
{
    public partial class MainPage : ContentPage, IServerEventObserver
    {
        public MainPage()
        {
            InitializeComponent();

            EventManager.Instance.Subscribe(
                this,
                new EventType[]
                {
                    EventType.Comment,
                    EventType.DirectMessage,
                    EventType.FriendRequest
                });

            EventManager.Instance.Unsubscribe(
                this,
                new EventType[]
                {
                    EventType.DirectMessage
                });
        }

        public void OnEventReceived(ServerEvent serverEvent)
        {
            Console.WriteLine("Event Type: " + serverEvent.Type);
            Console.WriteLine("Event Data: " + serverEvent.Data);
        }
    }
}
