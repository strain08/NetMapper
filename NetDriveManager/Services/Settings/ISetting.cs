namespace NetMapper.Services.Settings
{
    internal interface ISetting
    {
        public void Apply();
        public void Configure(ref object obj);
    }
}