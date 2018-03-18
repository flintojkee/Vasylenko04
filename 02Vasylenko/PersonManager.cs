using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace _02Vasylenko
{
    public class PersonManager
    {
        readonly PersonRepository _personRepository;
        public PersonManager()
        {
            _personRepository = new PersonRepository();
        }

        public bool Add(Person person)
        {
            return true;
        }
    }
}
