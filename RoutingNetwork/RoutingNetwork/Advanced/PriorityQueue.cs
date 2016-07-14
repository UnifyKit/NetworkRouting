using System;
using System.Text;

namespace NetworkRouting
{
    public class PriorityQueue<T> where T : IComparable
    {
        private T[] queueArray;
        private int size;

        public PriorityQueue()
        {
            this.queueArray = new T[11];
            this.size = 0;
        }

        public int Size
        {
            get { return size; }
        }

        public void Push(T item)
        {
            if (item == null)
            {
                throw new Exception("null");
            }
            int num = this.Size;
            if (num >= this.queueArray.Length)
            {
                this.Grow(num + 1);
            }
            this.size = num + 1;
            if (num == 0)
            {
                this.queueArray[0] = item;
            }
            else
            {
                this.SiftUp(num, item);
            }
        }

        public T Pop()
        {
            if (this.Size == 0)
            {
                return default(T);
            }

            int number = this.Size - 1;
            int takenNumber = number;
            this.size = number;

            T result = this.queueArray[0];
            T tokenObject = this.queueArray[takenNumber];
            this.queueArray[takenNumber] = default(T);
            if (takenNumber != 0)
            {
                this.SiftDown(0, tokenObject);
            }
            return result;
        }

        public T Peek()
        {
            return (this.Size != 0) ? this.queueArray[0] : default(T);
        }

        private void Grow(int num)
        {
            int length = this.queueArray.Length;
            int newLength = length + ((length >= 64) ? (length >> 1) : (length + 2));

            T[] newQueue = new T[newLength];
            Array.Copy(this.queueArray, 0, newQueue, 0, this.queueArray.Length);
            this.queueArray = newQueue;
        }

        private void SiftUp(int i, T item)
        {

            while (i > 0)
            {
                int number = (int)((uint)(i - 1) >> 1);
                T obj2 = this.queueArray[number];
                if (item.CompareTo(obj2) >= 0)
                {
                    break;
                }
                this.queueArray[i] = obj2;
                i = number;
            }
            this.queueArray[i] = item;
        }

        private void SiftDown(int i, T item)
        {
            int num = (int)((uint)this.Size >> 1);
            while (i < num)
            {
                int num2 = (i << 1) + 1;
                T obj2 = this.queueArray[num2];
                int num3 = num2 + 1;
                if (num3 < this.Size && obj2.CompareTo(this.queueArray[num3]) > 0)
                {
                    obj2 = this.queueArray[num2 = num3];
                }
                if (item.CompareTo(obj2) <= 0)
                {
                    break;
                }
                this.queueArray[i] = obj2;
                i = num2;
            }
            this.queueArray[i] = item;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                T item = this.queueArray[i];
                stringBuilder.AppendFormat("{0}, ", item.ToString());
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            return string.Format("[{0}]", stringBuilder.ToString());
        }
    }
}
