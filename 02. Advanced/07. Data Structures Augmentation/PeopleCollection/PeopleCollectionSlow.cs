namespace CollectionOfPeople
{
    using System.Collections.Generic;
    using System.Linq;

    public class PeopleCollectionSlow : IPeopleCollection
    {
        List<Person> people = new List<Person>();

        public int Count =>
            this.people.Count;

        public bool Add(string email, string name, int age, string town)
        {
            if (this.Find(email) != null)
                return false;

            Person person = new Person(email, name, age, town);
            people.Add(person);
            return true;
        }

        public bool Delete(string email)
            => this.people.Remove(this.Find(email));

        public Person Find(string email)
            => this.people.FirstOrDefault(x => x.Email == email);

        public IEnumerable<Person> FindPeople(string emailDomain)
            => this.people
            .Where(x => x.Email.EndsWith($"@{emailDomain}"))
            .OrderBy(x => x.Email);

        public IEnumerable<Person> FindPeople(string name, string town)
            => this.people
            .Where(x => x.Name == name && x.Town == town)
            .OrderBy(x => x.Email);

        public IEnumerable<Person> FindPeople(int startAge, int endAge)
            => this.people
            .Where(x => x.Age >= startAge && x.Age <= endAge)
            .OrderBy(x => x.Age)
            .ThenBy(x => x.Email);

        public IEnumerable<Person> FindPeople(int startAge, int endAge, string town)
            => this.people
            .Where(x => x.Age >= startAge && x.Age <= endAge && x.Town == town)
            .OrderBy(x => x.Age)
            .ThenByDescending(x => x.Town)
            .ThenBy(x => x.Email);
    }
}
