namespace _01.Inventory
{
    using _01.Inventory.Interfaces;
    using _01.Inventory.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Inventory : IHolder
    {
        private List<IWeapon> weapons;

        public Inventory()
        {
            this.weapons = new List<IWeapon>();
        }

        public int Capacity => this.weapons.Count;

        public void Add(IWeapon weapon)
        {
            this.weapons.Add(weapon);
        }

        public void Clear()
        {
            this.weapons.Clear();
        }

        public bool Contains(IWeapon weapon)
        {
            return this.weapons.Contains(weapon);
        }

        public void EmptyArsenal(Category category)
        {
            foreach (var item in weapons.Where(x => x.Category == category))
            {
                item.Ammunition = 0;
            }
        }

        public bool Fire(IWeapon weapon, int ammunition)
        {
            this.EnsureExist(weapon);

            IWeapon tofire = this.weapons.First(x => x.Equals(weapon));

            if (tofire.Ammunition < ammunition)
            {
                return false;
            }

            tofire.Ammunition -= ammunition;
            return true;
        }

        public IWeapon GetById(int id)
        {
            return this.weapons.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerator GetEnumerator()
        {
            return this.weapons.GetEnumerator();
        }

        public int Refill(IWeapon weapon, int ammunition)
        {
            this.EnsureExist(weapon);

            IWeapon toRefill = this.weapons.First(x => x.Equals(weapon));

            toRefill.Ammunition += ammunition;
            if (toRefill.Ammunition > toRefill.MaxCapacity)
            {
                toRefill.Ammunition = toRefill.MaxCapacity;
            }

            return toRefill.Ammunition;
        }

        public IWeapon RemoveById(int id)
        {
            IWeapon weapon = this.weapons.FirstOrDefault(x => x.Id == id);
            this.EnsureExist(weapon);
            this.weapons.Remove(weapon);
            return weapon;
        }

        public int RemoveHeavy()
        {
            List<IWeapon> temp = new List<IWeapon>();

            for (int i = 0; i < this.weapons.Count; i++)
            {
                if (weapons[i].Category == Category.Heavy)
                {
                    temp.Add(weapons[i]);
                }
            }

            foreach (var item in temp)
            {
                weapons.RemoveAll(x => x.Category == Category.Heavy);
            }

            return temp.Count;
        }

        public List<IWeapon> RetrieveAll()
        {
            return new List<IWeapon>(this.weapons);
        }

        public List<IWeapon> RetriveInRange(Category lower, Category upper)
        {
            List<IWeapon> result = new List<IWeapon>();

            foreach (var weapon in weapons)
            {
                if (weapon.Category >= lower && weapon.Category <= upper)
                {
                    result.Add(weapon);
                }
            }

            return result;
        }

        public void Swap(IWeapon firstWeapon, IWeapon secondWeapon)
        {
            this.EnsureExist(firstWeapon);
            this.EnsureExist(secondWeapon);

            int indexOfFirst = this.weapons.IndexOf(firstWeapon);
            int indexOfSecond = this.weapons.IndexOf(secondWeapon);

            if (weapons[indexOfFirst].Category == weapons[indexOfSecond].Category)
            {
                IWeapon temp = this.weapons[indexOfFirst];
                this.weapons[indexOfFirst] = this.weapons[indexOfSecond];
                this.weapons[indexOfSecond] = temp;
            }
        }

        private void EnsureExist(IWeapon weapon)
        {
            if (!weapons.Contains(weapon))
                throw new InvalidOperationException("Weapon does not exist in inventory!");
        }
    }
}
