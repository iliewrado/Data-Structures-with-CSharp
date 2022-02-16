namespace _04.CookiesProblem
{
    public class CookiesProblem
    {
        public int Solve(int k, int[] cookies)
        {
            MinHeap<int> heap = new MinHeap<int>();
            foreach (var cookie in cookies)
            {
                heap.Add(cookie);
            }

            int lessSweet = heap.Peek();
            int count = 0;
            
            while (heap.Size > 1 && lessSweet < k)
            {
                int first = heap.Dequeue();
                int second = heap.Dequeue();

                heap.Add(first + 2 * second);
                count++;

                lessSweet = heap.Peek();
            }       

            return lessSweet >= k ? count : -1;
        }
    }
}
