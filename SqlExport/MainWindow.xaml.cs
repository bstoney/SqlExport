namespace SqlExport
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interop;

    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Common.Options;
    using SqlExport.Messages;

    using WindowState = System.Windows.WindowState;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : ISingleInstanceForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.Closing += this.MainWindow_Closing;

            Messenger.Default.Register<AppExitingMessage>(this, this.AppExiting);

            Configuration.SetOptionsOn(this);
        }

        /// <summary>
        /// Occurs when [WND proc2].
        /// </summary>
        public event WndProc2EventHandler WndProc2;

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a window is restored, minimized, or maximized.
        /// </summary>
        /// <returns>A <see cref="T:System.Windows.WindowState" /> that determines whether a window is restored, minimized, or maximized. The default is <see cref="F:System.Windows.WindowState.Normal" /> (restored).</returns>
        [EnumOption("WindowState/FormState", typeof(WindowState))]
        public new WindowState WindowState
        {
            get { return base.WindowState; }
            set { base.WindowState = value; }
        }

        /// <summary>
        /// Gets or sets the position of the window's left edge, in relation to the desktop.
        /// </summary>
        /// <returns>The position of the window's left edge, in logical units (1/96th of an inch).</returns>
        [Option("WindowState/WindowSize/@X", DefaultValue = "15")]
        public new double Left
        {
            get { return base.Left; }
            set { base.Left = value; }
        }

        /// <summary>
        /// Gets or sets the position of the window's top edge, in relation to the desktop.
        /// </summary>
        /// <returns>The position of the window's top, in logical units (1/96").</returns>
        [Option("WindowState/WindowSize/@Y", DefaultValue = "15")]
        public new double Top
        {
            get { return base.Top; }
            set { base.Top = value; }
        }

        /// <summary>
        /// Gets or sets the width of the element.
        /// </summary>
        /// <returns>The width of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.NaN" />. This value must be equal to or greater than 0.0. See Remarks for upper bound information.</returns>
        [Option("WindowState/WindowSize/@Width", DefaultValue = "994")]
        public new double Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        /// <summary>
        /// Gets or sets the suggested height of the element.
        /// </summary>
        /// <returns>The height of the element, in device-independent units (1/96th inch per unit). The default value is <see cref="F:System.Double.NaN" />. This value must be equal to or greater than 0.0. See Remarks for upper bound information.</returns>
        [Option("WindowState/WindowSize/@Height", DefaultValue = "488")]
        public new double Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        /// <summary>
        /// Initialises the form.
        /// </summary>
        public void InitialiseForm()
        {
            Messenger.Default.Register<UnhandledExceptionMessage>(
                Configuration.Current,
                m =>
                Messenger.Default.Send(
                    new ApplicationDisplayMessage(m.Exception.Message)
                    {
                        Details = m.Exception.ToString(),
                        Type = DisplayMessageType.Error
                    }));

            Messenger.Default.Register<GetMainWindowMessage>(this, m => m.MainWindow = this);

            Messenger.Default.Send(new AppStartingMessage());
        }

        /// <summary>
        /// Handles the command arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void HandleCommandArguments(string[] args)
        {
            foreach (var filePath in args)
            {
                Messenger.Default.Send(new OpenQueryMessage { Filename = filePath });
            }

            this.Activate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(this.WndProc);
        }

        /// <summary>
        /// Handles the Opened event of the NewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void NewButton_Opened(object sender, RoutedEventArgs e)
        {
            var templatesMenu = (ContextMenu)this.NewButton.FindResource("TemplatesMenu");
            templatesMenu.IsOpen = true;
        }

        /// <summary>
        /// Handles the Closing event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var exitAppMessage = new ExitAppMessage();
            Messenger.Default.Send(exitAppMessage);
            e.Cancel = exitAppMessage.Cancel;
        }

        /// <summary>
        /// Apps the exiting.
        /// </summary>
        /// <param name="message">The message.</param>
        private void AppExiting(AppExitingMessage message)
        {
            Configuration.SaveOptionsFrom(this);
        }

        /// <summary>
        /// WNDs the proc.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <returns></returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here."),
        SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1616:ElementReturnValueDocumentationMustHaveText", Justification = "Reviewed. Suppression is OK here."),
        SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."),
        DebuggerHidden]
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // TODO 
            ////bool bCancel = false;
            ////if (WndProc2 != null)
            ////{
            ////    WndProc2(m, ref bCancel);
            ////}

            return IntPtr.Zero;
        }

        /// <summary>
        /// Handles the Opened event of the ContextMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var templatesMenu = (ContextMenu)sender;
            templatesMenu.PlacementTarget = this.NewButton;
            templatesMenu.Placement = PlacementMode.Bottom;
        }
    }
}
