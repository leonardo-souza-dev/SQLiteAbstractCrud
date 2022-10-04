namespace SQLiteAbstractCrud.Proxy
{
    public sealed class OriginalPropertyInfo
    {
        public int Order { get; }
        public string Name { get; }

        public OriginalPropertyInfo(int order, string name)
        {
            this.Order = order;
            this.Name = name;
        }
    }
}
