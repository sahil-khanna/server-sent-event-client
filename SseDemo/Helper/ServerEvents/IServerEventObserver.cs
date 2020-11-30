namespace SseDemo.Helper.ServerEvents
{
    public struct ServerEvent
    {
        public EventType Type;
        public object Data;
    }

    public enum EventType
    {
        Connect = 0,
        HeartBeat,
        Comment = 105,
        DirectMessage,
        FriendRequest = 207
    }

    public interface IServerEventObserver
    {
        void OnEventReceived(ServerEvent serverEvent);
    }
}