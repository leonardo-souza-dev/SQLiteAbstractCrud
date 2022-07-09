using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCrud
{
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
}
