using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    class Vector<T> where T : AllowToGenerics<Vector, T>
    {
        private T[] vector;
        public T GetValue(int i)
        {
            if (i > this.vector.Length) i = this.vector.Length;
            else if (i <= 0) i = 1;
            return this.vector[i - 1]; 
        }
        public int NumberOfDimensions { get { return this.vector.Length; } }
        public Vector(params T[] Elements)
        {
            this.vector = new T[Elements.Length];
            for (int i = 0; i < Elements.Length; i++)
            {
                try
                {
                    this.vector[i] = Elements[i];
                }
                catch (Exception) { throw new Exception("Dizi değerleri hatalı veya doğru değil."); }
            }
        }
        public override string ToString()
        {
            string vectorString = "(";
            foreach (var i in this.vector) vectorString += i + "\t";
            return vectorString.Substring(0, vectorString.Length - 1) + ")";
        }

        
    }
}
