namespace Problem02.Stack
{
    public class Node<T>
    {
        public Node(T element)
        {
            this.Value = element;
        }

        public T Value { get; set; }
        public Node<T> Next { get; set; }
    }
}