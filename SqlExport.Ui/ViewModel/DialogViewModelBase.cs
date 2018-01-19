using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SqlExport.Ui.Business;

namespace SqlExport
{
	public abstract class DialogViewModelBase : ViewModelBase
	{
		public DialogViewModelBase()
		{
			InitialiseWindowCommand = new RelayCommand<Window>( InitialiseWindow );
		}

		public RelayCommand<Window> InitialiseWindowCommand { get; private set; }

		private void InitialiseWindow( Window window )
		{
			// DELETE
			////window.AutoScaleDimensions = new SizeF( 6F, 13F );
			////window.AutoScaleMode = AutoScaleMode.Font;
			////window.Width = 292 - SystemParameters.FixedFrameVerticalBorderWidth * 2;
			////window.Height = 92 - SystemParameters.FixedFrameHorizontalBorderHeight * 2 - SystemParameters.CaptionHeight;
			
			////window.RemoveControlBox();
			window.WindowStyle = WindowStyle.ToolWindow;
			window.ResizeMode = ResizeMode.CanResize;
			window.Name = "Dialog";
			window.RemoveIcon();
			window.ShowInTaskbar = false;
			window.Title = "Dialog";
			window.Topmost = true;

			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			// DELETE in favour of centered position
			////WindowExtensions.POINT mousePosition;
			////if( !DesignerProperties.GetIsInDesignMode( window ) && WindowExtensions.GetCursorPos(out mousePosition) )
			////{
			////    int x = Math.Min( Math.Max( mousePosition.X - window.Width / 2, 0 ),
			////        SystemInformation.MaxWindowTrackSize.Width - window.Width );
			////    int y = Math.Min( Math.Max( mousePosition.Y - window.Height / 2, 0 ),
			////        SystemInformation.MaxWindowTrackSize.Height - window.Height - 80 );
			////    window.Left = x;
			////    window.Top = y;
			////}
		}
	}
}
