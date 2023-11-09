namespace NetMapper.Interfaces;

public interface IDataStore<TData> where TData : new()
{
    public TData GetData();
    public void Update(TData updatedStore);
}