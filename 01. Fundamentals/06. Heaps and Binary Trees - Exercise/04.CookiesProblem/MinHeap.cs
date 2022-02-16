namespace _04.CookiesProblem
{
    using System;
    using System.Collections.Generic;

    public class MinHeap<T>
        where T : IComparable<T>
    {
        private List<T> _elements;

        public MinHeap()
        {
            this._elements = new List<T>();
        }

        public int Size => this._elements.Count;

        public T Dequeue()
        {
            this.ValidateEmpty();
            T toReturn = this._elements[0];
            this._elements[0] = this._elements[Size - 1];
            this._elements.RemoveAt(Size - 1);
            this.HeapifyDown(0);
            return toReturn;
        }

        public void Add(T element)
        {
            this._elements.Add(element);
            this.HeapifyUp(Size - 1);
        }

        public T Peek()
        {
            this.ValidateEmpty();
            return this._elements[0];
        }

        private void HeapifyDown(int index)
        {
            int leftIndex = index * 2 + 1;
            int rightIndex = index * 2 + 2;
            int maxIndex = leftIndex;

            if (leftIndex >= Size) return;

            if ((rightIndex < Size) && _elements[leftIndex].CompareTo(_elements[rightIndex]) > 0)
                maxIndex = rightIndex;

            if (_elements[index].CompareTo(_elements[maxIndex]) > 0)
            {
                T temp = _elements[index];
                _elements[index] = _elements[maxIndex];
                _elements[maxIndex] = temp;
                HeapifyDown(maxIndex);
            }
        }

        private void HeapifyUp(int index)
        {
            if (index == 0) return;

            int parentIndex = (index - 1) / 2;

            if (this._elements[index]
                .CompareTo(this._elements[parentIndex]) < 0)
            {
                T temp = this._elements[index];
                this._elements[index] = this._elements[parentIndex];
                this._elements[parentIndex] = temp;
                HeapifyUp(parentIndex);
            }
        }

        private void ValidateEmpty()
        {
            if (this._elements.Count == 0)
                throw new InvalidOperationException();
        }
    }
}
