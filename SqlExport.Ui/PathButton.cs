using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SqlExport
{
    public class PathButton : Button
    {
        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }

        public PathButton()
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("pack://application:,,,/SQLExport.UI;component/Resources/Styles/Buttons.xaml");
            Resources.MergedDictionaries.Add(rd);

            Style = TryFindResource("PathButtonStyle") as Style;
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(Geometry), typeof(PathButton));

        public Geometry Path
        {
            get { return (Geometry)base.GetValue(PathProperty); }
            set { base.SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty EnabledFillProperty =
            DependencyProperty.Register("EnabledFill", typeof(Brush), typeof(PathButton));

        public Brush EnabledFill
        {
            get { return (Brush)base.GetValue(EnabledFillProperty); }
            set { base.SetValue(EnabledFillProperty, value); }
        }

        public static readonly DependencyProperty DisabledFillProperty =
            DependencyProperty.Register("DisabledFill", typeof(Brush), typeof(PathButton));

        public Brush DisabledFill
        {
            get { return (Brush)base.GetValue(DisabledFillProperty); }
            set { base.SetValue(DisabledFillProperty, value); }
        }

        public static readonly DependencyProperty PressedFillProperty =
            DependencyProperty.Register("PressedFill", typeof(Brush), typeof(PathButton));

        public Brush PressedFill
        {
            get { return (Brush)base.GetValue(PressedFillProperty); }
            set { base.SetValue(PressedFillProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Path path = base.GetTemplateChild("Path") as Path;
            if (path != null)
            {
                path.Data = Path;
            }
        }
    }
}
