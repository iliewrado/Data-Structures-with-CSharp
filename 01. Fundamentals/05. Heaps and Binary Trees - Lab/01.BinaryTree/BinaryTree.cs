namespace _01.BinaryTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
    {
        public BinaryTree(T value
            , IAbstractBinaryTree<T> leftChild
            , IAbstractBinaryTree<T> rightChild)
        {
            this.Value = value;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public T Value { get; private set; }

        public IAbstractBinaryTree<T> LeftChild { get; private set; }

        public IAbstractBinaryTree<T> RightChild { get; private set; }

        public string AsIndentedPreOrder(int indent)
        {
            return PreorderDfs(indent, this);
        }

        public List<IAbstractBinaryTree<T>> InOrder()
        {
            List<IAbstractBinaryTree<T>> result = new List<IAbstractBinaryTree<T>>();
            DfsInOrder(this, result);
            return result;
        }

        public List<IAbstractBinaryTree<T>> PostOrder()
        {
            List<IAbstractBinaryTree<T>> result = new List<IAbstractBinaryTree<T>>();
            DfsPostOrder(this, result);
            return result;
        }

        public List<IAbstractBinaryTree<T>> PreOrder()
        {
            List<IAbstractBinaryTree<T>> result = new List<IAbstractBinaryTree<T>>();
            DfsPreOrder(this, result);
            return result;
        }

        public void ForEachInOrder(Action<T> action)
        {
            if (this.LeftChild != null)
            {
                this.LeftChild.ForEachInOrder(action);
            }

            action.Invoke(this.Value);

            if (this.RightChild != null)
            {
                this.RightChild.ForEachInOrder(action);
            }
        }

        private void DfsPreOrder(IAbstractBinaryTree<T> tree, List<IAbstractBinaryTree<T>> result)
        {
            result.Add(tree);
            
            if (tree.LeftChild != null)
            {
                DfsPreOrder(tree.LeftChild, result);
            }

            if (tree.RightChild != null)
            {
                DfsPreOrder(tree.RightChild, result);
            }
        }

        private void DfsPostOrder(IAbstractBinaryTree<T> tree, List<IAbstractBinaryTree<T>> result)
        {
            if (tree.LeftChild != null)
            {
                DfsPostOrder(tree.LeftChild, result);
            }

            if (tree.RightChild != null)
            {
                DfsPostOrder(tree.RightChild, result);
            }
            
            result.Add(tree);
        }

        private void DfsInOrder(IAbstractBinaryTree<T> tree, List<IAbstractBinaryTree<T>> result)
        {
            if (tree.LeftChild != null)
            {
                DfsInOrder(tree.LeftChild, result);
            }
            
            result.Add(tree);

            if (tree.RightChild != null)
            {
                DfsInOrder(tree.RightChild, result);
            }
        }

        private string PreorderDfs(int spaceLevel, IAbstractBinaryTree<T> node)
        {
            StringBuilder result = new StringBuilder();
            result.Append(new string(' ', spaceLevel));
            result.AppendLine(node.Value.ToString());

            if (node.LeftChild != null)
            {
                result.AppendLine(PreorderDfs(spaceLevel + 2, node.LeftChild));
            }
            if (node.RightChild != null)
            {
                result.AppendLine(PreorderDfs(spaceLevel + 2, node.RightChild));
            }

            return result.ToString().TrimEnd();
        }
    }
}
