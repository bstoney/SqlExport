namespace SqlExport.SampleData
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the SampleMainWindowViewModel class.
    /// </summary>
    public class SampleMainWindowViewModel
    {
        public SampleMainWindowViewModel()
        {
            this.Queries = new ObservableCollection<SampleQueryViewModel>()
                {
                    new SampleQueryViewModel(),
                    new SampleQueryViewModel()
                };
        }

        public ObservableCollection<SampleQueryViewModel> Queries { get; set; }
    }
}
