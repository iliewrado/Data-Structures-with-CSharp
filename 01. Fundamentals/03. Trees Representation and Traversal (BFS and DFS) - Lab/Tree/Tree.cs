namespace Tree
{
    using System;
    using System.Collections.Generic;

    public class Tree<T> : IAbstractTree<T>
    {
        private readonly List<Tree<T>> _children;
        private bool IsRootDeleted = false;

        public Tree(T value)
        {
            this.Value = value;
            this.Parent = null;
            this._children = new List<Tree<T>>();
        }

        public Tree(T key, params Tree<T>[] children)
            : this(key)
        {
            foreach (Tree<T> child in children)
            {
                child.Parent = this;
                this._children.Add(child);
            }
        }

        public T Value { get; private set; }
        public Tree<T> Parent { get; private set; }
        public IReadOnlyCollection<Tree<T>> Children
            => this._children.AsReadOnly();

        public ICollection<T> OrderBfs()
        {
            List<T> result = new List<T>();
            if (IsRootDeleted)
            {
                return result;
            }

            Queue<Tree<T>> trees = new Queue<Tree<T>>();

            trees.Enqueue(this);

            while (trees.Count > 0)
            {
                Tree<T> subtree = trees.Dequeue();

                result.Add(subtree.Value);

                foreach (Tree<T> child in subtree._children)
                {
                    trees.Enqueue(child);
                }
            }
                        
            return result;
        }

        public ICollection<T> OrderDfs()
        {
            List<T> result = new List<T>();
            
            if (IsRootDeleted)
            {
                return result;
            }

            this.Dfs(this, result);

            return result;
        }

        public void AddChild(T parentKey, Tree<T> child)
        {
            Tree<T> searched = this.FindBfs(parentKey);

            this.CheckEmptyNode(searched);

            searched._children.Add(child);
        }

        public void RemoveNode(T nodeKey)
        {
            Tree<T> searched = this.FindBfs(nodeKey);
            this.CheckEmptyNode(searched);

            foreach (var child in searched._children)
            {
                child.Parent = null;
            }

            searched._children.Clear();

            Tree<T> searchedParent = searched.Parent;

            if (searchedParent == null)
            {
                this.IsRootDeleted = true;
            }
            else
            {
                searchedParent._children.Remove(searched);
            }

            searched.Value = default;
        }

        public void Swap(T firstKey, T secondKey)
        {
            Tree<T> firstTree = this.FindBfs(firstKey);
            this.CheckEmptyNode(firstTree);
            Tree<T> secondTree = this.FindBfs(secondKey);
            this.CheckEmptyNode(secondTree);

            var firstParent = firstTree.Parent;
            var secondParent = secondTree.Parent;

            if (firstTree.Parent == null)
            {
                this.Value = secondTree.Value;
                this._children.Clear();
                this._children.AddRange(secondTree._children);
            }
            else if (secondTree.Parent == null)
            {
                this.Value = firstTree.Value;
                this._children.Clear();
                this._children.AddRange(firstTree._children);
            }
            else
            {
                firstTree.Parent = secondParent;
                secondTree.Parent = firstParent;

                int indexOfFirst = firstParent._children.IndexOf(firstTree);
                int indexOfSecond = secondParent._children.IndexOf(secondTree);

                firstParent._children[indexOfFirst] = secondTree;
                secondParent._children[indexOfSecond] = firstTree;
            }

        }
        
        private void Dfs(Tree<T> tree, List<T> result)
        {
            foreach (var child in tree._children)
            {
                this.Dfs(child, result);
            }

            result.Add(tree.Value);
        }

        private Tree<T> FindBfs(T key)
        {
            Queue<Tree<T>> temp = new Queue<Tree<T>>();
            temp.Enqueue(this);

            while (temp.Count > 0)
            {
                Tree<T> node = temp.Dequeue();
                if (node.Value.Equals(key))
                {
                    return node;
                }

                foreach (var child in node._children)
                {
                    temp.Enqueue(child);
                }
            }

            return null;
        }

        private void CheckEmptyNode(Tree<T> searched)
        {
            if (searched == null)
                throw new ArgumentNullException();
        }
    }
}
