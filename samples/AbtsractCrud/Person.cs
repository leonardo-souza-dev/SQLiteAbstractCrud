using SQLiteAbstractCrud;

namespace AbstractCrud
{
    public class Person
    {
        [PrimaryKey] // required
        [AutoIncrement] // optional
        public int Id { get; set; }
        public bool IsDriver { get; set; }
        public string Name { get; set; }

        // required constructor with all properties
        public Person(int id, bool isDriver, string name)
        {
            Id = id;
            IsDriver = isDriver;
            Name = name;
        }

        public Person(bool isDriver, string name)
        {
            IsDriver = isDriver;
            Name = name;
        }
    }
}
