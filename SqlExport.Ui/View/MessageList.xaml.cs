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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqlExport.Ui.ViewModel;

namespace SqlExport.View
{
	/// <summary>
	/// Handler for item selected events.
	/// </summary>
	public delegate void ErrorSelectedHandler( object sender, int lineNumber );

	/// <summary>
	/// A control to display a list of errors or messages and associated details.
	/// </summary>
	public partial class MessageList : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the ErrorList class.
		/// </summary>
		public MessageList()
		{
			InitializeComponent();

		    System.Diagnostics.PresentationTraceSources.SetTraceLevel(
		        lstErrors.ItemContainerGenerator, System.Diagnostics.PresentationTraceLevel.High);
		}

		public MessageListViewModel ViewModel { get { return DataContext as MessageListViewModel; } }

		/// <summary>
		/// The <see cref="ScopeToken" /> dependency property's name.
		/// </summary>
		public const string ScopeTokenPropertyName = "ScopeToken";

		/// <summary>
		/// Gets or sets the value of the <see cref="ScopeToken" />
		/// property. This is a dependency property.
		/// </summary>
		public string ScopeToken
		{
			get
			{
				return (string)GetValue( ScopeTokenProperty );
			}
			set
			{
				var vm = DataContext as MessageListViewModel;
				vm.ScopeToken = value;

				SetValue( ScopeTokenProperty, value );
			}
		}

		/// <summary>
		/// Identifies the <see cref="ScopeToken" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ScopeTokenProperty = DependencyProperty.Register(
			ScopeTokenPropertyName,
			typeof( string ),
			typeof( MessageList ),
			new UIPropertyMetadata( null ) );
	}
}