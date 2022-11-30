namespace _02.VaniPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Agency : IAgency
    {
        private Dictionary<string, Invoice> archive;

        public Agency()
        {
            this.archive = new Dictionary<string, Invoice>();
        }

        public void Create(Invoice invoice)
        {
            if (this.archive.ContainsKey(invoice.SerialNumber))
                throw new ArgumentException();

            this.archive.Add(invoice.SerialNumber, invoice);
        }

        public void ThrowInvoice(string number)
        {
            if (!this.archive.ContainsKey(number))
                throw new ArgumentException();

            this.archive.Remove(number);
        }

        public void ThrowPayed()
        {
            List<Invoice> toRemove = this.archive.Values
                .Where(x => x.Subtotal == 0)
                .ToList();

            foreach (var item in toRemove)
            {
                this.archive.Remove(item.SerialNumber);
            }
        }

        public int Count()
            => this.archive.Count;

        public bool Contains(string number)
        {
            return this.archive.ContainsKey(number);
        }

        public void PayInvoice(DateTime due)
        {
            List<Invoice> toPay = this.archive.Values
                .Where(x => x.DueDate == due)
                .ToList();

            if (toPay.Count == 0)
                throw new ArgumentException();

            foreach (var item in toPay)
            {
                item.Subtotal = 0;
            }
        }

        public IEnumerable<Invoice> GetAllInvoiceInPeriod(DateTime start, DateTime end)
        {
            return this.archive.Values
                .Where(x => x.IssueDate >= (start) && x.IssueDate <= (end))
                .OrderBy(x => x.IssueDate)
                .ThenBy(x => x.DueDate);
        }

        public IEnumerable<Invoice> SearchBySerialNumber(string serialNumber)
        {
            List<Invoice> result =  this.archive.Values
                .Where(x => x.SerialNumber.Contains(serialNumber))
                .OrderByDescending(x => x.SerialNumber)
                .ToList();

            if (result.Count == 0)
                throw new ArgumentException();

            return result;
        }

        public IEnumerable<Invoice> ThrowInvoiceInPeriod(DateTime start, DateTime end)
        {
            List<Invoice> toRemove = this.archive.Values
                .Where(x => x.DueDate > start && x.DueDate < end)
                .ToList();

            if (toRemove.Count == 0)
                throw new ArgumentException();

            foreach (var item in toRemove)
            {
                this.archive.Remove(item.SerialNumber);
            }

            return toRemove;
        }

        public IEnumerable<Invoice> GetAllFromDepartment(Department department)
        {
            return this.archive.Values
                .Where(x => x.Department == department)
                .OrderByDescending(x => x.Subtotal)
                .ThenBy(x => x.IssueDate);
        }

        public IEnumerable<Invoice> GetAllByCompany(string company)
        {
            return this.archive.Values
                .Where(x => x.CompanyName == company)
                .OrderByDescending(x => x.SerialNumber);
        }

        public void ExtendDeadline(DateTime dueDate, int days)
        {
            List<Invoice> invoices = this.archive.Values
                .Where(x => x.DueDate == dueDate)
                .ToList();

            if (invoices.Count == 0)
                throw new ArgumentException();

            foreach (var inv in invoices)
            {
                this.archive[inv.SerialNumber]
                    .DueDate = this.archive[inv.SerialNumber]
                    .DueDate.AddDays(days);
            }
        }
    }
}
