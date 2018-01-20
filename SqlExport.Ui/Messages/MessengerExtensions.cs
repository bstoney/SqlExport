namespace SqlExport.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Defines the MessengerExtensions type.
    /// </summary>
    public static class MessengerExtensions
    {
        /// <summary>
        /// Sends the on dispatcher.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messenger">The messenger.</param>
        /// <param name="message">The message.</param>
        /// <param name="token">The token.</param>
        public static void SendOnDispatcher<TMessage>(this Messenger messenger, TMessage message, object token )
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => messenger.Send(message, token)));
        }
    }
}
