using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace SqlExport.Business
{
    using System.Windows.Threading;

    public static class ViewExtensions
    {
        [Obsolete("Use RegisterOnDispatcher instead.")]
        public static void SendMessage<T>(this Control view, T message)
        {
            view.Dispatcher.BeginInvoke(
                new Action<T, Control>((m, v) => Messenger.Default.Send<T>(m, v.DataContext)),
                message,
                view);
        }

        [Obsolete("Use RegisterOnDispatcher instead.")]
        public static void RegisterMessage<T>(this Control view, Action<T> callback)
        {
            view.Dispatcher.BeginInvoke(
                new Action<Action<T>, Control>((c, v) => Messenger.Default.Register<T>(view, view.DataContext, m => c(m))),
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
            messenger.Register<TMessage>(recipient, token, m => Dispatcher.CurrentDispatcher.BeginInvoke(action, m));
        }
    }
}
