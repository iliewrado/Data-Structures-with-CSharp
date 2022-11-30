using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MobileX
{
    public class VehicleRepository : IVehicleRepository
    {
        private Dictionary<string, Vehicle> vehicles;
        private Dictionary<string, List<Vehicle>> sellers;
        private Dictionary<string, List<Vehicle>> byBrand;

        public VehicleRepository()
        {
            this.vehicles = new Dictionary<string, Vehicle>();
            this.sellers = new Dictionary<string, List<Vehicle>>();
            this.byBrand = new Dictionary<string, List<Vehicle>>();
        }

        public int Count => this.vehicles.Count;

        public void AddVehicleForSale(Vehicle vehicle, string sellerName)
        {
            this.vehicles.Add(vehicle.Id, vehicle);

            if (!this.sellers.ContainsKey(sellerName))
                this.sellers[sellerName] = new List<Vehicle>();

            if (!this.byBrand.ContainsKey(vehicle.Brand))
                this.byBrand[vehicle.Brand] = new List<Vehicle>();

            this.byBrand[vehicle.Brand].Add(vehicle);

            this.sellers[sellerName].Add(vehicle);
            vehicle.Seller = sellerName;
        }

        public Vehicle BuyCheapestFromSeller(string sellerName)
        {
            if (!this.sellers.ContainsKey(sellerName))
                throw new ArgumentException();

            Vehicle vehicle = sellers[sellerName]
                .OrderBy(x => x.Price)
                .FirstOrDefault();

            if (vehicle == null)
                throw new ArgumentException();

            this.RemoveVehicle(vehicle.Id);

            return vehicle;
        }

        public bool Contains(Vehicle vehicle)
        {
            return this.vehicles.ContainsKey(vehicle.Id);
        }

        public Dictionary<string, List<Vehicle>> GetAllVehiclesGroupedByBrand()
        {
            if (this.Count == 0)
                throw new ArgumentException();

            return this.byBrand
                .Where(x => x.Value.Count > 0)
                .ToDictionary(x => x.Key, y => y.Value
                .OrderBy(z => z.Price).ToList());
        }

        public IEnumerable<Vehicle> GetAllVehiclesOrderedByHorsepowerDescendingThenByPriceThenBySellerName()
        {
            return this.vehicles.Values
                .OrderByDescending(x => x.Horsepower)
                .ThenBy(x => x.Price)
                .ThenBy(x => x.Seller);
        }

        public IEnumerable<Vehicle> GetVehicles(List<string> keywords)
        {
            return vehicles.Values
                .Where(x => keywords.Contains(x.Brand)
                || keywords.Contains(x.Model)
                || keywords.Contains(x.Location)
                || keywords.Contains(x.Color))
                .OrderBy(x => !x.IsVIP)
                .ThenBy(x => x.Price);
        }

        public IEnumerable<Vehicle> GetVehiclesBySeller(string sellerName)
        {
            if (!this.sellers.ContainsKey(sellerName))
                throw new ArgumentException();

            return this.sellers[sellerName];
        }

        public IEnumerable<Vehicle> GetVehiclesInPriceRange(double lowerBound, double upperBound)
        {
            return this.vehicles.Values
                .Where(x => x.Price >= lowerBound && x.Price <= upperBound)
                .OrderByDescending(x => x.Horsepower);
        }

        public void RemoveVehicle(string vehicleId)
        {
            if (!this.vehicles.ContainsKey(vehicleId))
                throw new ArgumentException();
            Vehicle vehicle = this.vehicles[vehicleId];
            this.sellers[vehicle.Seller].Remove(vehicle);
            this.byBrand[vehicle.Brand].Remove(vehicle);
            this.vehicles.Remove(vehicleId);
        }
    }
}
