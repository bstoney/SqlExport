using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlExport.Ui.ViewModel;

namespace SqlExport.Messages.StatusPanel
{
	public class SetLinesMessage
	{
		public SetLinesMessage( SelectedTextRange selectedTextRange )
		{
			this.SelectedTextRange = selectedTextRange;
		}

		public SelectedTextRange SelectedTextRange { get; set; }
	}
}
