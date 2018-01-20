namespace SqlExport.SampleData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SampleQueryViewModel
    {
        private static int counter;

        public SampleQueryViewModel()
        {
            this.DisplayText = "Sample " + counter++;
        }

        public string DisplayText { get; set; }
    }
}
