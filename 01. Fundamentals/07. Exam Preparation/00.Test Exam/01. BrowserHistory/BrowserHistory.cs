namespace _01._BrowserHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using _01._BrowserHistory.Interfaces;

    public class BrowserHistory : IHistory
    {
        private LinkedList<ILink> links;

        public BrowserHistory()
        {
            links = new LinkedList<ILink>();
        }

        public int Size => links.Count;

        public void Clear()
        {
            links.Clear();
        }

        public bool Contains(ILink link)
        {
            return links.Contains(link);
        }

        public ILink DeleteFirst()
        {
            EnsureNotEmpty();

            ILink link = links.Last();
            links.RemoveLast();

            return link;
        }

        public ILink DeleteLast()
        {
            EnsureNotEmpty();

            ILink link = links.First();
            links.RemoveFirst();

            return link;
        }

        public ILink GetByUrl(string url)
        {
            var link = links.First;

            while (link != null)
            {
                if (link.Value.Url == url)
                {
                    return link.Value;
                }

                link = link.Next;
            }

            return null;
        }

        public ILink LastVisited()
        {
            EnsureNotEmpty();
            return links.First();
        }

        private void EnsureNotEmpty()
        {
            if (links.Count == 0)
                throw new InvalidOperationException();
        }

        public void Open(ILink link)
        {
            links.AddFirst(link);
        }

        public int RemoveLinks(string url)
        {
            url = url.ToUpper();
            int count = 0;
            var node = links.First;

            while (node != null)
            {
                var next = node.Next;
                if (node.Value.Url.ToUpper().Contains(url))
                {
                    this.links.Remove(node);
                    count++;
                }

                node = next;
            }

            return count > 0 ? count : throw new InvalidOperationException();
        }

        public ILink[] ToArray()
        {
            return links.ToArray();
        }

        public List<ILink> ToList()
        {
            return links.ToList();
        }

        public string ViewHistory()
        {
            if (links.Count == 0)
                return "Browser history is empty!";

            StringBuilder result = new StringBuilder();

            foreach (var item in links)
            {
                result.AppendLine(item.ToString());
            }

            return result.ToString();
        }
    }
}
