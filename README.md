# SQLiteAbstractCrud

Use:

```
using System;
using System.Collections.Generic;
using System.Linq;
using SQLiteAbstractCrud;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            PersonRepository personRepository = new ("./my-db.db");

            Console.WriteLine("\r\nSimple insert");
            personRepository.Insert(new Person(false, "Bob"));
            var person = personRepository.GetAll().First();
            Console.WriteLine(person.Name);

            Console.WriteLine("\r\nBatch insert");
            var persons = new List<Person>
            {
                new Person(false, "Mary"),
                new Person(false, "John")
            };
            personRepository.InsertBatch(persons);
            personRepository.GetAll().ToList().ForEach(person => Console.WriteLine(person.Name));
        }
    }


    // repository
    
    public class PersonRepository : RepositoryBase<Person>
    {
        public PersonRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }



    // entity

    public class Person
    {
        [PrimaryKey] // required
        [AutoIncrement] // optional
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDriver { get; set; }

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


```
