namespace _01.DogVet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DogVet : IDogVet
    {
        Dictionary<string, Dog> dogs;
        Dictionary<string, Dictionary<string, Dog>> dogOwners;

        public DogVet()
        {
            this.dogs = new Dictionary<string, Dog>();
            this.dogOwners = new Dictionary<string, Dictionary<string, Dog>>();
        }

        public int Size => this.dogs.Count;

        public void AddDog(Dog dog, Owner owner)
        {
            if (this.dogs.ContainsKey(dog.Id) 
                || this.dogOwners.ContainsKey(owner.Id)
                && this.dogOwners[owner.Id].ContainsKey(dog.Name))
                throw new ArgumentException();

            dog.Owner = owner;
            dogs.Add(dog.Id, dog);

            if (!this.dogOwners.ContainsKey(owner.Id))
                this.dogOwners[owner.Id] = new Dictionary<string, Dog>();

            this.dogOwners[owner.Id].Add(dog.Name, dog);
        }

        public bool Contains(Dog dog)
        {
            return this.dogs.ContainsKey(dog.Id);
        }

        public Dog GetDog(string name, string ownerId)
        {
            if (!this.dogOwners.ContainsKey(ownerId)
                || !this.dogOwners[ownerId].ContainsKey(name))
                throw new ArgumentException();

            return this.dogOwners[ownerId][name];
        }

        public Dog RemoveDog(string name, string ownerId)
        {
            if (!this.dogOwners.ContainsKey(ownerId)
                || !this.dogOwners[ownerId].ContainsKey(name))
                throw new ArgumentException();

            Dog dog = this.dogOwners[ownerId][name];
            this.dogOwners[ownerId].Remove(name);
            this.dogs.Remove(dog.Id);

            return dog;
        }

        public IEnumerable<Dog> GetDogsByOwner(string ownerId)
        {
            if (!this.dogOwners.ContainsKey(ownerId))
                throw new ArgumentException();

            return this.dogOwners[ownerId].Values;
        }

        public IEnumerable<Dog> GetDogsByBreed(Breed breed)
        {
            List<Dog> byBreed = this.dogs.Values
                .Where(x => x.Breed == breed)
                .ToList();

            return byBreed.Count > 0 ? byBreed
                : throw new ArgumentException();
        }

        public void Vaccinate(string name, string ownerId)
        {
            if (!this.dogOwners.ContainsKey(ownerId) 
                || !this.dogOwners[ownerId].ContainsKey(name))
                throw new ArgumentException();

            this.dogOwners[ownerId][name].Vaccines++;
        }

        public void Rename(string oldName, string newName, string ownerId)
        {
            if (!this.dogOwners.ContainsKey(ownerId)
                || !this.dogOwners[ownerId].ContainsKey(oldName))
                throw new ArgumentException();

            Dog dog = this.dogOwners[ownerId][oldName];
            this.dogOwners[ownerId].Remove(oldName);
            dog.Name = newName;
            this.dogOwners[ownerId].Add(dog.Name, dog);
        }

        public IEnumerable<Dog> GetAllDogsByAge(int age)
        {
            List<Dog> dogsByage = this.dogs.Values
                .Where(x => x.Age == age)
                .ToList();

            return dogsByage.Count > 0 ? dogsByage : throw new ArgumentException();
        }

        public IEnumerable<Dog> GetDogsInAgeRange(int lo, int hi)
        {
            return this.dogs.Values
                .Where(x => x.Age >= lo && x.Age <= hi);
        }

        public IEnumerable<Dog> GetAllOrderedByAgeThenByNameThenByOwnerNameAscending()
        {
            return this.dogs.Values
                .OrderBy(x => x.Age)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Owner.Name);
        }
    }
}