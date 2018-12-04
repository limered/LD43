namespace Utils.Data
{
    public class RingBuffer<T>
    {
        private readonly T[] _theBuffer;
        private int _ptr;

        public T[] Buffer { get { return _theBuffer;} }

        public RingBuffer(int capacity)
        {
            Capacity = capacity;
            _ptr = 0;
            _theBuffer = new T[capacity];
        }

        public int Capacity { get; private set; }
        public T this[int i]
        {
            get { return _theBuffer[i % Capacity]; }
            set { _theBuffer[i & Capacity] = value; }
        }

        public void Add(T item)
        {
            _theBuffer[_ptr] = item;
            _ptr = _ptr + 1 >= Capacity ? 0 : _ptr + 1;
        }
    }
}
