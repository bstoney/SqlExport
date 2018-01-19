namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Defines the PropertyItem type.
    /// </summary>
    public abstract class PropertyItem : ViewModelBase
    {
        /// <summary>
        /// The <see cref="DisplayName" /> property's name.
        /// </summary>
        public const string DisplayNamePropertyName = "DisplayName";

        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        /// <summary>
        /// The display name.
        /// </summary>
        private string displayName = null;

        /// <summary>
        /// The value.
        /// </summary>
        private string value = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItem"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="name">The name.</param>
        protected PropertyItem(string category, string name)
        {
            this.Category = category;
            this.Name = name;
            this.DisplayName = name;
            this.ValueGetter = () => this.value;
            this.ValueSetter = v => this.value = v;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }

            set
            {
                if (this.displayName == value)
                {
                    return;
                }

                this.displayName = value;
                this.RaisePropertyChanged(DisplayNamePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.ValueGetter();
            }

            set
            {
                if (this.ValueGetter() == value)
                {
                    return;
                }

                this.ValueSetter(value);

                // Update bindings, no broadcast
                this.RaisePropertyChanged(ValuePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the value getter.
        /// </summary>
        internal Func<string> ValueGetter { get; set; }

        /// <summary>
        /// Gets or sets the value setter.
        /// </summary>
        internal Action<string> ValueSetter { get; set; }

        /// <summary>
        /// Gets the edit control.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns>A new UI control.</returns>
        public abstract FrameworkElement GetEditControl(Binding binding);
    }
}
