namespace SqlExport.Logic
{
    using System;
    using System.Windows.Controls;

    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Defines the ViewExtensions class.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="view">The view.</param>
        /// <param name="message">The message.</param>
        [Obsolete("Use RegisterOnDispatcher instead.")]
        public static void SendMessage<TMessage>(this Control view, TMessage message)
        {
            view.Dispatcher.BeginInvoke(
                new Action<TMessage, Control>((m, v) => Messenger.Default.Send(m, v.DataContext)),
                message,
                view);
        }

        /// <summary>
        /// Registers the message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="view">The view.</param>
        /// <param name="callback">The callback.</param>
        [Obsolete("Use RegisterOnDispatcher instead.")]
        public static void RegisterMessage<TMessage>(this Control view, Action<TMessage> callback)
        {
            view.Dispatcher.BeginInvoke(
                new Action<Action<TMessage>, Control>((c, v) => Messenger.Default.Register(view, view.DataContext, c)),
                callback,
                view);
        }

        /// <summary>
        /// Registers a message handler on the current dispatcher.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="messenger">The messenger.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="token">The token.</param>
        /// <param name="action">The action.</param>
        public static void RegisterOnDispatcher<TMessage>(this Messenger messenger, object recipient, object token, Action<TMessage> action)
        {
            messenger.Register<TMessage>(recipient, token, m => System.Windows.Application.Current.Dispatcher.BeginInvoke(action, m));
        }
    }
}
