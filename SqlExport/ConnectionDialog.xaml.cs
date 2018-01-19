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

namespace SqlExport
{
	public class Property
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}

	/// <summary>
	/// Interaction logic for ConnectionDialog.xaml
	/// </summary>
	public partial class ConnectionDialog : Window
	{
		public ConnectionDialog()
		{
			InitializeComponent();
		}

		internal static Property[] GetProperties()
		{
			return new[] { 
				new Property() { Name = "Name", Value = null },
				new Property() { Name = "Value", Value = null }
			};
		}
	}
}
