namespace SqlExport
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Defines the LabeledImage type.
    /// </summary>
    public class LabeledImage : Label
    {
        /// <summary>
        /// The image source property.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(LabeledImage));

        /// <summary>
        /// The image dock property.
        /// </summary>
        public static readonly DependencyProperty ImageDockProperty =
            DependencyProperty.Register("ImageDock", typeof(Dock), typeof(LabeledImage));

        /// <summary>
        /// Initializes static members of the <see cref="LabeledImage"/> class.
        /// </summary>
        static LabeledImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledImage), new FrameworkPropertyMetadata(typeof(LabeledImage)));
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image dock.
        /// </summary>
        public Dock ImageDock
        {
            get { return (Dock)this.GetValue(ImageDockProperty); }
            set { this.SetValue(ImageDockProperty, value); }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Content != null ? this.Content.ToString() : string.Empty;
        }
    }
}
