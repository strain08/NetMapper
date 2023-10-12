namespace NetMapper.Services.Stores
{
    public interface IStore<TData> where TData : new()
    {
        public TData GetAll();
        public void Load();
        public void Save();
        public void Update(TData updatedStore);

    }
}
