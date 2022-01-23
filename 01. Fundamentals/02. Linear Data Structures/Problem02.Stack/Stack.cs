namespace Problem02.Stack
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Stack<T> : IAbstractStack<T>
    {
        private Node<T> _top;

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            Node<T> currentNode = this._top;
            
            while (currentNode != null)
            {
                if (currentNode.Value.Equals(item))
                {
                    return true;
                }
                currentNode = currentNode.Next;
            }

            return false;
        }

        public T Peek()
        {
            this.EnsureNotEmpty();
            return this._top.Value;
        }

        public T Pop()
        {
            this.EnsureNotEmpty();

            T topItem = this._top.Value;
            Node<T> newTopItem = this._top.Next;
            this._top.Next = null;
            this._top = newTopItem;
            this.Count--;
            return topItem;
        }
                
        public void Push(T item)
        {
            Node<T> newNode = new Node<T>(item)
            {
                Next = this._top
            };
            this._top = newNode;
            this.Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currentNode = this._top;

            while (currentNode != null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() 
            => this.GetEnumerator();

        private void EnsureNotEmpty()
        {
            if (this.Count == 0)
                throw new InvalidOperationException("Stack is Empty");
        }
    }
}