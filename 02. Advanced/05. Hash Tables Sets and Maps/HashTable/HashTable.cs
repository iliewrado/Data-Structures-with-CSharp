namespace HashTable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    public class HashTable<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
    {
        private const int initialCapacity = 4;
        private const float loadFactor = 0.75f;
        private LinkedList<KeyValue<TKey, TValue>>[] slots;

        public int Count { get; private set; }

        public int Capacity => this.slots.Length;

        public HashTable() 
            : this (initialCapacity)
        {
        }

        public HashTable(int capacity)
        {
            this.slots = new LinkedList<KeyValue<TKey, TValue>>[capacity];
            this.Count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            this.GrowIfNeedet();
            int index = this.FindSlotNumber(key);

            if (this.slots[index] == null)
            {
                this.slots[index] = new LinkedList<KeyValue<TKey, TValue>>();
            }

            foreach (var item in this.slots[index])
            {
                if (item.Key.Equals(key))
                    throw new ArgumentException("Key already exists" + key);
            }
            
            KeyValue<TKey, TValue> newElement = new KeyValue<TKey, TValue>(key, value);
            this.slots[index].AddLast(newElement);
            this.Count++;
        }

        private int FindSlotNumber(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % this.Capacity;
        }

        private void GrowIfNeedet()
        {
            if ((float)(this.Count + 1) / this.Capacity >= loadFactor)
            {
                HashTable<TKey, TValue> newTable = 
                    new HashTable<TKey, TValue>(this.Capacity * 2);

                foreach (var element in this)
                {
                    newTable.Add(element.Key, element.Value);
                }

                this.slots = newTable.slots;
                this.Count = newTable.Count;
            }
        }

        public bool AddOrReplace(TKey key, TValue value)
        {
            try
            {
                this.Add(key, value);
            }
            catch (ArgumentException)
            {
                int index = this.FindSlotNumber(key);
                KeyValue<TKey, TValue> current = this.FirstOrDefault(x => x.Key.Equals(key));
                current.Value = value;
                return true;
            }

            return false;
        }

        public TValue Get(TKey key)
        {
            KeyValue<TKey, TValue> element = this.Find(key);
            
            if (element == null)
                throw new KeyNotFoundException();
            
            return element.Value;
        }

        public TValue this[TKey key]
        {
            get
            {
                return this.Get(key);
            }
            set
            {
                this.AddOrReplace(key, value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            KeyValue<TKey, TValue> element = this.Find(key);

            if(element != null)
            {
                value = element.Value;
                return true;
            }

            value = default;
            return false;
        }

        public KeyValue<TKey, TValue> Find(TKey key)
        {
            int index = FindSlotNumber(key);

            if (this.slots[index] != null)
            {
                foreach (var item in this.slots[index])
                {
                    if (item.Key.Equals(key))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public bool ContainsKey(TKey key)
        {
            KeyValue<TKey, TValue> element = this.Find(key);
            return element != null;
        }

        public bool Remove(TKey key)
        {
            int index = FindSlotNumber(key);
            
            if (this.slots[index] != null)
            {
                var slot = this.slots[index].First;

                while (slot != null)
                {
                    if (slot.Value.Key.Equals(key))
                    {
                        this.slots[index].Remove(slot);
                        this.Count--;
                        return true;
                    }

                    slot = slot.Next;
                }
            }

            return false;
        }

        public void Clear()
        {
            this.slots = new LinkedList<KeyValue<TKey, TValue>>[initialCapacity];
            this.Count = 0;
        }

        public IEnumerable<TKey> Keys => this.Select(x => x.Key);

        public IEnumerable<TValue> Values => this.Select(x => x.Value);

        public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
        {
            foreach (var slot in this.slots)
            {
                if (slot != null)
                {
                    foreach (var item in slot)
                    {
                        yield return item;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
