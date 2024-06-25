using CustomerTestApp.WPF.Messages;

namespace CustomerTestApp.WPF.Helpers.Messenger
{
    /// <summary>
    /// The Messenger class is responsible for sending and receiving messages. This is a singleton class.
    /// </summary>
    internal class Messenger : IMessenger
    {
        #region Private Properties

        private static readonly Lazy<Messenger> _instance = new Lazy<Messenger>(() => new Messenger());

        private Dictionary<Type, List<Action<BaseMessage>>> _subscribers = new Dictionary<Type, List<Action<BaseMessage>>>();

        #endregion

        private Messenger() { }

        #region Public Methods

        public static Messenger Instance => _instance.Value;

        public void Register<T>(Action<T> action) where T : BaseMessage
        {
            var type = typeof(T);
            if(!_subscribers.ContainsKey(type))
            {
                _subscribers.Add(type, new List<Action<BaseMessage>>());
            }
            Action<BaseMessage> handler = x => action((T)x);
            _subscribers[type].Add(handler);
        }

        public void Send<T>(T message) where T : BaseMessage
        {
            var messageType = message.GetType();
            if (_subscribers.ContainsKey(messageType))
            {
                foreach (var sub in _subscribers[messageType])
                {
                    sub(message);
                }
            }
        }

        #endregion
    }
}
