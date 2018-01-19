using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Collections;
using System.Text;
using System.ComponentModel;
using SqlExport.Ui;
using SqlExport.Ui.ViewModel;
using SqlExport.Ui.Messages;
using System.Windows;

namespace SqlExport
{
    using System.Windows.Interop;

    using GalaSoft.MvvmLight.Messaging;

    public static class ErrorDialog
    {
        private static ApplicationMessages _errorForm;

        public static void AddError(Exception error)
        {
            _errorForm.MessageList.AddError(error);
        }

        public static void AddError(string message, int? lineNumber)
        {
            _errorForm.MessageList.AddError(message, lineNumber);
        }

        public static void AddWarning(string message)
        {
            _errorForm.MessageList.AddMessage(message, DisplayMessageType.Warning);
        }

        public static void AddInformation(string message)
        {
            _errorForm.MessageList.AddMessage(message, DisplayMessageType.Information);
        }

        public static void AddSuccess(string message)
        {
            _errorForm.MessageList.AddMessage(message, DisplayMessageType.Success);
        }

        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public static void ShowForm(Window owner = null)
        {
            if (owner == null)
            {
                Messenger.Default.Send(new GetMainWindowMessage(w => owner = w));
            }

            _errorForm.Dispatcher.Invoke(new Action(() => _errorForm.ShowFormInternal(owner)));
        }

        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="handle">The handle.</param>
        public static void ShowForm(IntPtr handle)
        {
            _errorForm.Dispatcher.Invoke(new Action(() => _errorForm.ShowFormInternal(null, handle)));
        }

        public static void Initialise()
        {
            if (_errorForm == null)
            {
                _errorForm = new ApplicationMessages();

                // TODO
                ////// Ensure Handle
                ////IntPtr h = _errorForm.Handle;
                ////while( !_errorForm.IsHandleCreated || h == IntPtr.Zero )
                ////{
                ////    h = _errorForm.Handle;
                ////}
            }
        }
    }

}
