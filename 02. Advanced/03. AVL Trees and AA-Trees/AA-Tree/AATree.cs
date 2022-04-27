namespace AA_Tree
{
    using System;

    public class AATree<T> : IBinarySearchTree<T>
        where T : IComparable<T>
    {
        private class Node
        {
            public Node(T element)
            {
                this.Value = element;
                this.Level = 1;
            }

            public T Value { get; set; }
            public Node Right { get; set; }
            public Node Left { get; set; }
            public int Level { get; set; }
        }

        private Node root;

        public int Count()
        {
            return this.Count(this.root);
        }

        private int Count(Node node)
        {
            if (node == null)
                return 0;

            return 1 + this.Count(node.Left) + this.Count(node.Right);
        }

        public void Insert(T element)
        {
            this.root = this.Insert(this.root, element);
        }

        private Node Insert(Node node, T element)
        {
            if (node == null)
                return new Node(element);

            if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.Insert(node.Left, element);
            }
            else
            {
                node.Right = this.Insert(node.Right, element);
            }

            node = this.Skew(node);
            node = this.Split(node);

            return node;
        }

        private Node Split(Node node)
        {
            if (node.Right == null || node.Right.Right == null)
            {
                return node;
            }
            else if (node.Right.Right.Level == node.Level)
            {
                node = this.RotateRight(node);
                node.Level++;
            }

            return node;
        }

        private Node RotateRight(Node node)
        {
            Node current = node.Right;
            node.Right = current.Left;
            current.Left = node;

            return current;
        }

        private Node Skew(Node node)
        {
            if (node.Left != null && node.Level == node.Left.Level)
            {
                node = this.RotateLeft(node);
            }

            return node;
        }

        private Node RotateLeft(Node node)
        {
            Node current = node.Left;
            node.Left = current.Right;
            current.Right = node;

            return current;
        }

        public bool Contains(T element)
        {
            Node current = this.root;

            while (current != null)
            {
                if (current.Value.CompareTo(element) == 0)
                {
                    return true;
                }
                else if (current.Value.CompareTo(element) > 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return false;
        }

        public void InOrder(Action<T> action)
        {
            this.InOrder(this.root, action);
        }

        private void InOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;

            this.InOrder(node.Left, action);
            action(node.Value);
            this.InOrder(node.Right, action);
        }

        public void PreOrder(Action<T> action)
        {
            this.PreOrder(this.root, action);
        }

        private void PreOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;

            action(node.Value);
            this.PreOrder(node.Left, action);
            this.PreOrder(node.Right, action);
        }

        public void PostOrder(Action<T> action)
        {
            this.PostOrder(this.root, action);
        }

        private void PostOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;

            this.PostOrder(node.Left, action);
            this.PostOrder(node.Right, action);
            action(node.Value);
        }
    }
}