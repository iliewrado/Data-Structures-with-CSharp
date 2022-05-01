namespace Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    public class Hierarchy<T> : IHierarchy<T>
    {
        private Node<T> root;
        private Dictionary<T, Node<T>> nodesByValue;

        public Hierarchy(T value)
        {
            this.root = new Node<T>(value);
            this.nodesByValue = new Dictionary<T, Node<T>>();
            this.nodesByValue.Add(value, this.root);
        }

        public int Count => this.nodesByValue.Count;

        public void Add(T element, T child)
        {
            if (!this.nodesByValue.ContainsKey(element) ||
                this.nodesByValue.ContainsKey(child))
                throw new ArgumentException();

            Node<T> node = new Node<T>(child)
            {
                Parent = this.nodesByValue[element]
            };

            this.nodesByValue[element].Children.Add(node);
            this.nodesByValue.Add(child, node);
        }

        public bool Contains(T element)
        {
            return this.nodesByValue.ContainsKey(element);
        }

        public IEnumerable<T> GetChildren(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
                throw new ArgumentException();

            return this.nodesByValue[element]
                .Children
                .Select(x => x.Value);
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            return this.nodesByValue.Keys
                .Intersect(other.nodesByValue.Keys);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Queue<Node<T>> nodes = new Queue<Node<T>>();
            nodes.Enqueue(this.root);

            while (nodes.Count != 0)
            {
                Node<T> current = nodes.Dequeue();

                yield return current.Value;

                foreach (var child in current.Children)
                {
                    nodes.Enqueue(child);
                }
            }
        }

        public T GetParent(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
                throw new ArgumentException();

            if (this.nodesByValue[element].Parent == null)
                return default;

            return this.nodesByValue[element].Parent.Value;
        }

        public void Remove(T element)
        {
            if (element.Equals(this.root.Value))
                throw new InvalidOperationException();

            if (!this.nodesByValue.ContainsKey(element))
                throw new ArgumentException();

            Node<T> node = this.nodesByValue[element];
            Node<T> parentNode = node.Parent;

            parentNode.Children.AddRange(node.Children);
            parentNode.Children.Remove(node);

            foreach (var child in node.Children)
            {
                this.nodesByValue[child.Value].Parent = parentNode;
            }

            this.nodesByValue.Remove(element);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}