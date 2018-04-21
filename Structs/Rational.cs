using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Mathematics
{
    struct Rational : AllowToGenerics<Vector, Rational>, AllowToGenerics<Matrix, Rational>
    {

        #region: Properties and Fields

        private Integer numerator;
        private bool _isPositive;
        private int logDenominator;
        public bool IsZero { get { return this.numerator.IsZero; } }
        public int DigitLength { get { return this.numerator.DigitLength; } }
        public bool IsPositive { get { return this._isPositive; } }
        private static int _sensibility = 40;
        public static int Sensibility { get { return Rational._sensibility; } set { Rational._sensibility = value; } }

        #endregion

        #region: Constructors

        public Rational(string rational)
        {
            string[] stringArray = rational.Split(',');
            if (stringArray.Length == 2)
            {
                rational = rational.TrimEnd('0');
                stringArray = rational.Split(',');
                this.logDenominator = stringArray[1].Length;
                string fullString = stringArray[0] + stringArray[1];
                
                if (this.logDenominator > Rational._sensibility)
                {
                    int trimindex = this.logDenominator - Rational._sensibility;
                    this.logDenominator = Rational._sensibility;
                    fullString = fullString.Substring(0, stringArray[0].Length + logDenominator);
                }
                this.numerator = new Integer(fullString);
                this._isPositive = this.numerator.IsPositive;
                this.numerator.IsPositive = true;
                if (this.numerator.IsZero) this.logDenominator = 0;
            }
            else if (stringArray.Length == 1)
            {
                this.logDenominator = 0;
                this.numerator = new Integer(stringArray[0]);
                this._isPositive = this.numerator.IsPositive;
                this.numerator.IsPositive = true;
            }
            else throw new Exception();

        }
        public Rational(Integer integer, int logDenominator)
        {
            if (!integer.IsZero)
            {
                if (logDenominator != 0)
                {
                    int trimIndex = integer.DigitLength; int loops = logDenominator;
                    for (int i = 0; i < loops; i++)
                    {
                        if (integer[integer.DigitLength - i - 1] == 0) { trimIndex--; logDenominator--; }
                        else break;
                    }
                    integer = integer.RemoveDigit(trimIndex);
                }
                if (logDenominator > Rational._sensibility)
                {
                    int trimIndex = logDenominator - Rational._sensibility;
                    logDenominator = Rational._sensibility;
                    integer = integer.RemoveDigit(integer.DigitLength - trimIndex);
                }
                this.logDenominator = logDenominator;
                if (integer.IsPositive) { this.numerator = integer; this._isPositive = true; }
                else { integer.IsPositive = true; this._isPositive = false; this.numerator = integer; }
                if (this.numerator.IsZero) { this.logDenominator = 0; }
            }
            else
            {
                this.logDenominator = 0;
                this.numerator = new Integer(true, 0);
                this._isPositive = true;
            }
        }

        #endregion

        #region: Operator Overloads

        public static Rational operator +(Rational R1, Rational R2)
        {
            return R1.Add(R2);
        }
        public static Rational operator -(Rational R1, Rational R2)
        {
            return R1.Add(-R2);
        }
        public static Rational operator *(Rational R1, Rational R2)
        {
            return R1.Mul(R2);
        }
        public static Rational operator /(Rational R1, Rational R2)
        {
            return R1.Div(R2);
        }

        public static Rational operator -(Rational R)
        {
            R._isPositive = !R._isPositive;
            return R;
        }
        public static Rational operator ++(Rational R)
        {
            return R.Add(new Rational("1"));
        }
        public static Rational operator --(Rational R)
        {
            return R.Add(-(new Rational("1")));
        }


        public static bool operator ==(Rational R1, Rational R2) 
        {
            return R1.CompareTo(R2) == 0;
        }
        public static bool operator !=(Rational R1, Rational R2) 
        {
            return R1.CompareTo(R2) != 0;
        }
        public static bool operator <(Rational R1, Rational R2) 
        {
            return R1.CompareTo(R2) == -1;
        }
        public static bool operator >(Rational R1, Rational R2) 
        {
            return R1.CompareTo(R2) == 1;
        }
        public static bool operator <=(Rational R1, Rational R2) 
        {
            int Comp = R1.CompareTo(R2);
            return Comp == -1 || Comp == 0;
        }
        public static bool operator >=(Rational R1, Rational R2) 
        {
            int Comp = R1.CompareTo(R2);
            return Comp == 1 || Comp == 0;
        }

        public static implicit operator Rational(Integer I)
        {
            return new Rational(I, 0);
        }
        public static implicit operator Rational(int I)
        {
            return new Rational(I, 0);
        }

        #endregion

        #region: Methods

        public static Integer Floor(Rational rational)
        {
            int loops = rational.DigitLength - rational.logDenominator;
            sbyte[] s = new sbyte[loops];
            for (int i = 0; i < loops; i++)
            {
                s[i] = rational.numerator[i];
            }
            Integer I = new Integer(rational._isPositive, s);
            if (I.IsPositive)
            {
                return I;
            }
            else
            {
                if (rational.logDenominator == 0) return I;
                return I - (new Integer(true, new sbyte[1] { 1 }));
            }
        }
        public static Integer Round(Rational rational)
        {
            if (rational.logDenominator == 0) { return rational.numerator; }
            else
            {
                Integer I = Rational.Floor(rational);
                if (rational._isPositive)
                {
                    if (rational.numerator[rational.DigitLength - rational.logDenominator] < 5)
                    {
                        return I;
                    }
                    else
                    {
                        return I + (new Integer(true, new sbyte[1] { 1 }));
                    }
                }
                else
                {
                    if (rational.numerator[rational.DigitLength - rational.logDenominator] >= 5)
                    {
                        return I;
                    }
                    else
                    {
                        return I + (new Integer(true, new sbyte[1] { 1 }));
                    }
                }
            }
        }
        public static Integer Ceiling(Rational rational)
        {
            if (rational.logDenominator == 0)
            {
                if (rational._isPositive) return rational.numerator;
                else return -rational.numerator; 
            }
            else
            {
                Integer I = Rational.Floor(rational);
                return I + (new Integer(true, new sbyte[1] { 1 }));
            }
        }

        public int CompareTo(Rational otherRational)
        {
            if (this._isPositive && !otherRational._isPositive) return 1;
            else if (!this._isPositive && otherRational._isPositive) return -1;
            else
            {
                if (this._isPositive)
                {
                    Rational R = this.Sub(otherRational);
                    if (R.numerator.IsZero) return 0;
                    else if (R.IsPositive) return 1;
                    else return -1;
                }
                else
                {
                    Rational R = this.Sub(otherRational);
                    if (R.numerator.IsZero) return 0;
                    else if (R.IsPositive) return -1;
                    else return 1;
                }
            }
        }

        #endregion

        #region: Helper Methods

        private Rational Add(Rational otherRational)
        {
            if (this._isPositive == otherRational._isPositive)
            {
                if (this.logDenominator > otherRational.logDenominator)
                {
                    sbyte[] s = new sbyte[this.logDenominator - otherRational.logDenominator + 1];
                    s[0] = 1;
                    Integer I = new Integer(true, s);
                    Integer ThisI = otherRational.numerator * I;
                    ThisI = ThisI + this.numerator;
                    if (this.IsPositive) ThisI.IsPositive = true;
                    else ThisI.IsPositive = false;
                    return new Rational(ThisI, this.logDenominator);
                }
                else if (this.logDenominator < otherRational.logDenominator)
                {
                    sbyte[] s = new sbyte[otherRational.logDenominator - this.logDenominator + 1];
                    s[0] = 1;
                    Integer I = new Integer(true, s);
                    Integer ThisI = this.numerator * I;
                    ThisI = ThisI + otherRational.numerator;
                    if (this.IsPositive) ThisI.IsPositive = true;
                    else ThisI.IsPositive = false;
                    return new Rational(ThisI, otherRational.logDenominator);
                }
                else
                {
                    if (this._isPositive) return new Rational(this.numerator + otherRational.numerator, this.logDenominator);
                    else return new Rational(-this.numerator + otherRational.numerator, this.logDenominator);
                }
            }
            else
            {
                if (this.logDenominator > otherRational.logDenominator)
                {
                    sbyte[] s = new sbyte[this.logDenominator - otherRational.logDenominator + 1];
                    s[0] = 1;
                    Integer I = new Integer(true, s);
                    Integer ThisI = otherRational.numerator * I;
                    if (this.IsPositive) ThisI = this.numerator - ThisI;
                    else { ThisI = ThisI - this.numerator; }
                    return new Rational(ThisI, this.logDenominator);
                }
                else if (this.logDenominator < otherRational.logDenominator)
                {
                    sbyte[] s = new sbyte[otherRational.logDenominator - this.logDenominator + 1];
                    s[0] = 1;
                    Integer I = new Integer(true, s);
                    Integer ThisI = this.numerator * I;
                    if (this.IsPositive) ThisI = ThisI - otherRational.numerator;
                    else { ThisI = otherRational.numerator - ThisI; }
                    return new Rational(ThisI, otherRational.logDenominator);
                }
                else
                {
                    if (this.IsPositive) return new Rational(this.numerator - otherRational.numerator, this.logDenominator);
                    else { return new Rational(otherRational.numerator - this.numerator, this.logDenominator); }
                }
            }
        }
        private Rational Sub(Rational otherRational)
        {
            return this.Add(-otherRational);
        }
        private Rational Mul(Rational otherRational)
        {
            Integer I = this.numerator * otherRational.numerator;
            if (this._isPositive != otherRational._isPositive) I = -I;
            return new Rational(I, this.logDenominator + otherRational.logDenominator);
        }
        private Rational Div(Rational otherRational)
        {
            if (otherRational.numerator.IsZero) throw new Exception("0'a bölünmez.");
            sbyte[] array = new sbyte[Rational._sensibility + 1];
            array[0] = 1;
            Integer I = new Integer(true, array);
            Integer ThisI = this.numerator * I;
            Integer Divisor = ThisI / otherRational.numerator;
            if (this._isPositive == otherRational._isPositive) return new Rational(Divisor, Rational._sensibility + this.logDenominator - otherRational.logDenominator);
            else return new Rational(-Divisor, Rational._sensibility + this.logDenominator - otherRational.logDenominator);
        }

        #endregion

        public override string ToString()
        {
            if (logDenominator == 0)
            {
                if (this._isPositive) return this.numerator.ToString();
                else return "-" + this.numerator.ToString();
            }
            else if (logDenominator >= this.numerator.DigitLength)
            {
                string s = "0,";
                int zeroCount = logDenominator - this.numerator.DigitLength;
                for (int i = 0; i < zeroCount; i++)
                {
                    s += 0;
                }
                s += this.numerator.ToString();
                if (this._isPositive) return s;
                else return "-" + s;
            }
            else
            {
                string s = "";
                for (int i = 0; i < this.numerator.DigitLength - this.logDenominator; i++)
                {
                    s += this.numerator[i];
                }
                s += ",";
                for (int i = this.numerator.DigitLength - this.logDenominator; i < this.numerator.DigitLength; i++)
                {
                    s += this.numerator[i];
                }
                if (this._isPositive) return s;
                else return "-" + s;
            }
        }




        Rational AllowToGenerics<Vector, Rational>.Add(Rational operand)
        {
            return this.Add(operand);
        }
        Rational AllowToGenerics<Vector, Rational>.Sub(Rational operand)
        {
            return this.Sub(operand);
        }
        Rational AllowToGenerics<Vector, Rational>.Mul(Rational operand)
        {
            return this.Mul(operand);
        }
        Rational AllowToGenerics<Vector, Rational>.Div(Rational operand)
        {
            return this.Div(operand);
        }
        Rational AllowToGenerics<Matrix, Rational>.Add(Rational operand)
        {
            return this.Add(operand);
        }
        Rational AllowToGenerics<Matrix, Rational>.Sub(Rational operand)
        {
            return this.Sub(operand);
        }
        Rational AllowToGenerics<Matrix, Rational>.Mul(Rational operand)
        {
            return this.Mul(operand);
        }
        Rational AllowToGenerics<Matrix, Rational>.Div(Rational operand)
        {
            return this.Div(operand);
        }

        Rational IDivisible<Rational>.Add(Rational operand)
        {
            return this.Add(operand);
        }
        Rational IDivisible<Rational>.Sub(Rational operand)
        {
            return this.Sub(operand);
        }
        Rational IDivisible<Rational>.Mul(Rational operand)
        {
            return this.Mul(operand);
        }
        Rational IDivisible<Rational>.Div(Rational operand)
        {
            return this.Div(operand);
        }
        Rational IOperable<Rational>.Add(Rational operand)
        {
            return this.Add(operand);
        }
        Rational IOperable<Rational>.Sub(Rational operand)
        {
            return this.Sub(operand);
        }
        Rational IOperable<Rational>.Mul(Rational operand)
        {
            return this.Mul(operand);
        }
    }
}
