using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    struct Degrees
    {
        private double _value;
        public double Degree { get { return this._value; } }
        public double Radian { get { return _value * Math.PI / 180; } }

        public Degrees(double degree)
        {
            this._value = degree;
        }

        public static Degrees operator +(Degrees A1, Degrees A2)
        {
            return new Degrees(A1._value + A2._value);
        }
        public static Degrees operator -(Degrees A1, Degrees A2)
        {
            return new Degrees(A1._value - A2._value);
        }

        public static Degrees operator *(Degrees A1, Degrees A2)
        {
            return new Degrees(A1._value * A2._value);
        }
        public static Degrees operator *(Degrees A, double D)
        {
            return new Degrees(A._value * D);
        }
        public static Degrees operator *(double D, Degrees A)
        {
            return new Degrees(A._value * D);
        }

        public static Degrees operator /(Degrees A1, Degrees A2)
        {
            return new Degrees(A1._value / A2._value);
        }
        public static Degrees operator /(Degrees A, double D)
        {
            return new Degrees(A._value / D);
        }

        public static implicit operator Degrees(double D)
        {
            return new Degrees(D);
        }



        public override string ToString()
        {
            return this.Degree.ToString();
        }
    }
}
