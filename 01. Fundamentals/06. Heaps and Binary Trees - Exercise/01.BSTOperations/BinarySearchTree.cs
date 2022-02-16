namespace _01.BSTOperations
{
    using System;
    using System.Collections.Generic;

    public class BinarySearchTree<T> : IAbstractBinarySearchTree<T>
        where T : IComparable<T>
    {
        public BinarySearchTree()
        {
            this.Root = null;
        }

        public BinarySearchTree(Node<T> root)
        {
            Root = root;
            LeftChild = Root.LeftChild;
            RightChild = Root.RightChild;
            Count++;
        }

        public Node<T> Root { get; private set; }

        public Node<T> LeftChild { get; private set; }

        public Node<T> RightChild { get; private set; }

        public T Value => this.Root.Value;

        public int Count { get; private set; }

        public bool Contains(T element)
        {
            return Contains(element, this.Root);
        }

        public void Insert(T element)
        {
            Insert(element, this.Root);
        }

        public IAbstractBinarySearchTree<T> Search(T element)
        {
            Node<T> node = this.Root;

            while (node != null)
            {
                if (node.Value.CompareTo(element) > 0)
                {
                    node = node.LeftChild;
                }
                else if (node.Value.CompareTo(element) < 0)
                {
                    node = node.RightChild;
                }
                else
                {
                    return new BinarySearchTree<T>(node);
                }
            }

            return null;
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(action, this.Root);
        }

        public List<T> Range(T lower, T upper)
        {
            List<T> result = new List<T>();

            this.Range(lower, upper, this.Root, result);

            return result;
        }

        public void DeleteMin()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException();
            }

            Node<T> temp = this.Root;

            while (temp.LeftChild.LeftChild != null)
            {
                temp = temp.LeftChild;
            }

            if (temp.LeftChild.RightChild != null)
            {
                temp.LeftChild = temp.LeftChild.RightChild;
            }
            else
            {
                temp.LeftChild = null;
            }
            Count--;
        }

        public void DeleteMax()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException();
            }

            Node<T> temp = this.Root;

            while (temp.RightChild.RightChild != null)
            {
                temp = temp.RightChild;
            }

            if (temp.RightChild.LeftChild != null)
            {
                temp.RightChild = temp.RightChild.LeftChild;
            }
            else
            {
                temp.RightChild = null;
            }
            Count--;
        }

        public int GetRank(T element)
        {
            return GetRank(element, Root);
        }

        private void Range(T lower, T upper, Node<T> node, List<T> result)
        {
            if (node == null) return;

            int min = lower.CompareTo(node.Value);
            int max = upper.CompareTo(node.Value);

            if (min < 0)
            {
                this.Range(lower, upper, node.LeftChild, result);
            }

            if (min <= 0 && max >= 0)
            {
                result.Add(node.Value);
            }

            if (max > 0)
            {
                this.Range(lower, upper, node.RightChild, result);
            }
        }


        private int GetRank(T element, Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            int compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                return this.GetRank(element, node.LeftChild);
            }

            if (compare > 0)
            {
                return 1 + this.GetRank(element, node.LeftChild)
                    + this.GetRank(element, node.RightChild);
            }

            return 1;
        }

        private void Insert(T element, Node<T> node)
        {
            if (node == null)
            {
                node = new Node<T>(element, null, null);
                Root = node;
                Count++;
                return;
            }

            if (node.Value.CompareTo(element) > 0)
            {
                if (node.LeftChild == null)
                {
                    node.LeftChild = new Node<T>(element, null, null);
                    Count++;
                    return;
                }
                Insert(element, node.LeftChild);
            }
            else
            {
                if (node.RightChild == null)
                {
                    node.RightChild = new Node<T>(element, null, null);
                    Count++;
                    return;
                }
                Insert(element, node.RightChild);
            }
        }

        private void EachInOrder(Action<T> action, Node<T> node)
        {
            if (node == null) return;

            this.EachInOrder(action, node.LeftChild);
            action(node.Value);
            this.EachInOrder(action, node.RightChild);
        }

        private bool Contains(T element, Node<T> node)
        {
            if (node == null) return false;

            if (node.Value.CompareTo(element) == 0)
                return true;

            if (node.Value.CompareTo(element) > 0)
            {
                return Contains(element, node.LeftChild);
            }
            else
            {
                return Contains(element, node.RightChild);
            }
        }        
    }
}
