using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlExport.Messages
{
	public class DisplayMessage
	{
		public readonly static DisplayMessage Separator = new DisplayMessage( MessageItemViewModel.SeparatorText );

		public DisplayMessage( string text )
		{
			if( string.IsNullOrEmpty( text ) )
			{
				throw new ArgumentException( "Text is required for a display message." );
			}

			Text = text;
		}

		public DisplayMessage( string text, string details, DisplayMessageType type, Action callback )
			: this( text )
		{
			Details = details;
			Type = type;
			Callback = callback;
		}

		/// <summary>
		/// Gets or sets the text to display in the <see cref="MessageList"/>.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets any additional details of the message.
		/// </summary>
		public string Details { get; set; }

		/// <summary>
		/// Gets or sets the type of message.
		/// </summary>
		public DisplayMessageType Type { get; set; }

		/// <summary>
		/// Gets or set a callbak method which will be call when the message is selected.
		/// </summary>
		public Action Callback { get; set; }

		public static DisplayMessage FromException( Exception ex )
		{
			return new DisplayMessage( ex.Message, ex.ToString(), DisplayMessageType.Error, null );
		}
	}
}
