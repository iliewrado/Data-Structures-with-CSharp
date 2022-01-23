namespace Problem03.ReversedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReversedList<T> : IAbstractList<T>
    {
        private const int DefaultCapacity = 4;
        private T[] _items;

        public ReversedList()
            : this(DefaultCapacity)
        {
        }

        public ReversedList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            this._items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                this.ValidateIndex(index);
                return this._items[this.Count - 1 - index];
            }
            set
            {
                this.ValidateIndex(index);
                this._items[index] = value;
            }
        }

        public int Count { get; private set; }

        public void Add(T item)
        {
            this.GrowIfNecessery();
            this._items[Count++] = item;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        public int IndexOf(T item)
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (this._items[i].Equals(item))
                {
                    return this.Count - 1 - i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            this.GrowIfNecessery();
            this.ValidateIndex(index);
            int indexToInsert = this.Count - index;

            for (int i = this.Count; i > indexToInsert; i--)
            {
                this._items[i] = this._items[i - 1];
            }
            
            this._items[indexToInsert] = item;
            this.Count++;
        }

        public bool Remove(T item)
        {
            int itemIndex = this.IndexOf(item);
            
            if (itemIndex < 0)
            {
                return false;
            }

            this.RemoveAt(itemIndex);
            return true;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);
            int itemIndex = this.Count - 1 - index;

            for (int i = itemIndex; i < this.Count - 1; i++)
            {
                this._items[i] = this._items[i + 1];
            }

            this._items[Count - 1] = default;
            this.Count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                yield return this._items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void GrowIfNecessery()
        {
            if (this.Count == this._items.Length)
            {
                this._items = this.Grow();
            }
        }

        private T[] Grow()
        {
            T[] newArray = new T[this.Count * 2];
            Array.Copy(_items, newArray, _items.Length);
            return newArray;
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= this.Count)
                throw new IndexOutOfRangeException(nameof(index));
        }
    }
}