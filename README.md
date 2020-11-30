# Introduction
A Xamarin multiplatform app that uses the ServiceStack.Client nuget and demonstrates the use of Server-Sent Events.

The backend API that this app connects to is available in this [Git repository](https://github.com/sahil-khanna/server-sent-event-backend)

The details of the source code is explained in this [article](https://sahil-khanna.medium.com/server-sent-events-adc149cea606)

# Usage

```public partial class MainPage : ContentPage, IServerEventObserver
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
