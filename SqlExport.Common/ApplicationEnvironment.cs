namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the ApplicationEnvironment type.
    /// </summary>
    public class ApplicationEnvironment
    {
        /// <summary>
        /// The default environment.
        /// </summary>
        private static readonly ApplicationEnvironment DefaultEnvironment = new ApplicationEnvironment();

        /// <summary>
        /// The file version info.
        /// </summary>
        private FileVersionInfo fileVersionInfo;

        /// <summary>
        /// Gets the default.
        /// </summary>
        public static ApplicationEnvironment Default
        {
            get { return DefaultEnvironment; }
        }

        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName
        {
            get { return this.FileVersionInfo.CompanyName; }
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName
        {
            get { return this.FileVersionInfo.ProductName; }
        }

        /// <summary>
        /// Gets the product version.
        /// </summary>
        public string ProductVersion
        {
            get { return this.FileVersionInfo.ProductVersion; }
        }

        /// <summary>
        /// Gets the file version info.
        /// </summary>
        private FileVersionInfo FileVersionInfo
        {
            get
            {
                if (this.fileVersionInfo == null)
                {
                    // Application.UserAppDataPath and Application.LocalUserAppDataPath don't work for click once install
                    var assembly = Assembly.GetEntryAssembly();
                    this.fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                }

                return this.fileVersionInfo;
            }
        }

        /// <summary>
        /// Initialises the application environment.
        /// </summary>
        public virtual void InitialiseEnvironment()
        {
            // Load connection adapters and other options.
            Configuration.Load();
        }

        /// <summary>
        /// Exits the specified exit code.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
