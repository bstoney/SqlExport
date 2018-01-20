namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using GalaSoft.MvvmLight;

    using SqlExport.Common;
    using SqlExport.Common.Options;
    using SqlExport.ViewModel.Options;

    /// <summary>
    /// Defines the OptionViewModel class.
    /// </summary>
    internal class OptionViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        /// <summary>
        /// The option
        /// </summary>
        private readonly Option option;

        /// <summary>
        /// Indicates whether the option is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionViewModel" /> class.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="optionControlCreator">The control creator.</param>
        private OptionViewModel(Option option, OptionControlCreator optionControlCreator)
        {
            this.option = option;
            this.Children = from c in this.option.Children
                            orderby c.DisplayName
                            select (OptionViewModel)c;
            this.Properties = (from p in this.option.Properties
                               orderby p.DisplayName
                               select (PropertyViewModel)p).ToList();

            this.OptionControlCreator = optionControlCreator;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return this.option.Name.Name; }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName
        {
            get { return this.option.DisplayName; }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path
        {
            get { return this.option.GetPath().ToString(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has properties.
        /// </summary>
        public bool HasProperties
        {
            get { return this.Properties != null && this.Properties.Any(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        public bool HasChildren
        {
            get { return this.Children != null && this.Children.Any(); }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public IEnumerable<PropertyViewModel> Properties { get; private set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IEnumerable<OptionViewModel> Children { get; private set; }

        /// <summary>
        /// Gets or sets the Value property.
        /// </summary>
        public string Value
        {
            get
            {
                return this.option.Value;
            }

            set
            {
                if (this.option.Value == value)
                {
                    return;
                }

                this.option.SetValueByPath(string.Empty, value);
                this.RaisePropertyChanged(ValuePropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected == value)
                {
                    return;
                }

                this.isSelected = value;
                this.RaisePropertyChanged(IsSelectedPropertyName);
            }
        }

        /// <summary>
        /// Gets the option control creator.
        /// </summary>
        internal OptionControlCreator OptionControlCreator { get; private set; }

        /// <summary>
        /// Converts the option to an option view model.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>A new option view model.</returns>
        public static explicit operator OptionViewModel(Option option)
        {
            OptionControlCreator optionControlCreator;
            switch (option.Type)
            {
                case OptionType.Boolean:
                    optionControlCreator = new BooleanOptionControlCreator();
                    break;
                case OptionType.Numeric:
                    optionControlCreator = new NumericOptionControlCreator();
                    break;
                case OptionType.Selection:
                    var selectionOption = option.OptionDefinition as SelectionOptionAttribute;
                    string[] items = selectionOption != null ? selectionOption.SelectionItems : new string[] { };
                    optionControlCreator = new SelectionOptionControlCreator(items);
                    break;
                default:
                    optionControlCreator = new TextOptionControlCreator();
                    break;
            }

            return new OptionViewModel(option, optionControlCreator);
        }

        /// <summary>
        /// Defines the PropertyViewModel class.
        /// </summary>
        internal class PropertyViewModel : ViewModelBase
        {
            /// <summary>
            /// The <see cref="Value" /> property's name.
            /// </summary>
            public const string ValuePropertyName = "Value";

            /// <summary>
            /// The option
            /// </summary>
            private readonly OptionProperty option;

            /// <summary>
            /// Initializes a new instance of the <see cref="PropertyViewModel" /> class.
            /// </summary>
            /// <param name="option">The option.</param>
            /// <param name="optionControlCreator">The option control creator.</param>
            public PropertyViewModel(OptionProperty option, OptionControlCreator optionControlCreator)
            {
                this.option = option;
                this.OptionControlCreator = optionControlCreator;
            }

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name
            {
                get { return this.option.Name.Name; }
            }

            /// <summary>
            /// Gets the path.
            /// </summary>
            public string Path
            {
                get { return this.option.GetPath().ToString(); }
            }

            /// <summary>
            /// Gets or sets the Value property.
            /// </summary>
            public string Value
            {
                get
                {
                    return this.option.Value;
                }

                set
                {
                    if (this.Value == value)
                    {
                        return;
                    }

                    this.option.Value = value;
                    this.RaisePropertyChanged(ValuePropertyName);
                }
            }

            /// <summary>
            /// Gets the option control creator.
            /// </summary>
            internal OptionControlCreator OptionControlCreator { get; private set; }

            /// <summary>
            /// Converts the option property to a property view model.
            /// </summary>
            /// <param name="option">The option property.</param>
            /// <returns>A new property view model.</returns>
            public static explicit operator PropertyViewModel(OptionProperty option)
            {
                OptionControlCreator optionControlCreator;
                switch (option.Type)
                {
                    case OptionType.Boolean:
                        optionControlCreator = new BooleanOptionControlCreator();
                        break;
                    case OptionType.Numeric:
                        optionControlCreator = new NumericOptionControlCreator();
                        break;
                    case OptionType.Selection:
                        var selectionOption = option.OptionDefinition as SelectionOptionAttribute;
                        string[] items = selectionOption != null ? selectionOption.SelectionItems : new string[] { };
                        optionControlCreator = new SelectionOptionControlCreator(items);
                        break;
                    default:
                        optionControlCreator = new TextOptionControlCreator();
                        break;
                }

                return new PropertyViewModel(option, optionControlCreator);
            }
        }
    }
}
