using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _02Vasylenko
{
    static class DBAdapter
    {
        public static List<Person> Persons { get; }

        static DBAdapter()
        {
            var filepath = Path.Combine(GetAndCreateDataPath(),Person.Filename);
            if (File.Exists(filepath))
            {
                try
                {
                    Persons = SerializeHelper.Deserialize<List<Person>>(filepath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to get users from file.{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }
            else
            {
                Persons = new List<Person>();
                Random rnd = new Random();
                string[] lastNames = { "Nesterov", "Vasylenko", "Shudra", "Fomin", "Kundik", "Margal", "Hitler", "Obama", "Trump", "Nazar" };
                string[] firstNames = { "Taras", "Max", "Kirill", "Nik", "Vova", "Den", "Igor", "Taras", "Adolph", "Gena" };
                for (int i = 0; i < 50; i++)
                    Persons.Add(new Person(lastNames[rnd.Next(0,10)], firstNames[rnd.Next(0, 10)], $"user{i}@gmail.com", new DateTime(rnd.Next(DateTime.Today.Year - 135, DateTime.Today.Year-1), rnd.Next(1, 13), rnd.Next(1, 30))));
            }
        }
        internal static Person AddPerson(string lastName, string firstName, string email, DateTime date)
        {
            Person person = new Person(lastName, firstName, email, date);
            Persons.Add(person);
            return person;
        }
        internal static void SaveData()
        {
            SerializeHelper.Serialize(Persons, Path.Combine(GetAndCreateDataPath(), Person.Filename));
        }

        private static string GetAndCreateDataPath()
        {
            string dir = StationManager.WorkingDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
