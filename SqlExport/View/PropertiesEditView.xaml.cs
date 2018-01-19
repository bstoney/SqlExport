namespace SqlExport.View
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    using GalaSoft.MvvmLight;

    using SqlExport.ViewModel;

    /// <summary>
    /// Interaction logic for PropertiesEditView.xaml
    /// </summary>
    public partial class PropertiesEditView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesEditView"/> class.
        /// </summary>
        public PropertiesEditView()
        {
            this.InitializeComponent();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                this.Properties = new[]
                    {
                        new
                            {
                                Key = "Category",
                                Properties = new[]
                                    {
                                        new NumericPropertyItem("Category", "Name")
                                            { 
                                                DisplayName = "Display Name", 
                                                Value = "1" 
                                            }
                                    }
                            }
                    };
            }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        private object Properties
        {
            get { return (ObservableCollection<PropertyItem>)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        /// <summary>
        /// The properties property.
        /// </summary>
        private static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register("Properties", typeof(object), typeof(PropertiesEditView), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(object),
            typeof(PropertiesEditView),
            new FrameworkPropertyMetadata(null, ItemsSourceChanges));

        /// <summary>
        /// Handles change of ItemsSource.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ItemsSourceChanges(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var propertiesEditView = dependencyObject as PropertiesEditView;
            var properties = args.NewValue as IEnumerable<PropertyItem>;
            if (properties != null)
            {
                propertiesEditView.Properties = from p in properties
                                                group p by p.Category
                                                    into c
                                                    select new
                                                    {
                                                        Category = c.Key,
                                                        Properties = c
                                                    };
            }
        }
    }
}
