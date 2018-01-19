namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the IOptionLoader type.
    /// </summary>
    internal interface IOptionLoader
    {
        /// <summary>
        /// Gets or sets the last loaded.
        /// </summary>
        DateTime? LastLoaded { get; set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        IOptions Options { get; }

        /// <summary>
        /// Gets all options.
        /// </summary>
        IEnumerable<Option> AllOptions { get; }
    
        /// <summary>
        /// Saves the config.
        /// </summary>
        /// <param name="document">The document.</param>
        void SaveConfig(XmlDocument document);

        /// <summary>
        /// Loads the config.
        /// </summary>
        /// <param name="document">The document.</param>
        void LoadConfig(XmlDocument document);

        /// <summary>
        /// Gets the value with the supplied name.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The value.
        /// </returns>
        string GetOptionValue(string path);

        /// <summary>
        /// Sets the option value.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        void SetOptionValue(string path, object value);

        /// <summary>
        /// Removes the option.
        /// </summary>
        /// <param name="path">The path.</param>
        void RemoveOption(string path);
    }
}