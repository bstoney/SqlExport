using System;
using System.Collections.Generic;
using System.Text;
using SqlExport.Ui.Properties;
using System.Drawing;
using GalaSoft.MvvmLight;

namespace SqlExport
{
	public class MessageItemViewModel : ViewModelBase
	{
		public static readonly string SeparatorText = "--";

		/// <summary>
		/// The <see cref="Occurrences" /> property's name.
		/// </summary>
		public const string OccurrencesPropertyName = "Occurrences";

		/// <summary>
		/// The <see cref="IsSelected" /> property's name.
		/// </summary>
		public const string IsSelectedPropertyName = "IsSelected";

		/// <summary>
		/// The <see cref="Tag" /> property's name.
		/// </summary>
		public const string TagPropertyName = "Tag";

		/// <summary>
		/// The <see cref="Text" /> property's name.
		/// </summary>
		public const string TextPropertyName = "Text";

		/// <summary>
		/// The <see cref="Image" /> property's name.
		/// </summary>
		public const string ImagePropertyName = "Image";

		/// <summary>
		/// The <see cref="IsSeparator" /> property's name.
		/// </summary>
		public const string IsSeparatorPropertyName = "IsSeparator";

		/// <summary>
		/// The <see cref="DisplayText" /> property's name.
		/// </summary>
		public const string DisplayTextPropertyName = "DisplayText";

		private string _displayText = null;
		private Image _image = null;
		private string _text = null;
		private object _tag = null;
		private bool _isSelected = false;
		private int _occurrences = 0;

		public MessageItemViewModel( Image image, string text, object tag )
		{
			Image = image;
			Text = text;
			Tag = tag;
			Occurrences = 1;
		}

		/// <summary>
		/// Gets the Image property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public Image Image
		{
			get
			{
				return _image;
			}

			set
			{
				if( _image == value )
				{
					return;
				}

				_image = value;

				// Update bindings, no broadcast
				RaisePropertyChanged( ImagePropertyName );
			}
		}

		/// <summary>
		/// Gets the Text property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}

			set
			{
				if( _text == value )
				{
					return;
				}

				_text = value;

				// Update bindings, no broadcast
				RaisePropertyChanged( TextPropertyName );
				RaisePropertyChanged( IsSeparatorPropertyName );
				RaisePropertyChanged( DisplayTextPropertyName );
			}
		}

		/// <summary>
		/// Gets the Tag property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public object Tag
		{
			get
			{
				return _tag;
			}

			set
			{
				if( _tag == value )
				{
					return;
				}

				_tag = value;

				// Update bindings, no broadcast
				RaisePropertyChanged( TagPropertyName );
			}
		}

		/// <summary>
		/// Gets or sets the Occurrences property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public int Occurrences
		{
			get
			{
				return _occurrences;
			}

			set
			{
				if( _occurrences != value )
				{
					_occurrences = value;

					// Update bindings, no broadcast
					RaisePropertyChanged( OccurrencesPropertyName );
					RaisePropertyChanged( DisplayTextPropertyName );
				}
			}
		}

		/// <summary>
		/// Gets or sets the DisplayText property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public string DisplayText
		{
			get
			{
				if( Occurrences < 2 )
				{
					return Text;
				}
				else
				{
					return string.Format( "{0} ({1} occurrences)", Text, Occurrences );
				}
			}
		}

		public bool IsSeparator
		{
			get { return Text == SeparatorText; }
		}

		/// <summary>
		/// Gets the IsSelected property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}

			set
			{
				if( _isSelected == value || IsSeparator )
				{
					return;
				}

				_isSelected = value;

				// Update bindings, no broadcast
				RaisePropertyChanged( IsSelectedPropertyName );
			}
		}

		public bool Equals( MessageItemViewModel obj )
		{
			return (object)obj != null &&
				Image == obj.Image &&
				Text == obj.Text &&
				object.Equals( Tag, obj.Tag );
		}

		public override string ToString()
		{
			return (Occurrences <= 1 ? Text : Text + " (Repeated " + Occurrences + " time(s))");
		}
	}
}
