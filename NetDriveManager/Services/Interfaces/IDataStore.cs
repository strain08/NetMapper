﻿namespace NetMapper.Services.Stores;

public interface IDataStore<TData> where TData : new()
{
    public TData GetData();
    public void Update(TData updatedStore);
}