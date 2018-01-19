using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;

namespace SqlExport.ViewModel
{
	public class ApplicationMessagesViewModel : DialogViewModelBase
	{
		public ApplicationMessagesViewModel()
		{
			ScopeToken = Guid.NewGuid().ToString();
			Messenger.Default.Register<ApplicationDisplayMessage>( this, MessageReceived );
		}

		public string ScopeToken { get; private set; }

		private void MessageReceived( ApplicationDisplayMessage message )
		{
			Messenger.Default.Send( (DisplayMessage)message, ScopeToken );
		}
	}
}
