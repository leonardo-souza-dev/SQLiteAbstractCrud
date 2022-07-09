using SQLiteAbstractCrud;
using System;

namespace AbstractCrud
{
    public class PersonRepository : RepositoryBase<Person>
    {
        public PersonRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }
}
