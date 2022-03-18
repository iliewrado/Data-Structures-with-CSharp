namespace Problem02.DoublyLinkedList
{
    public class Node<T>
    {
        public Node(T element)
        {
            this.Value = element;
        }

        public T Value { get; set; }

        public Node<T> Next { get; set; }

        public Node<T> Previous { get; set; }
    }
}