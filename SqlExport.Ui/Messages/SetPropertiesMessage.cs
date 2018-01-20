namespace SqlExport.Messages
{
    using System.Collections.Generic;

    using SqlExport.ViewModel;

    public class SetPropertiesMessage
    {
        public SetPropertiesMessage(IEnumerable<PropertyItem> properties)
        {
            Properties = properties;
        }

        public IEnumerable<PropertyItem> Properties { get; set; }
    }
}
