namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// Defines the DependencyResolver type.
    /// </summary>
    public class DependencyResolver
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly UnityContainer container;

        /// <summary>
        /// Initializes static members of the <see cref="DependencyResolver"/> class.
        /// </summary>
        static DependencyResolver()
        {
            Default = new DependencyResolver();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyResolver"/> class.
        /// </summary>
        public DependencyResolver()
        {
            this.container = new UnityContainer();
            this.container.LoadConfiguration();
        }

        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        public static DependencyResolver Default { get; set; }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns>An instance of the type.</returns>
        public TType Resolve<TType>()
        {
            return this.container.Resolve<TType>();
        }
    }
}
