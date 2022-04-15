namespace _01.RedBlackTree
{
    using System;

    //Left-leaning red–black tree
    public class RedBlackTree<T> where T : IComparable
    {
        private const bool Red = true;
        private const bool Black = false;
        public class Node
        {
            public Node(T value)
            {
                this.Value = value;
                this.Color = Red;
            }

            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public bool Color { get; set; }
        }

        public Node root;

        public RedBlackTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        public RedBlackTree()
        {

        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.root, action);
        }

        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;
            
            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }

        public RedBlackTree<T> Search(T element)
        {
            Node current = this.FindNode(element);

            return new RedBlackTree<T>(current);
        }

        private Node FindNode(T element)
        {
            Node current = this.root;

            while (current != null)
            {
                if (AreEqual(element, current.Value))
                {
                    break;
                }
                else if (IsLesser(element, current.Value))
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return current;
        }

        public void Insert(T value)
        {
            this.root = this.Insert(this.root, value);
            this.root.Color = Black;
        }

        private Node Insert(Node node, T value)
        {
            if (node == null)
                return new Node(value);

            if (IsLesser(value, node.Value))
            {
                node.Left = this.Insert(node.Left, value);
            }
            else
            {
                node.Right = this.Insert(node.Right, value);
            }

            if (IsRed(node.Right))
                node = this.RotateLeft(node);

            if (IsRed(node.Left) && IsRed(node.Left.Left))
               node = this.RotateRight(node);
                        
            if (IsRed(node.Left) && IsRed(node.Right))
                this.FlipColors(node);

            return node;
        }

        public void Delete(T key)
        {
            if (this.root == null)
                throw new InvalidOperationException();

            this.root = this.Delete(this.root, key);

            if (this.root != null)
                this.root.Color = Black;
        }

        private Node Delete(Node node, T element)
        {
            if (IsLesser(element, node.Value))
            {
                if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                    node = this.MoveRedLeft(node);

                node.Left = this.Delete(node.Left, element);
            }
            else
            {
                if (IsRed(node.Left))
                    node = RotateRight(node);

                if (AreEqual(element, node.Value) && node.Right == null)
                    return null;

                if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                    node = MoveRedRight(node);

                if (AreEqual(element, node.Value))
                {
                    node.Value = FindMinValueInSubTree(node.Right);
                    node.Right = this.DeleteMin(node.Right);
                }
                else
                {
                    node.Right = this.Delete(node.Right, element);
                }
            }

            return this.FixUp(node);
        }

        public void DeleteMin()
        {
            if (this.root == null)
                throw new InvalidOperationException();

            this.root = this.DeleteMin(this.root);

            if (this.root != null)
                this.root.Color = Black;
        }

        private Node DeleteMin(Node node)
        {
            if (node.Left == null)
                return null;

            if (!IsRed(node.Left) && !IsRed(node.Left.Left))
            {
                node = this.MoveRedLeft(node);
            }

            node.Left = this.DeleteMin(node.Left);

            return this.FixUp(node);
        }

        public void DeleteMax()
        {
            if (this.root == null)
                throw new InvalidOperationException();

            this.root = this.DeleteMax(this.root);

            if (this.root != null)
                this.root.Color = Black;
        }

        private Node DeleteMax(Node node)
        {
            if (IsRed(node.Left))
                node = RotateRight(node);

            if (node.Right == null)
                return null;

            if (!IsRed(node.Right) && !IsRed(node.Right.Left))
            {
                node = MoveRedRight(node);
            }

            node.Right = DeleteMax(node.Right);

            return FixUp(node);
        }

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

        //Rotations
        private Node RotateLeft(Node node)
        {
            Node temp = node.Right;
            node.Right = temp.Left;
            temp.Left = node;
            temp.Color = temp.Left.Color;
            temp.Left.Color = Red;

            return temp;
        }

        private Node RotateRight(Node node)
        {
            Node temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;
            temp.Color = temp.Right.Color;
            temp.Right.Color = Red;

            return temp;
        }

        private void FlipColors(Node node)
        {
            node.Color = !node.Color;
            node.Left.Color = !node.Left.Color;
            node.Right.Color = !node.Right.Color;
        }

        //Help Methods
        private bool IsRed(Node node)
        {
            return node?.Color == Red;
        }

        private bool IsLesser(T first, T second)
        {
            return first.CompareTo(second) < 0;
        }

        private bool AreEqual(T first, T second)
        {
            return first.CompareTo(second) == 0;
        }

        private void PreOrderCopy(Node node)
        {
            if (node == null)
                return;

            this.Insert(node.Value);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }

        private Node MoveRedLeft(Node node)
        {
            this.FlipColors(node);

            if (IsRed(node.Right.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                this.FlipColors(node);
            }

            return node;
        }

        private Node FixUp(Node node)
        {
            if (IsRed(node.Right))
            {
                node = this.RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = this.RotateRight(node);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                this.FlipColors(node);
            }

            return node;
        }

        private Node MoveRedRight(Node node)
        {
            this.FlipColors(node);

            if (IsRed(node.Left.Left))
            {
                node = this.RotateRight(node);
                this.FlipColors(node);
            }

            return node;
        }

        private T FindMinValueInSubTree(Node node)
        {
            if (node.Left == null)
                return node.Value;

            return this.FindMinValueInSubTree(node.Left);
        }
    }
}