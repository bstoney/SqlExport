namespace SqlExport
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Logic;
    using SqlExport.Messages;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates the static resources for designer.
        /// </summary>
        /// <param name="element">The element.</param>
        internal static void CreateStaticResourcesForDesigner(Control element)
        {
            if (AppDomain.CurrentDomain.BaseDirectory.Contains("Blend 4"))
            {
                // Load resources.
                var resourceFiles = new List<string>
                    {
                        "Themes/CommonResources.xaml",
                        "Resources/Styles/Common.xaml",
                        "Resources/Styles/ClosableTabItem.xaml",
                        "Resources/Styles/ScrollableTabControl.xaml",
                        "Resources/Styles/Buttons.xaml",
                        "Resources/Styles/LabeledImage.xaml",
                        "Resources/Styles/Popup.xaml",
                    };

                resourceFiles.ForEach(
                    file =>
                        {
                            try
                            {
                                element.Resources.MergedDictionaries.Add(
                                    new ResourceDictionary
                                        {
                                            Source =
                                                new Uri(
                                                "pack://application:,,,/SqlExport;component/" + file, UriKind.Absolute)
                                        });
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error creating reference to resource file {0}.", file), ex);
                            }
                        });

                element.Resources.Add("Locator", new Locator());
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = e.Args;

            // If the application was started via Click-Once the will not be null.
            if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null)
            {
                // A Click-Once deployment uses activation data instead of the standard command args.
                args = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData ?? new string[] { };
            }

            // Make sure the Error Dialog is created in the UI thread.
            ErrorDialogLogic.Initialise();

            ApplicationEnvironment.Default.InitialiseEnvironment();

            Messenger.Default.Register<ExitAppMessage>(
                this,
                m =>
                {
                    var appExitingMessage = new AppExitingMessage();
                    Messenger.Default.Send(appExitingMessage);

                    Configuration.SaveOptions();

                    if (!appExitingMessage.Cancel)
                    {
                        ApplicationEnvironment.Default.Exit(0);
                    }

                    m.Cancel = appExitingMessage.Cancel;
                });

            if (!Configuration.Current.SingleInstance || SingleInstanceHelper.IsFirstInstance)
            {
                // No previous instance
                Messenger.Default.Send(new ApplicationDisplayMessage("Application has started."));

                var mainWindow = new MainWindow();
                mainWindow.InitialiseForm();
                SingleInstanceHelper.SetMainForm(mainWindow);
                mainWindow.HandleCommandArguments(args);

                this.MainWindow = mainWindow;

                mainWindow.Show();
            }
            else
            {
                SingleInstanceHelper.NotifyPreviousWindow(IntPtr.Zero, args);
                ApplicationEnvironment.Default.Exit(0);
            }

            base.OnStartup(e);
        }
    }
}
