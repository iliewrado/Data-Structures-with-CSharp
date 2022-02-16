namespace _05.TopView
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
        where T : IComparable<T>
    {
        public BinaryTree(T value, BinaryTree<T> left, BinaryTree<T> right)
        {
            this.Value = value;
            this.LeftChild = left;
            this.RightChild = right;
        }

        public T Value { get; set; }

        public BinaryTree<T> LeftChild { get; set; }

        public BinaryTree<T> RightChild { get; set; }

        public List<T> TopView()
        {
            Dictionary<int, (T nodeValue, int level)> map =
                new Dictionary<int, (T nodeValue, int level)>();

            this.TopView(this, map, 0, 0);

            return map.Values.Select(x => x.nodeValue).ToList();
        }

        private void TopView(BinaryTree<T> node, Dictionary<int,
            (T nodeValue, int nodeLevel)> map, int distance, int level)
        {
            if (node == null) return;

            if (map.ContainsKey(distance))
            {
                if (map[distance].nodeLevel > level)
                {
                    map[distance] = (node.Value, level);
                }
            }
            else
            {
                map.Add(distance, (node.Value, level));
            }

            this.TopView(node.LeftChild, map, distance - 1, level + 1);
            this.TopView(node.RightChild, map, distance + 1, level + 1);
        }
    }
}
