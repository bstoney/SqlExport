namespace SqlExport
{
    /// <summary>
    /// Main program class.
    /// </summary>
    public sealed class Program
    {
        /////// <summary>
        /////// Main program entry point.
        /////// </summary>
        /////// <param name="args">The args.</param>
        ////[STAThread]
        ////public static void Main(string[] args)
        ////{
        ////    // If the application was started via Click-Once the will not be null.
        ////    if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null)
        ////    {
        ////        // A Click-Once deployment uses activation data instead of the standard command args.
        ////        args = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData ?? new string[] { };
        ////    }

        ////    // Make sure the Error Dialog is created in the UI thread.
        ////    ErrorDialog.Initialise();

        ////    ApplicationEnvironment.Default.InitialiseEnvironment();

        ////    if (!Options.Current.SingleInstance || SingleInstanceHelper.IsFirstInstance)
        ////    {
        ////        // No previous instance
        ////        Application.EnableVisualStyles();
        ////        SplashForm sf = new SplashForm();
        ////        sf.Show();
        ////        Application.DoEvents();

        ////        Messenger.Default.Send(new ApplicationDisplayMessage("Application has started."));

        ////        MainForm mf = new MainForm(sf);
        ////        mf.InitialiseForm();
        ////        SingleInstanceHelper.SetMainForm(mf);
        ////        mf.HandleCommandArguments(args);
        ////        Application.Run(mf);
        ////    }
        ////    else
        ////    {
        ////        SingleInstanceHelper.NotifyPreviousWindow(IntPtr.Zero, args);
        ////    }

        ////    ApplicationEnvironment.Default.Exit(0);
        ////}
    }
}
