namespace _02.DOM
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using _02.DOM.Interfaces;
    using _02.DOM.Models;

    public class DocumentObjectModel : IDocument
    {
        public DocumentObjectModel(IHtmlElement root)
        {
            this.Root = root;
        }

        public DocumentObjectModel()
        {
            this.Root = new HtmlElement(ElementType.Document,
                new HtmlElement(ElementType.Html,
                new HtmlElement(ElementType.Head),
                new HtmlElement(ElementType.Body)));

        }

        public IHtmlElement Root { get; private set; }

        public IHtmlElement GetElementByType(ElementType type)
        {
            Queue<IHtmlElement> htmls = new Queue<IHtmlElement>();

            htmls.Enqueue(this.Root);

            while (htmls.Count > 0)
            {
                IHtmlElement current = htmls.Dequeue();

                if (current.Type == type)
                {
                    return current;
                }

                foreach (var child in current.Children)
                {
                    htmls.Enqueue(child);
                }
            }

            return null;
        }

        public List<IHtmlElement> GetElementsByType(ElementType type)
        {
            List<IHtmlElement> result = new List<IHtmlElement>();

            this.DfsGetByType(this.Root, type, result);

            return result;
        }

        private void DfsGetByType(IHtmlElement root, ElementType type, List<IHtmlElement> result)
        {
            foreach (var child in root.Children)
            {
                this.DfsGetByType(child, type, result);
            }

            if (root.Type == type)
            {
                result.Add(root);
            }
        }

        public bool Contains(IHtmlElement htmlElement)
        {
            return this.FindElement(htmlElement) != null;
        }

        public void InsertFirst(IHtmlElement parent, IHtmlElement child)
        {
            this.ElementExist(parent);
            parent.Children.Insert(0, child);
            child.Parent = parent;
        }

        private void ElementExist(IHtmlElement element)
        {
            if (!this.Contains(element))
                throw new InvalidOperationException();
        }

        private IHtmlElement FindElement(IHtmlElement element)
        {
            Queue<IHtmlElement> htmls = new Queue<IHtmlElement>();

            htmls.Enqueue(this.Root);

            while (htmls.Count > 0)
            {
                IHtmlElement current = htmls.Dequeue();

                if (current == element)
                {
                    return current;
                }

                foreach (var child in current.Children)
                {
                    htmls.Enqueue(child);
                }
            }

            return null;
        }

        public void InsertLast(IHtmlElement parent, IHtmlElement child)
        {
            this.ElementExist(parent);
            parent.Children.Add(child);
            child.Parent = parent;
        }

        public void Remove(IHtmlElement htmlElement)
        {
            this.ElementExist(htmlElement);

            htmlElement.Parent.Children.Remove(htmlElement);
            htmlElement.Parent = null;
            htmlElement.Children.Clear();
        }

        public void RemoveAll(ElementType elementType)
        {

            Queue<IHtmlElement> htmls = new Queue<IHtmlElement>();

            htmls.Enqueue(this.Root);

            while (htmls.Count > 0)
            {
                IHtmlElement htmlElement = htmls.Dequeue();

                if (htmlElement.Type == elementType)
                {
                    htmlElement.Parent.Children.Remove(htmlElement);
                    htmlElement.Parent = null;
                    htmlElement.Children.Clear();
                }

                foreach (var child in htmlElement.Children)
                {
                    htmls.Enqueue(child);
                }
            }
        }

        public bool AddAttribute(string attrKey, string attrValue, IHtmlElement htmlElement)
        {
            this.ElementExist(htmlElement);

            if (htmlElement.Attributes.ContainsKey(attrKey))
                return false;

            htmlElement.Attributes.Add(attrKey, attrValue);
            return true;
        }

        public bool RemoveAttribute(string attrKey, IHtmlElement htmlElement)
        {
            this.ElementExist(htmlElement);

            if (!htmlElement.Attributes.ContainsKey(attrKey))
                return false;

            htmlElement.Attributes.Remove(attrKey);
            return true;
        }

        public IHtmlElement GetElementById(string idValue)
        {
            Queue<IHtmlElement> htmls = new Queue<IHtmlElement>();

            htmls.Enqueue(this.Root);

            while (htmls.Count > 0)
            {
                IHtmlElement current = htmls.Dequeue();

                if (current.Attributes.ContainsKey("id"))
                {
                    if( current.Attributes["id"] == idValue)
                    {
                        return current;
                    }
                }

                foreach (var child in current.Children)
                {
                    htmls.Enqueue(child);
                }
            }

            return null;
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();

            this.DfsToString(this.Root, 0, toString);

            return toString.ToString();
        }

        private void DfsToString(IHtmlElement element, int spaces, StringBuilder toString)
        {
            toString.Append(' ', spaces).AppendLine(element.Type.ToString());

            foreach (var child in element.Children)
            {
                this.DfsToString(child, spaces + 2, toString);
            }
        }
    }
}
