using System;
using System.Linq;

namespace NReplayGain
{
    struct CPtr<T>
    {
        public T[] Array;
        public int Index;

        public CPtr(T[] array, int index = 0)
        {
            this.Array = array;
            this.Index = index;
        }

        public T this[int index]
        {
            get
            {
                return this.Array[this.Index + index];
            }
            set
            {
                this.Array[this.Index + index] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.Array.Length - this.Index;
            }
        }

        public static CPtr<T> operator +(CPtr<T> pointer, long offset)
        {
            if (offset > int.MaxValue)
            {
                throw new ArgumentException("Offset is too big!");
            }
            return new CPtr<T>(pointer.Array, pointer.Index + (int)offset);
        }

        public static CPtr<T> operator ++(CPtr<T> pointer)
        {
            return pointer + 1;
        }
    }
}
