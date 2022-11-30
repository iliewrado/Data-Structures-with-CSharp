namespace _01.Microsystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Microsystems : IMicrosystem
    {
        private Dictionary<int, Computer> computeres;

        public Microsystems()
        {
            this.computeres = new Dictionary<int, Computer>();
        }

        public void CreateComputer(Computer computer)
        {
            if (this.computeres.ContainsKey(computer.Number))
                throw new ArgumentException();

            this.computeres.Add(computer.Number, computer);
        }

        public bool Contains(int number)
        {
            return this.computeres.ContainsKey(number);
        }

        public int Count()
            => this.computeres.Count;

        public Computer GetComputer(int number)
        {
            if (!this.computeres.ContainsKey(number))
                throw new ArgumentException();

            return this.computeres[number];
        }

        public void Remove(int number)
        {
            if (!this.computeres.ContainsKey(number))
                throw new ArgumentException();

            this.computeres.Remove(number);
        }

        public void RemoveWithBrand(Brand brand)
        {
            List<Computer> temp = this.computeres.Values
                .Where(x => x.Brand == brand)
                .ToList();

            if (temp.Count == 0)
                throw new ArgumentException();

            foreach (var comp in temp)
            {
                this.computeres.Remove(comp.Number);
            }
        }

        public void UpgradeRam(int ram, int number)
        {
            if (!this.computeres.ContainsKey(number))
                throw new ArgumentException();

            if (this.computeres[number].RAM < ram)
                this.computeres[number].RAM = ram;
        }

        public IEnumerable<Computer> GetAllFromBrand(Brand brand)
        {
            return this.computeres.Values
                .Where(x => x.Brand == brand)
                .OrderByDescending(x => x.Price);
        }

        public IEnumerable<Computer> GetAllWithScreenSize(double screenSize)
        {
            return this.computeres.Values
                .Where(x => x.ScreenSize == screenSize)
                .OrderByDescending(x => x.Number);
        }

        public IEnumerable<Computer> GetAllWithColor(string color)
        {
            return this.computeres.Values
                .Where(x => x.Color == color)
                .OrderByDescending(x => x.Price);
        }

        public IEnumerable<Computer> GetInRangePrice(double minPrice, double maxPrice)
        {
            return this.computeres
                .Where(x => x.Value.Price >= minPrice && x.Value.Price <= maxPrice)
                .OrderByDescending(x => x.Value.Price)
                .Select(x=>x.Value)
                .ToList();
        }
    }
}
