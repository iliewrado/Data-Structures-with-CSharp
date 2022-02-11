namespace _04.BinarySearchTree
{
    using System;

    public class BinarySearchTree<T> : IAbstractBinarySearchTree<T>
        where T : IComparable<T>
    {
        public BinarySearchTree()
        {
            Root = null;
        }

        public BinarySearchTree(Node<T> root)
        {
            this.Root = root;
            LeftChild = Root.LeftChild;
            RightChild = Root.RightChild;
        }

        public Node<T> Root { get; private set; }

        public Node<T> LeftChild { get; private set; }

        public Node<T> RightChild { get; private set; }

        public T Value => this.Root.Value;

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
            return Search(element, this.Root);
        }

        private void Insert(T element, Node<T> node)
        {
            if (node == null)
            {
                node = new Node<T>(element, null , null);
                Root = node;
                return;
            }

            if (node.Value.CompareTo(element) > 0)
            {
                if (node.LeftChild == null)
                {
                    node.LeftChild = new Node<T>(element, null, null);
                    return;
                }
                Insert(element, node.LeftChild);
            }
            else
            {
                if (node.RightChild == null)
                {
                    node.RightChild = new Node<T>(element, null, null);
                    return;
                }
                Insert(element, node.RightChild);
            }
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

        private IAbstractBinarySearchTree<T> Search(T element, Node<T> node)
        {
            if (node == null) return null;

            if (node.Value.CompareTo(element) == 0)
                return new BinarySearchTree<T>(node);
            
            if (node.Value.CompareTo(element) > 0)
            {
                return Search(element, node.LeftChild);
            }
            else
            {
                return Search(element, node.RightChild);
            }
        }
    }
}
