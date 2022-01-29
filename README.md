# SQLiteAbstractCrud

Use:

    public class MyEntityRepository : RepositoryBase<MyEntity>
    {
        public MyEntityRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }
