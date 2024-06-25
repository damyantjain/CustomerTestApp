using CustomerTestApp.WPF.Messages;

namespace CustomerTestApp.WPF.Helpers.Messenger
{
    /// <summary>
    /// The IMessenger interface is responsible for sending and receiving messages
    /// across ViewModels based on the BaseMessage type.
    /// </summary>
    internal interface IMessenger
    {

        /// <summary>
        /// The Register method registers a message type and an action to be executed when the message is sent.
        /// </summary>
        /// <typeparam name="T">The type has to be of BaseMessage</typeparam>
        /// <param name="action">Delegate to be executed for the message</param>
        void Register<T>(Action<T> action) where T : BaseMessage;

        /// <summary>
        /// The Send method sends a message to the registered action.
        /// </summary>
        /// <typeparam name="T">The type has to be of a BaseMessage</typeparam>
        /// <param name="message">The message to be sent to the subscribers</param>
        void Send<T>(T message) where T : BaseMessage;
    }
}
