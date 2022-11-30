using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private Dictionary<string, Route> routs;

        public MoovIt()
        {
            this.routs = new Dictionary<string, Route>();
        }

        public int Count => this.routs.Count;

        public void AddRoute(Route route)
        {
            if (this.routs.Any(x=>x.Value.Equals(route)))
                throw new ArgumentException();

            this.routs.Add(route.Id, route);
        }

        public void ChooseRoute(string routeId)
        {
            if (!this.routs.ContainsKey(routeId))
                throw new ArgumentException();

            this.routs[routeId].Popularity += 1;
        }

        public bool Contains(Route route)
        {
            return this.routs.Values
                .Any(x => x.LocationPoints[0] == route.LocationPoints[0]
                && x.LocationPoints[Count - 1] == route.LocationPoints[Count - 1]
                && x.Distance == route.Distance);
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {
            if (!this.routs.Values.Any(x => x.IsFavorite == true))
                return new List<Route>();

            List<Route> result = this.routs.Values
                .Where(x => x.IsFavorite == true)
                .Where(x=>x.LocationPoints.Skip(1).Contains(destinationPoint))
                .ToList();

            return result.OrderBy(x => x.Distance)
                .ThenByDescending(x => x.Popularity);
        }

        public Route GetRoute(string routeId)
        {
            if (!this.routs.ContainsKey(routeId))
                throw new ArgumentException();

            return this.routs[routeId];
        }

        public IEnumerable<Route> 
            GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            if (this.Count == 0)
                return new List<Route>();

            return this.routs.Values
                .OrderByDescending(x => x.Popularity)
                .ThenBy(x => x.Distance)
                .ThenBy(x => x.LocationPoints.Count)
                .Take(5);
        }

        public void RemoveRoute(string routeId)
        {
            if (!this.routs.ContainsKey(routeId))
                throw new ArgumentException();

            this.routs.Remove(routeId);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {
            if (this.Count == 0)
                return new List<Route>();

            List<Route> result = this.routs.Values
                .Where(x => x.LocationPoints.Contains(startPoint)
                && x.LocationPoints.Contains(endPoint)
                && x.LocationPoints.IndexOf(startPoint) < x.LocationPoints.IndexOf(endPoint))
                .ToList();

            return result
                .OrderBy(x => x.IsFavorite == true)
                .ThenBy(x => x.LocationPoints.IndexOf(endPoint)
                - x.LocationPoints.IndexOf(startPoint))
                .ThenByDescending(x => x.Popularity);
        }
    }
}
