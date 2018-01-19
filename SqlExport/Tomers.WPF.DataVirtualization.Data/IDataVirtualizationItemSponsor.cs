using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomers.WPF.DataVirtualization.Data
{
    public interface IDataVirtualizationItemSponsor
    {
        void ExtendItems(IEnumerable<object> items);
        void DeflateItem(object item);
    }
}
