namespace Problem02.DoublyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DoublyLinkedList<T> : IAbstractLinkedList<T>
    {
        private Node<T> _first;
        private Node<T> _last;

        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            Node<T> newNode = new Node<T>(item);

            if (this.Count == 0)
            {
                this._first = this._last = newNode;
            }
            else
            {
                this._first.Previous = newNode;
                newNode.Next = this._first;
                this._first = newNode;
            }

            this.Count++;
        }

        public void AddLast(T item)
        {
            Node<T> newNode = new Node<T>(item);

            if (this.Count == 0)
            {
                this._first = this._last = newNode;
            }
            else
            {
                this._last.Next = newNode;
                newNode.Previous = this._last;
                this._last = newNode;
            }

            this.Count++;
        }

        public T GetFirst()
        {
            this.EnsureNotEmpty();
            return this._first.Value;
        }

        public T GetLast()
        {
            this.EnsureNotEmpty();
            return this._last.Value;
        }

        public T RemoveFirst()
        {
            this.EnsureNotEmpty();

            T temp = this._first.Value;
            if (this._first.Next != null)
            {
                Node<T> newFirst = this._first.Next;
                this._first.Next.Previous = null;
                this._first = newFirst;
            }
            else
            {
                this._first = this._last = null;
            }

            this.Count--;
            return temp;
        }

        public T RemoveLast()
        {
            this.EnsureNotEmpty();

            T temp = this._last.Value;
            if (this._last.Previous != null)
            {
                Node<T> newLast = this._last.Previous;
                this._last.Previous.Next = null;
                this._last = newLast;
            }
            else
            {
                this._first = this._last = null;
            }

            this.Count--;
            return temp;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currentNode = this._first;

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
                throw new InvalidOperationException("SinglyLinkedList is Empty");
        }
    }
}