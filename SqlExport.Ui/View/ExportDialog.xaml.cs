namespace SqlExport.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using GalaSoft.MvvmLight.Messaging;
    using SqlExport.Ui.Messages;

    /// <summary>
    /// Interaction logic for ExportDialog.xaml
    /// </summary>
    public partial class ExportDialog : DialogViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDialog"/> class.
        /// </summary>
        public ExportDialog()
        {
            this.InitializeComponent();
        }
    }
}
