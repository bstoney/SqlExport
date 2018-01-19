namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Defines the BindableStaticResource type.
    /// </summary>
    public class BindableStaticResource : StaticResourceExtension
    {
        /// <summary>
        /// The dummy property.
        /// </summary>
        private static readonly DependencyProperty DummyProperty = DependencyProperty.RegisterAttached(
            "Dummy", typeof(object), typeof(DependencyObject), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableStaticResource"/> class.
        /// </summary>
        public BindableStaticResource()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableStaticResource"/> class.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public BindableStaticResource(Binding binding)
        {
            this.Binding = binding;
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.StaticResourceExtension"/>, this is the object found in a resource dictionary, where the object to find is identified by the <see cref="P:System.Windows.StaticResourceExtension.ResourceKey"/>.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the markup extension provided value is evaluated.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = (FrameworkElement)target.TargetObject;

            Binding.Source = targetObject.DataContext;
            var dummyDO = new DependencyObject();

            BindingOperations.SetBinding(dummyDO, DummyProperty, Binding);

            this.ResourceKey = dummyDO.GetValue(DummyProperty) ?? "EmptyIcon";

            return base.ProvideValue(serviceProvider);
        }
    }
}
