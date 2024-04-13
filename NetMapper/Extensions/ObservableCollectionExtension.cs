using System.Collections.ObjectModel;

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
