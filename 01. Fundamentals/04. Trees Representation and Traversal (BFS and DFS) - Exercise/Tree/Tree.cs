namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree<T> : IAbstractTree<T>
    {
        private readonly List<Tree<T>> _children;

        public Tree(T key, params Tree<T>[] children)
        {
            this.Key = key;
            _children = new List<Tree<T>>();

            foreach (var child in children)
            {
                this.AddChild(child);
                child.AddParent(this);
            }

        }

        public T Key { get; private set; }

        public Tree<T> Parent { get; private set; }


        public IReadOnlyCollection<Tree<T>> Children
            => this._children.AsReadOnly();

        public void AddChild(Tree<T> child)
        {
            this._children.Add(child);
        }

        public void AddParent(Tree<T> parent)
        {
            this.Parent = parent;
        }

        public string GetAsString()
        {
            return this.GetAsString(0, this);
        }

        public Tree<T> GetDeepestLeftomostNode()
        {
            return DeepestNode();
        }

        
        public List<T> GetLeafKeys()
        {
            List<Tree<T>> result = this.GetLeafNodes();          

            return result.Select(x=> x.Key)
                .OrderBy(x=>x)
                .ToList();
        }

        public List<T> GetMiddleKeys()
        {
            List<T> result = new List<T>();

            foreach (var child in _children)
            {
                if (child.Parent != null && child._children.Count > 0)
                {
                    result.Add(child.Key);
                }
            }

            return result.OrderBy(x => x).ToList();
        }

        public List<T> GetLongestPath()
        {
            List<T> result = new List<T>();
            Tree<T> node = DeepestNode();
            result.Add(node.Key);

            while (node.Parent != null)
            {
                node = node.Parent;
                result.Add(node.Key);
            }

            result.Reverse();
            return result;
        }

        public List<List<T>> PathsWithGivenSum(int sum)
        {
            List<List<T>> result = new List<List<T>>();
            List<Tree<T>> leafNodes = GetLeafNodes();

            foreach (var leaf in leafNodes)
            {
                Tree<T> node = leaf;
                int currentSum = 0;
                List<T> currentNodes = new List<T>();

                while (node != null)
                {
                    currentNodes.Add(node.Key);
                    currentSum += Convert.ToInt32(node.Key);
                    node = node.Parent;
                }

                if (currentSum == sum)
                {
                    currentNodes.Reverse();
                    result.Add(currentNodes);
                }
            }

            return result;
        }

        private Tree<T> DeepestNode()
        {
            Stack<Tree<T>> result = new Stack<Tree<T>>();
            
            Queue<Tree<T>> trees = new Queue<Tree<T>>();

            trees.Enqueue(this);

            while (trees.Count > 0)
            {
                Tree<T> subtree = trees.Dequeue();

                result.Push(subtree);
                subtree._children.Reverse();
                foreach (Tree<T> child in subtree._children)
                {
                    trees.Enqueue(child);
                }
            }

            return result.Pop();
        }

        private List<Tree<T>> GetLeafNodes()
        {
            List<Tree<T>> result = new List<Tree<T>>();
            Queue<Tree<T>> temp = new Queue<Tree<T>>();
            temp.Enqueue(this);

            while (temp.Count > 0)
            {
                Tree<T> node = temp.Dequeue();

                if (node._children.Count == 0)
                {
                    result.Add(node);
                }

                foreach (var child in node._children)
                {
                    temp.Enqueue(child);
                }
            }

            return result;
        }

        public List<Tree<T>> SubTreesWithGivenSum(int sum)
        {
            List<Tree<T>> roots = new List<Tree<T>>();

            SubTreeDfsSum(this, sum, roots);

            return roots;
        }

        private int SubTreeDfsSum(Tree<T> node, int targetSum, List<Tree<T>> roots)
        {
            int currentSum = Convert.ToInt32(node.Key);

            foreach (var child in node._children)
            {
                currentSum += SubTreeDfsSum(child, targetSum, roots);
            }

            if (currentSum == targetSum)
            {
                roots.Add(node);
            }

            return currentSum;
        }

        private string GetAsString(int spaceLevel, Tree<T> node)
        {
            StringBuilder result = new StringBuilder();
            result.Append(new string(' ', spaceLevel));
            result.AppendLine(node.Key.ToString());

            foreach (var child in node._children)
            {
                result.AppendLine(GetAsString(spaceLevel + 2, child));
            }

            return result.ToString().TrimEnd();
        }

        private void Dfs(Tree<T> tree, List<T> result)
        {
            if (tree._children.Count == 0)
            {
                result.Add(tree.Key);
                return;
            }

            foreach (var child in tree._children)
            {
                this.Dfs(child, result);
            }            
        }
    }
}
