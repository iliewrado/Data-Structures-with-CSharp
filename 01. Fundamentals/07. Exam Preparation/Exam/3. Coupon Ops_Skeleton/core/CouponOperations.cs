using System;
using System.Collections.Generic;
using System.Linq;

namespace CouponOps
{
    public class CouponOperations : ICouponOperations
    {
        private Dictionary<string, Website> websites;
        private List<Coupon> coupons;
        public CouponOperations()
        {
            websites = new Dictionary<string, Website>();
            coupons = new List<Coupon>();
        }

        public void RegisterSite(Website w)
        {
            if (websites.ContainsKey(w.Domain))
                throw new ArgumentException();

            websites.Add(w.Domain, w);
        }

        public void AddCoupon(Website w, Coupon c)
        {
            CheckWebsiteExist(w.Domain);

            websites[w.Domain].Coupons.Add(c);
            coupons.Add(c);
        }

        private void CheckWebsiteExist(string domain)
        {
            if (!websites.ContainsKey(domain))
                throw new ArgumentException();
        }

        public Website RemoveWebsite(string domain)
        {
            CheckWebsiteExist(domain);

            Website website = websites[domain];

            foreach (var item in website.Coupons)
            {
                coupons.Remove(item);
            }

            websites.Remove(domain);
            return website;
        }

        public Coupon RemoveCoupon(string code)
        {
            if (!coupons.Exists(x => x.Code == code))
                throw new ArgumentException();

            Coupon coupon = coupons.FirstOrDefault(x => x.Code == code);
            coupons.Remove(coupon);
            // remove coupon from website

            return coupon;
        }

        public bool Exist(Website w)
        {
            return websites.ContainsKey(w.Domain);
        }

        public bool Exist(Coupon c)
        {
            return coupons.Contains(c);
        }

        public IEnumerable<Website> GetSites()
        {
            return websites.Values;
        }

        public IEnumerable<Coupon> GetCouponsForWebsite(Website w)
        {
            CheckWebsiteExist(w.Domain);

            return websites[w.Domain].Coupons;
        }

        public void UseCoupon(Website w, Coupon c)
        {
            CheckWebsiteExist(w.Domain);
            if (!Exist(c))
                throw new ArgumentException();

            if (!websites[w.Domain].Coupons.Exists(x => x.Code == c.Code))
                throw new ArgumentException();

            websites[w.Domain].Coupons.Remove(c);
        }

        public IEnumerable<Coupon> GetCouponsOrderedByValidityDescAndDiscountPercentageDesc()
        {
            return coupons
                .OrderByDescending(x => x.Validity)
                .ThenByDescending(x => x.DiscountPercentage);
        }

        public IEnumerable<Website> GetWebsitesOrderedByUserCountAndCouponsCountDesc()
        {
            return websites.Values
                .OrderByDescending(x => x.UsersCount)
                .ThenByDescending(x => x.Coupons.Count);
        }
    }
}
