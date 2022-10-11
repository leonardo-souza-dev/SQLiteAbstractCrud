# SQLiteAbstractCrud

Use:

```
using System;
using System.Collections.Generic;
using System.Linq;
using SQLiteAbstractCrud;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace Sample
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 1. Create a repository class with RepositoryBase<T> as base
            // 2. Pass the path of the file db as constructor
            PersonRepository personRepository = new ("./my-db.db");

            Console.WriteLine("\r\nSimple insert");
            personRepository.Insert(new Person(1, false, "Bob")); // insert
            var person = personRepository.GetAll().First(); // getAll
            Console.WriteLine(person.Name);

            Console.WriteLine("\r\nBatch insert");
            var persons = new List<Person>
            {
                new Person(2, false, "Mary"),
                new Person(3, false, "John")
            };
            personRepository.InsertBatch(persons); // insertBatch
            var people = personRepository.GetAll().ToList(); // getAll
            people.ForEach(person => Console.WriteLine(person.Name));

            Console.WriteLine("\r\nDelete");
            personRepository.Delete(people[0].Id); // delete
            personRepository.GetAll().ToList().ForEach(person => Console.WriteLine(person.Name));// getAll

            Console.ReadKey();
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
        public int Id { get; set; }
        public bool IsDriver { get; set; }
        public string Name { get; set; }

        public Person(int id, bool isDriver, string name)
        {
            Id = id;
            IsDriver = isDriver;
            Name = name;
        }
    }
}



```
