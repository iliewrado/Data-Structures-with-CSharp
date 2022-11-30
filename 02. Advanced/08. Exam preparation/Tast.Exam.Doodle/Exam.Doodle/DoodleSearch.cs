using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Doodle
{
    public class DoodleSearch : IDoodleSearch
    {
        private Dictionary<string, Doodle> doodlesById;
        private Dictionary<string, string> doodleTitle;

        public DoodleSearch()
        {
            this.doodlesById = new Dictionary<string, Doodle>();
            this.doodleTitle = new Dictionary<string, string>();
        }

        public int Count => this.doodlesById.Count;

        public void AddDoodle(Doodle doodle)
        {
            this.doodlesById.Add(doodle.Id, doodle);
            this.doodleTitle.Add(doodle.Title, doodle.Id);
        }

        public bool Contains(Doodle doodle)
        {
            return this.doodlesById.ContainsKey(doodle.Id);
        }

        public Doodle GetDoodle(string id)
        {
            if (!this.doodlesById.ContainsKey(id))
                throw new ArgumentException();

            return this.doodlesById[id];
        }

        public IEnumerable<Doodle> GetDoodleAds()
        {
            return this.doodlesById
                .Values.Where(x => x.IsAd == true)
                .OrderByDescending(x => x.Revenue)
                .ThenByDescending(x => x.Visits);
        }

        public IEnumerable<Doodle> GetTop3DoodlesByRevenueThenByVisits()
        {
            return this.doodlesById.Values
                .OrderByDescending(x => x.Revenue)
                .ThenByDescending(x => x.Visits)
                .Take(3);
        }

        public double GetTotalRevenueFromDoodleAds()
        {
            double result = 0;

            foreach (var item in this.doodlesById.Values
                .Where(x => x.IsAd == true))
            {
                result += item.Visits * item.Revenue;
            }

            return result;
        }

        public void RemoveDoodle(string doodleId)
        {
            if (!this.doodlesById.ContainsKey(doodleId))
                throw new ArgumentException();
            this.doodleTitle.Remove(this.doodlesById[doodleId].Title);
            this.doodlesById.Remove(doodleId);
        }

        public IEnumerable<Doodle> SearchDoodles(string searchQuery)
        {
            List<Doodle> result = this.doodlesById.Values.Where(x => x.Title.Contains(searchQuery))
                .OrderBy(x => x.Title.Length)
                .ThenByDescending(x => x.Visits)
                .ToList();

            return result.OrderByDescending(x => x.IsAd == true);
        }

        public void VisitDoodle(string title)
        {
            if (!this.doodleTitle.ContainsKey(title))
                throw new ArgumentException();

            this.doodlesById[this.doodleTitle[title]].Visits += 1;
        }
    }
}
