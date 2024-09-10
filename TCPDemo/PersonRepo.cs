using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TCPDemo
{
    public static class PersonRepo
    {
        public static List<Person> Persons {  get; set; } = new List<Person>();

        public static Person? Get(int index)
        {
            try
            {
                return Persons[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            
        }
        public static Person? Delete(int index)
        {
            try
            {
                Person person = Persons[index];
                Persons.RemoveAt(index);
                return person;

            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }

        }
        public static Person? Update(int index, Person replacement)
        {
            try
            {
                Persons[index] = replacement;
                return Persons[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
        public static void Add(Person person)
        {
            Persons.Add(person);
            

        }
        public static Person? ProcessObject(ParseObject parseObj)
        {
            if (parseObj == null) return null;
            if (parseObj.Command == "update")
            {
                Person? person = Get(parseObj.Index);
                if (person == null)
                {
                    throw new ArgumentException($"index {parseObj.Index} does not exist");
                }
                foreach (var keyValuePair in parseObj.PropertyData)
                {
                    switch (keyValuePair.Key)
                    {
                        case "name":
                            person.Name = keyValuePair.Value;
                            break;
                        case "address":
                            person.Address = keyValuePair.Value;
                            break;
                        case "phone":
                            person.Phone = keyValuePair.Value;
                            break;

                    }                   
                }
                Update(parseObj.Index, person);
                return person;

            }
            else if (parseObj.Command == "delete")
            {
                Person? person = Delete(parseObj.Index);
                if (person == null)
                {
                    throw new ArgumentException($"index {parseObj.Index} does not exist");
                }
                else
                {
                    return person;
                }
            }
            else if (parseObj.Command == "get")
            {
                Person? person = Get(parseObj.Index);
                if (person == null)
                {
                    throw new ArgumentException($"index {parseObj.Index} does not exist");
                }
                else
                {
                    return person;
                }
            }
            else if (parseObj.Command == "add")
            {
                Person? person = new Person();
                foreach (var keyValuePair in parseObj.PropertyData)
                {
                    switch (keyValuePair.Key)
                    {
                        case "name":
                            person.Name = keyValuePair.Value;
                            break;
                        case "address":
                            person.Address = keyValuePair.Value;
                            break;
                        case "phone":
                            person.Phone = keyValuePair.Value;
                            break;
                    }
                }
                Add(person);
                return person;
            }
            return null;

        }
    }
}
