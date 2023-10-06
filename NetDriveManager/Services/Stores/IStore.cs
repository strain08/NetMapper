namespace NetMapper.Services.Stores
{
    public interface IStore<TData> where TData : new()
    {
        public TData GetAll();
        public bool Load();
        public bool Save();
        public bool Update(TData updatedStore);

    }
}
