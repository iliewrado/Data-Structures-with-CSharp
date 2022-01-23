namespace Problem03.Queue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Queue<T> : IAbstractQueue<T>
    {
        private Node<T> _head;
        private Node<T> _tail;

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            Node<T> currentNode = this._head;
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

        public T Dequeue()
        {
            this.EnsureNotEmpty();
            T temp = this._head.Value;
            
            if (_head.Next != null)
            {
                Node<T> newHead = this._head.Next;
                this._head.Next.Previous = null;
                this._head = newHead;
            }
            else
            {
                _head = _tail = null;
            }
                  
            Count--;
            return temp;
        }

        public void Enqueue(T item)
        {
            Node<T> newNode = new Node<T>(item);

            if (Count == 0)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Previous = this._tail;
                this._tail.Next = newNode;
                this._tail = newNode;
            }

            Count++;
        }

        public T Peek()
        {
            this.EnsureNotEmpty();
            return this._head.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currentNode = this._head;

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
                throw new InvalidOperationException("Queue is Empty");
        }
    }
}