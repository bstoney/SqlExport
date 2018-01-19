namespace SqlExport.Logic
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Messages;
    using SqlExport.View;

    /// <summary>
    /// Defines the ErrorDialogLogic class.
    /// </summary>
    public static class ErrorDialogLogic
    {
        /// <summary>
        /// The error form
        /// </summary>
        private static ApplicationMessages errorForm;

        /// <summary>
        /// Adds the error message.
        /// </summary>
        /// <param name="error">The error.</param>
        public static void AddError(Exception error)
        {
            if (errorForm != null)
            {
                Messenger.Default.Send((ApplicationDisplayMessage)error);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(error.ToString());
            }
        }

        /// <summary>
        /// Adds the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void AddError(string message)
        {
            if (errorForm != null)
            {
                Messenger.Default.Send(new ApplicationDisplayMessage(message, DisplayMessageType.Error));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// Adds the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void AddWarning(string message)
        {
            if (errorForm != null)
            {
                Messenger.Default.Send(new ApplicationDisplayMessage(message, DisplayMessageType.Warning));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// Adds the information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void AddInformation(string message)
        {
            if (errorForm != null)
            {
                Messenger.Default.Send(new ApplicationDisplayMessage(message, DisplayMessageType.Information));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// Adds the success message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void AddSuccess(string message)
        {
            if (errorForm != null)
            {
                Messenger.Default.Send(new ApplicationDisplayMessage(message, DisplayMessageType.Success));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// Shows the form.
        /// </summary>
        public static void ShowForm()
        {
            if (errorForm != null)
            {
                errorForm.Dispatcher.Invoke(() => errorForm.ShowFormInternal());
            }
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public static void Initialise()
        {
            if (errorForm == null)
            {
                errorForm = new ApplicationMessages();
            }
        }
    }
}
