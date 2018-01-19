using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Messages;

namespace SqlExport.ViewModel
{
	public class FilePropertyItem : PropertyItem
	{
		private string _fileFilter;
		private TextBox _txtFilename;

		public FilePropertyItem( string category, string name, string fileFilter )
			: base( category, name )
		{
			_fileFilter = fileFilter;
		}

		public override FrameworkElement GetEditControl( Binding binding )
		{
			var filePanel = new DockPanel();

			var button = new Button();
			button.SetValue( DockPanel.DockProperty, Dock.Right );
			button.Content = " ... ";
			button.Click += new RoutedEventHandler( button_Click );
			filePanel.Children.Add( button );

			_txtFilename = new TextBox();
			_txtFilename.SetBinding( TextBox.TextProperty, binding );
			filePanel.Children.Add( _txtFilename );

			return filePanel;
		}

		private void button_Click( object sender, RoutedEventArgs e )
		{
			ShowSaveFileDialog();
		}

		public bool ShowSaveFileDialog()
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.FileName = _txtFilename.Text;
			saveFile.Filter = _fileFilter;

			try
			{
				saveFile.InitialDirectory = Path.GetDirectoryName( saveFile.FileName );
			}
			catch( Exception ex )
			{
				Messenger.Default.Send( new UnhandledExceptionMessage() { Exception = ex } );
			}

			if( saveFile.ShowDialog() ?? false )
			{
				_txtFilename.Text = saveFile.FileName;
				return true;
			}

			return false;
		}
	}
}
