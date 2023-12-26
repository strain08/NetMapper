using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Extensions;

public static class ObservableCollectionExtension
{
    public static void Refresh<T>(this ObservableCollection<T> value) where T : new()
    {
        var temp = new T();
        value.Add(temp);
        value.Remove(temp);
    }
}
