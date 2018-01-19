using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SqlExport.Ui.Messages;
using SqlExport.Data;
using DevZest.Windows.DataVirtualization;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace SqlExport.ViewModel
{
	public class DataViewViewModel : ViewModelBase ////, IVirtualListLoader<object>
	{
		/// <summary>
		/// The <see cref="ItemsSource" /> property's name.
		/// </summary>
		public const string ItemsSourcePropertyName = "ItemsSource";

		/// <summary>
		/// The <see cref="ItemCount" /> property's name.
		/// </summary>
		public const string ItemCountPropertyName = "ItemCount";

		private object _itemsSource = null;
		private int _itemCount = -1;
		private DataResult _originalResult;
		////private List<object> _resultList;
		////private SortDescription _lastSortDescription;
		////private Dictionary<SortDescription, List<object>> _previousSorts = new Dictionary<SortDescription, List<object>>();

		public DataViewViewModel()
		{
			Messenger.Default.Register<UnloadDataMessage>( this, this, m => UnloadData() );
			Messenger.Default.Register<SetDataResultMessage>( this, this, m => LoadData( m.Data ) );
			Messenger.Default.Register<InitialiseDataViewMessage>( this, this, m => m.CellValueCallback = _originalResult.FetchValue );
			Messenger.Default.Register<GetDataResultMessage>( this, this, m => m.GetDataResultCallback( _originalResult ) );
		}
		
		/// <summary>
		/// Gets or sets the ItemCount property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public int ItemCount
		{
			get
			{
				return _itemCount;
			}

			set
			{
				if( _itemCount != value )
				{
					_itemCount = value;

					// Update bindings, no broadcast
					RaisePropertyChanged( ItemCountPropertyName );
				}
			}
		}

		/// <summary>
		/// Gets or sets the ItemsSource property.
		/// TODO Update documentation:
		/// Changes to that property's value raise the PropertyChanged event. 
		/// This property's value is broadcasted by the Messenger's default instance when it changes.
		/// </summary>
		public object ItemsSource
		{
			get
			{
				return _itemsSource;
			}

			set
			{
				if( _itemsSource != value )
				{
					_itemsSource = value;

					// Update bindings, no broadcast
					RaisePropertyChanged( ItemsSourcePropertyName );
				}
			}
		}

		private void UnloadData()
		{
			_originalResult = null;
			ItemCount = 0;

			Messenger.Default.Unregister<UnloadDataMessage>( this );
			Messenger.Default.Unregister<SetDataResultMessage>( this );
			Messenger.Default.Unregister<InitialiseDataViewMessage>( this );
		}

		private void LoadData( DataResult dataResult )
		{
			_originalResult = dataResult;
			////_resultList = dataResult.AsEnumerable().OfType<object>().ToList();
			ItemCount = dataResult.FetchCount();
			////ItemsSource = _resultList; // new VirtualList<object>( this );
			ItemsSource = new Tomers.WPF.DataVirtualization.HugeCollection( dataResult.AsEnumerable() );
			////ItemsSource = dataResult.AsEnumerable();

			Messenger.Default.Send( new InitialiseDataViewMessage()
			{
				CellValueCallback = _originalResult.FetchValue,
				Columns = _originalResult.FetchColumns(),
				RowCount = _originalResult.FetchCount()
			}, this );
		}

		////#region IVirtualListLoader<object> Members

		////public bool CanSort
		////{
		////    get { return true; }
		////}

		////public IList<object> LoadRange( int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount )
		////{
		////    overallCount = ItemCount;

		////    SortDescription sortDescription = sortDescriptions == null || sortDescriptions.Count == 0 ? new SortDescription() : sortDescriptions[0];

		////    if( sortDescription != _lastSortDescription )
		////    {
		////        if( !_previousSorts.ContainsKey( sortDescription ) )
		////        {
		////            Func<object, object> sortvalue = o => TypeDescriptor.GetProperties( o ).OfType<PropertyDescriptor>().Where( p => p.Name == sortDescription.PropertyName ).Select( p => p.GetValue( o ) ).FirstOrDefault();

		////            IOrderedEnumerable<object> sorted;
		////            if( sortDescription.Direction == ListSortDirection.Ascending )
		////            {
		////                sorted = from o in _resultList
		////                         orderby sortvalue( o ) ascending
		////                         select o;
		////            }
		////            else
		////            {
		////                sorted = from o in _resultList
		////                         orderby sortvalue( o ) descending
		////                         select o;
		////            }

		////            _resultList = sorted.ToList();
		////            _previousSorts.Add( sortDescription, _resultList );
		////        }
		////        else
		////        {
		////            _resultList = _previousSorts[sortDescription];
		////        }

		////        _lastSortDescription = sortDescription;
		////    }

		////    return _resultList.Skip( startIndex ).Take( count ).ToList();
		////}

		////#endregion
	}
}
