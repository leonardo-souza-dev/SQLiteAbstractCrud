# SQLiteAbstractCrud

Use:

    public class MyEntityRepository : RepositoryBase<MyEntity>
    {
        public MyEntityRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            PersonRepository personRepository = new PersonRepository("./my-db.db");

            Console.WriteLine("Simple insert");
            personRepository.Insert(new Person(false, "Bob"));
            var person = personRepository.GetAll().First();
            Console.WriteLine(person.Name);

            Console.WriteLine("Batch insert");
            var persons = new List<Person>
            {
                new Person(false, "Mary"),
                new Person(false, "John")
            };
            personRepository.InsertBatch(persons);
            personRepository.GetAll().ToList().ForEach(person => Console.WriteLine(person.Name));
        }
    }

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

