using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Mathematics
{
    struct Integer : AllowToGenerics<Vector, Integer>, AllowToGenerics<Matrix, Integer>
    {
        #region: Properties and Fields

        private sbyte[] integer;
        private bool _isPositive;
        public static Integer One { get { return new Integer(true, new sbyte[1] { 1 }); } }
        public int DigitLength { get { return this.integer.Length; } }
        public bool IsPositive { get { return this._isPositive; } set { this._isPositive = value; } }
        public bool IsOdd { get { return !this.IsEven; } }
        public bool IsEven { get { return 2 * (this.integer[this.integer.Length - 1] / 2) == this.integer[this.integer.Length - 1]; } }
        public int DigitsSum { get { int s = 0; for (int i = 0; i < this.DigitLength; i++) s += this.integer[i]; return s; } }
        public sbyte this[int index] { get { return this.integer[index]; } }
        public bool IsZero { get { return this.integer.Length == 1 && this.integer[0] == 0; } }

        #endregion

        #region: Constructors

        public Integer(string integer)
        {
            if (integer[0] != '-')
            {
                integer = integer.TrimStart('0');
                if (integer.Length == 0) integer = "0";
                this.integer = new sbyte[integer.Length];
                for (int i = 0; i < integer.Length; i++)
                {
                    this.integer[i] = sbyte.Parse(integer[i].ToString());
                }
                this._isPositive = true;
            }
            else
            {
                integer = integer.TrimStart('-');
                integer = integer.TrimStart('0');
                if (integer.Length == 0) integer = "0";
                this.integer = new sbyte[integer.Length];
                for (int i = 0; i < integer.Length; i++)
                {
                    this.integer[i] = sbyte.Parse(integer[i].ToString());
                }
                this._isPositive = false;
                if (this.integer.Length == 1 && this.integer[0] == 0) { this._isPositive = true; }
            }
        }
        public Integer(bool isPositive, params sbyte[] integer)
        {
            if (integer[0] == 0) integer = Integer.ByteArraysZeroTrimStart(integer);
            this.integer = integer;
            this._isPositive = isPositive;
            if (this.integer.Length == 1 && this.integer[0] == 0) { this._isPositive = true; }
        }

        #endregion

        #region: Methods

        public Integer RemoveDigit(int startIndex, int length)
        {
            if (startIndex < 0) return this;
            if (startIndex + length < this.integer.Length)
            {
                sbyte[] array = new sbyte[this.DigitLength - length];
                for (int i = 0; i < startIndex; i++)
                {
                    array[i] = this.integer[i];
                }
                for (int i = startIndex + length; i < this.integer.Length; i++)
                {
                    array[i - length] = this.integer[i];
                }
                if (array.Length > 0) return new Integer(this._isPositive, array);
                else return new Integer(true, 0);
            }
            else return this.RemoveDigit(startIndex);
        }
        public Integer RemoveDigit(int startIndex)
        {
            if (startIndex == 0) return new Integer(true, 0);
            if (startIndex < this.integer.Length && startIndex >= 0)
            {
                sbyte[] array = new sbyte[startIndex];
                for (int i = 0; i < startIndex; i++)
                {
                    array[i] = this.integer[i];
                }
                return new Integer(this._isPositive, array);
            }
            else return this;
        }   
        public int CompareTo(Integer obj)
        {
            if (this._isPositive && !obj._isPositive) return 1;
            else if (!this._isPositive && obj._isPositive) return -1;
            else
            {
                if (this._isPositive)
                {
                    if (this.DigitLength > obj.DigitLength) return 1;
                    else if (obj.DigitLength > this.DigitLength) return -1;
                    else
                    {
                        for (int i = 0; i < this.integer.Length; i++)
                        {
                            if (this.integer[i] > obj.integer[i]) return 1;
                            else if (this.integer[i] < obj.integer[i]) return -1;
                        }
                        return 0;
                    }
                }
                else
                {
                    if (this.DigitLength > obj.DigitLength) return -1;
                    else if (obj.DigitLength > this.DigitLength) return 1;
                    else
                    {
                        for (int i = 0; i < this.integer.Length; i++)
                        {
                            if (this.integer[i] > obj.integer[i]) return -1;
                            else if (this.integer[i] < obj.integer[i]) return 1;
                        }
                        return 0;
                    }
                }
            }
        }
        public Integer DividedByWithReminder(Integer otherInt, out Integer Rem)
        {
            if (otherInt.IsZero) throw new Exception("0'a bölünmez.");
            bool thisIsPositive = this._isPositive, otherIsPositive = otherInt._isPositive;
            otherInt._isPositive = true;
            Integer thisInt = this; thisInt._isPositive = true;
            if (thisInt.CompareTo(otherInt) == -1)
            {
                if (thisIsPositive == otherIsPositive) Rem = this;
                else
                {
                    if (thisIsPositive) Rem = otherInt.Sub(this);
                    else Rem = otherInt.Add(this);
                }
                return new Integer(true, 0);
            }

            int loops = this.integer.Length - otherInt.integer.Length;
            sbyte[] array = new sbyte[loops + 1];
            Integer ce, otherI;
            for (int i = loops; i >= 0; i--)
            {
                sbyte divDigit = 0;
                ce = Integer.CreateExp10(i);
                otherI = ce.Mul(otherInt);
                while (true)
                {
                    if (thisInt.CompareTo(otherI) == -1) break;
                    thisInt = thisInt.Sub(otherI);
                    divDigit++;
                }
                array[loops - i] = divDigit;
            }
            if (thisIsPositive == otherIsPositive) Rem = thisInt;
            else Rem = otherInt.Sub(thisInt);
            return new Integer(thisIsPositive == otherIsPositive, array);
        }
        
        public static Integer Pow(Integer I1, uint I2)
        {
            if (I2 == 0) return new Integer(true, new sbyte[1] { 1 });
            else if (I2 <= 2) { if (I2 == 1) return I1; else return I1.Mul(I1); }
            else
            {
                bool IsEven = (I2 & 1) == 0;
                if (IsEven)
                {
                    return Integer.Pow(Integer.Pow(I1, I2 / 2), 2);
                }
                else
                {
                    return Integer.Pow(Integer.Pow(I1, I2 / 2), 2).Mul(I1);
                }
            }
        }
        public static Integer Factorial(uint value)
        {
            uint d = value; int n;
            for (n = -1; d != 0; n++)
            {
                d = d >> 1;
            }
            uint Val = (uint)Math.Pow(2, n);
            
            List<Integer> array = new List<Integer>();
            array.Add(1);
            Integer temp = 1, T = 1; uint reg1, reg2;
            for (int j = 1; j < n; j++)
            {
                reg1 = (uint)Math.Pow(2, j - 1); reg2 = (uint)Math.Pow(2, j);
                for (uint i = reg1; i < reg2; i++)
                {
                    temp *= 2 * i + 1;
                }
                T = T * temp;
                temp = 1;
                array.Add(T);
            }
            Integer inner = Integer.One;
            for (int i = 1; i < n; i++)
            {
                inner *= array[i];
            }
            for (uint i = Val + 1; i <= value; i++)
            {
                temp *= i;
            }
            return temp * inner * Integer.Pow(2, Val - 1);
        }
        

        #endregion

        #region: Operator Overloads

        public static Integer operator -(Integer I)
        {
            I._isPositive = !I._isPositive;
            return I;
        }
        public static Integer operator ++(Integer I)
        {
            return I.Add(Integer.One);
        }
        public static Integer operator --(Integer I)
        {
            return I.Sub(Integer.One);
        }


        public static Integer operator +(Integer I1, Integer I2)
        {
            if (I1._isPositive && I2._isPositive)
            {
                return new Integer(true, Integer.Addtion(I1.integer, I2.integer));
            }
            else if (I1._isPositive && !I2._isPositive)
            {
                return I1.Sub(-I2);
            }
            else if (!I1._isPositive && I2._isPositive)
            {
                return I2.Sub(-I1);
            }
            else
            {
                return new Integer(false, Integer.Addtion(I1.integer, I2.integer));
            }
        }
        public static Integer operator -(Integer I1, Integer I2)
        {
            if (I1._isPositive && I2._isPositive)
            {
                int CompRes = I1.CompareTo(I2);
                if (CompRes == 1)
                {
                    return new Integer(true, Integer.Subtraction(I1.integer, I2.integer));
                }
                else if (CompRes == -1)
                {
                    return new Integer(false, Integer.Subtraction(I2.integer, I1.integer));
                }
                else return new Integer(true, 0);
            }
            else if (I1._isPositive && !I2._isPositive)
            {
                return new Integer(true, Integer.Addtion(I1.integer, I2.integer));
            }
            else if (!I1._isPositive && I2._isPositive)
            {
                return new Integer(false, Integer.Addtion(I1.integer, I2.integer));
            }
            else
            {
                int CompRes = I1.CompareTo(I2);
                if (CompRes == -1)
                {
                    return new Integer(false, Integer.Subtraction(I1.integer, I2.integer));
                }
                else if (CompRes == 1)
                {
                    return new Integer(true, Integer.Subtraction(I2.integer, I1.integer));
                }
                else return new Integer(true, 0);
            }
        }
        public static Integer operator *(Integer I1, Integer I2)
        {
            return new Integer(I1._isPositive == I2._isPositive, Integer.Multiplication(I1.integer, I2.integer));
        }
        public static Integer operator /(Integer I1, Integer I2)
        {
            if (I2.IsZero) throw new Exception("0'a bölünmez.");
            bool thisIsPositive = I1._isPositive, otherIsPositive = I2._isPositive;
            I2._isPositive = true;
            Integer thisInt = I1; thisInt._isPositive = true;
            if (thisInt.CompareTo(I2) == -1)
            {
                return new Integer(true, 0);
            }
            int loops = I1.integer.Length - I2.integer.Length;
            sbyte[] array = new sbyte[loops + 1];
            Integer ce, otherI;
            for (int i = loops; i >= 0; i--)
            {
                sbyte divDigit = 0;
                ce = Integer.CreateExp10(i);
                otherI = ce.Mul(I2);
                while (true)
                {
                    if (thisInt.CompareTo(otherI) == -1) break;
                    thisInt = thisInt.Sub(otherI);
                    divDigit++;
                }
                array[loops - i] = divDigit;
            }
            return new Integer(thisIsPositive == otherIsPositive, array);
        }
        public static Integer operator %(Integer I1, Integer I2)
        {
            if (I2.IsZero) throw new Exception("Mod 0 bulunmaz.");
            bool thisIsPositive = I1._isPositive, otherIsPositive = I2._isPositive;
            I2._isPositive = true;
            Integer thisInt = I1; thisInt._isPositive = true;
            int Comp = I1.CompareTo(I2);
            if (Comp == -1)
            {
                if (thisIsPositive == otherIsPositive) return thisInt;
                else return I2.Sub(thisInt);
            }
            if (Comp == 0) return new Integer(true, 0);
            int loops = I1.integer.Length - I2.integer.Length;
            Integer ce, otherI;
            for (int i = loops; i >= 0; i--)
            {
                ce = Integer.CreateExp10(i);
                otherI = ce.Mul(I2);
                while (true)
                {
                    if (thisInt.CompareTo(otherI) == -1) break;
                    thisInt = thisInt.Sub(otherI);
                }
            }
            if (thisIsPositive == otherIsPositive) return thisInt;
            else return I2.Sub(thisInt);
        }

        
        public static bool operator ==(Integer I1, Integer I2)
        {
            return I1.CompareTo(I2) == 0;
        }
        public static bool operator !=(Integer I1, Integer I2)
        {
            return I1.CompareTo(I2) != 0;
        }
        public static bool operator <(Integer I1, Integer I2)
        {
            return I1.CompareTo(I2) == -1;
        }
        public static bool operator >(Integer I1, Integer I2)
        {
            return I1.CompareTo(I2) == 1;
        }
        public static bool operator <=(Integer I1, Integer I2)
        {
            int Comp = I1.CompareTo(I2);
            return Comp == 0 || Comp == -1;
        }
        public static bool operator >=(Integer I1, Integer I2)
        {
            int Comp = I1.CompareTo(I2);
            return Comp == 0 || Comp == 1;
        }


        public static explicit operator Integer(Rational rational)
        {
            if (rational.IsPositive) return Rational.Floor(rational);
            else return Rational.Ceiling(rational);
        }
        public static implicit operator Integer(int Int)
        {
            return new Integer(Int.ToString());
        }
        public static implicit operator Integer(uint Int)
        {
            return new Integer(Int.ToString());
        }


        #endregion

        #region: Helper Methods

        private static sbyte[] Addtion(sbyte[] array1, sbyte[] array2)
        {
            int loops; sbyte[] array;
            if (array1.Length > array2.Length)
            {
                array = new sbyte[array1.Length + 1];
                loops = array2.Length;
                for (int i = 1; i <= loops; i++)
                {
                    array[array.Length - i] = (sbyte)(array1[array1.Length - i] + array2[array2.Length - i]);
                }
                for (int i = loops + 1; i <= array1.Length; i++)
                {
                    array[array.Length - i] = array1[array1.Length - i];
                }
            }
            else if (array2.Length > array1.Length)
            {
                loops = array1.Length;
                array = new sbyte[array2.Length + 1];
                for (int i = 1; i <= loops; i++)
                {
                    array[array.Length - i] = (sbyte)(array1[array1.Length - i] + array2[array2.Length - i]);
                }
                for (int i = loops + 1; i <= array2.Length; i++)
                {
                    array[array.Length - i] = array2[array2.Length - i];
                }
            }
            else
            {
                array = new sbyte[array1.Length + 1];
                loops = array2.Length;
                for (int i = 1; i <= loops; i++)
                {
                    array[array.Length - i] = (sbyte)(array1[array1.Length - i] + array2[array2.Length - i]);
                }
            }
            for (int i = array.Length - 1; i >= 1; i--)
            {
                if (array[i] > 9) { array[i] %= 10; array[i - 1]++; }
            }
            return array;
        }
        private static sbyte[] Subtraction(sbyte[] array1, sbyte[] array2)
        {
            int loops; sbyte[] array;
            array = new sbyte[array1.Length];
            loops = array2.Length;
            for (int i = 1; i <= loops; i++)
            {
                array[array.Length - i] = (sbyte)(array1[array1.Length - i] - array2[array2.Length - i]);
            }
            for (int i = loops + 1; i <= array1.Length; i++)
            {
                array[array.Length - i] = array1[array1.Length - i];
            }
            for (int i = array.Length - 1; i >= 1; i--)
            {
                if (array[i] < 0) { array[i] += 10; array[i - 1]--; }
            }
            return array;
        }
        private static sbyte[] Multiplication(sbyte[] array1, sbyte[] array2)
        {
            sbyte[] array = new sbyte[array1.Length + array2.Length];
            for (int i = array1.Length - 1; i >= 0; i--)
            {
                for (int j = array2.Length - 1; j >= 0; j--)
                {
                    array[i + j + 1] += (sbyte)(array1[i] * array2[j]);
                    if (array[i + j + 1] > 9)
                    {
                        array[i + j] += (sbyte)(array[i + j + 1] / 10);
                        array[i + j + 1] %= 10;
                    }
                }
            }
            return array;
        }
        private static sbyte[] ByteArraysZeroTrimStart(sbyte[] array)
        {
            if (array[0] != 0) return array;
            else if (array.Length == 0) return new sbyte[1] { 0 };
            else
            {
                int startindex = array.Length;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != 0) { startindex = i; break; }
                }
                sbyte[] newArray = new sbyte[array.Length - startindex];
                for (int i = startindex; i < array.Length; i++)
                {
                    newArray[i - startindex] = array[i];
                }
                if (newArray.Length == 0) return new sbyte[1] { 0 };
                else return newArray;
            }
        }

        private static Integer CreateExp10(int Log)
        {
            sbyte[] array = new sbyte[Log + 1];
            array[0] = 1;
            return new Integer(true, array);
        }

        private Integer Add(Integer otherInt)
        {
            if (this._isPositive && otherInt._isPositive)
            {
                return new Integer(true, Integer.Addtion(this.integer, otherInt.integer));
            }
            else if (this._isPositive && !otherInt._isPositive)
            {
                return this.Sub(-otherInt);
            }
            else if (!this._isPositive && otherInt._isPositive)
            {
                return otherInt.Sub(-this);
            }
            else
            {
                return new Integer(false, Integer.Addtion(this.integer, otherInt.integer));
            }
        }
        private Integer Sub(Integer otherInt)
        {
            if (this._isPositive && otherInt._isPositive)
            {
                int CompRes = this.CompareTo(otherInt);
                if (CompRes == 1)
                {
                    return new Integer(true, Integer.Subtraction(this.integer, otherInt.integer));
                }
                else if (CompRes == -1)
                {
                    return new Integer(false, Integer.Subtraction(otherInt.integer, this.integer));
                }
                else return new Integer(true, 0);
            }
            else if (this._isPositive && !otherInt._isPositive)
            {
                return new Integer(true, Integer.Addtion(this.integer, otherInt.integer));
            }
            else if (!this._isPositive && otherInt._isPositive)
            {
                return new Integer(false, Integer.Addtion(this.integer, otherInt.integer));
            }
            else
            {
                int CompRes = this.CompareTo(otherInt);
                if (CompRes == -1)
                {
                    return new Integer(false, Integer.Subtraction(this.integer, otherInt.integer));
                }
                else if (CompRes == 1)
                {
                    return new Integer(true, Integer.Subtraction(otherInt.integer, this.integer));
                }
                else return new Integer(true, 0);
            }
        }
        private Integer Mul(Integer otherInt)
        {
            return new Integer(this._isPositive == otherInt._isPositive, Integer.Multiplication(this.integer, otherInt.integer));
        }
        private Integer Div(Integer otherInt)
        {
            if (otherInt.IsZero) throw new Exception("0'a bölünmez.");
            bool thisIsPositive = this._isPositive, otherIsPositive = otherInt._isPositive;
            otherInt._isPositive = true;
            Integer thisInt = this; thisInt._isPositive = true;
            if (thisInt.CompareTo(otherInt) == -1)
            {
                return new Integer(true, 0);
            }
            int loops = this.integer.Length - otherInt.integer.Length;
            sbyte[] array = new sbyte[loops + 1];
            Integer ce, otherI;
            for (int i = loops; i >= 0; i--)
            {
                sbyte divDigit = 0;
                ce = Integer.CreateExp10(i);
                otherI = ce.Mul(otherInt);
                while (true)
                {
                    if (thisInt.CompareTo(otherI) == -1) break;
                    thisInt = thisInt.Sub(otherI);
                    divDigit++;
                }
                array[loops - i] = divDigit;
            }
            return new Integer(thisIsPositive == otherIsPositive, array);
        }
        private Integer Rem(Integer otherInt)
        {
            if (otherInt.IsZero) throw new Exception("Mod 0 bulunmaz.");
            bool thisIsPositive = this._isPositive, otherIsPositive = otherInt._isPositive;
            otherInt._isPositive = true;
            Integer thisInt = this; thisInt._isPositive = true;
            int Comp = this.CompareTo(otherInt);
            if (Comp == -1)
            {
                if (thisIsPositive == otherIsPositive) return thisInt;
                else return otherInt.Sub(thisInt);
            }
            if (Comp == 0) return new Integer(true, 0);
            int loops = this.integer.Length - otherInt.integer.Length;
            Integer ce, otherI;
            for (int i = loops; i >= 0; i--)
            {
                ce = Integer.CreateExp10(i);
                otherI = ce.Mul(otherInt);
                while (true)
                {
                    if (thisInt.CompareTo(otherI) == -1) break;
                    thisInt = thisInt.Sub(otherI);
                }
            }
            if (thisIsPositive == otherIsPositive) return thisInt;
            else return otherInt.Sub(thisInt);
        }

        #endregion

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < this.integer.Length; i++)
            {
                s += this.integer[i];
            }
            if (this._isPositive) return s;
            else return "-" + s;
        }

        Integer AllowToGenerics<Vector, Integer>.Add(Integer operand)
        {
            return this.Add(operand);
        }
        Integer AllowToGenerics<Vector, Integer>.Sub(Integer operand)
        {
            return this.Sub(operand);
        }
        Integer AllowToGenerics<Vector, Integer>.Mul(Integer operand)
        {
            return this.Mul(operand);
        }
        Integer AllowToGenerics<Vector, Integer>.Div(Integer operand)
        {
            return this.Div(operand);
        }
        Integer AllowToGenerics<Matrix, Integer>.Add(Integer operand)
        {
            return this.Add(operand);
        }
        Integer AllowToGenerics<Matrix, Integer>.Sub(Integer operand)
        {
            return this.Sub(operand);
        }
        Integer AllowToGenerics<Matrix, Integer>.Mul(Integer operand)
        {
            return this.Mul(operand);
        }
        Integer AllowToGenerics<Matrix, Integer>.Div(Integer operand)
        {
            return this.Div(operand);
        }
        Integer IDivisible<Integer>.Add(Integer operand)
        {
            return this.Add(operand);
        }
        Integer IDivisible<Integer>.Sub(Integer operand)
        {
            return this.Sub(operand);
        }
        Integer IDivisible<Integer>.Mul(Integer operand)
        {
            return this.Mul(operand);
        }
        Integer IDivisible<Integer>.Div(Integer operand)
        {
            return this.Div(operand);
        }
        Integer IOperable<Integer>.Add(Integer operand)
        {
            return this.Add(operand);
        }
        Integer IOperable<Integer>.Sub(Integer operand)
        {
            return this.Sub(operand);
        }
        Integer IOperable<Integer>.Mul(Integer operand)
        {
            return this.Mul(operand);
        }

    }
}
