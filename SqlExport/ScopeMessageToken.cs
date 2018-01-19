namespace SqlExport
{
    using System;
    using System.Windows;

    /// <summary>
    /// Defines the ScopeMessageToken class.
    /// </summary>
    public static class ScopeMessageToken
    {
        public static Guid GetMessageToken(DependencyObject obj)
        {
            return (Guid)obj.GetValue(MessageTokenProperty);
        }

        public static void SetMessageToken(DependencyObject obj, Guid value)
        {
            obj.SetValue(MessageTokenProperty, value);
        }

        public static readonly DependencyProperty MessageTokenProperty =
            DependencyProperty.RegisterAttached(
                "MessageToken", typeof(Guid), typeof(ScopeMessageToken), new FrameworkPropertyMetadata(Guid.Empty));
    }
}
