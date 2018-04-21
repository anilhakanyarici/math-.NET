using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Mathematics
{
    struct Real
    {
        private BigInteger numerator;
        private int Log10Denominator;
        private bool _isPositive;
        private static int _sens = 30;
        public static int Sensibility { get { return Real._sens; } set { Real._sens = value; } }
        public bool IsPositive { get { return this._isPositive; } }
        public bool IsInteger { get { return this.Log10Denominator == 0; } }
        public Real PartOfInt 
        {
            get 
            {
                if (this.IsPositive) return new Real(this.numerator / BigInteger.Pow(10, this.Log10Denominator), 0);
                else 
                {
                    if(this.Log10Denominator == 0) return -(new Real(this.numerator / BigInteger.Pow(10, this.Log10Denominator), 0));
                    else return -(new Real(this.numerator / BigInteger.Pow(10, this.Log10Denominator) + 1, 0));
                } 
            }
        }
        public Real PartOfDecimal
        {
            get
            {
                if (this.IsPositive) return new Real(this.numerator - this.PartOfInt.numerator * BigInteger.Pow(10,this.Log10Denominator), this.Log10Denominator);
                else return new Real(this.PartOfInt.numerator * BigInteger.Pow(10, this.Log10Denominator) - this.numerator, this.Log10Denominator);
            }
        }

        public Real(string number)
        {
            this._isPositive = true;
            if (number[0] == '-') { this._isPositive = false; number = number.TrimStart('-'); }
            string[] num = number.Split(',');
            if (num.Length == 1)
            {
                if (number != "")
                {
                    if (!BigInteger.TryParse(number, out this.numerator))
                    {
                        throw new Exception("Düzgün sayı gir amk bebesi");
                    }
                }
                else { this.numerator = 0; }
                this.Log10Denominator = 0;
            }
            else
            {
                num[1] = num[1].TrimEnd('0');
                if (num[1].Length > Real._sens)
                {
                    num[1] = num[1].Substring(0, Real._sens);
                }
                if (!BigInteger.TryParse(num[0] + num[1], out this.numerator))
                {
                    throw new Exception("Düzgün sayı gir amk bebesi");
                }
                this.Log10Denominator = num[1].Length;
            }
        }
        public Real(BigInteger integer, int Log)
        {
            this._isPositive = true;
            if (integer.Sign == -1) { integer *= -1; this._isPositive = false; }
            if (integer.IsZero) { this.numerator = 0; this.Log10Denominator = 0; this._isPositive = true; }
            else
            {
                if (Log > Real._sens)
                {
                    integer = integer / BigInteger.Pow(10, Log - Real._sens);
                    Log = Real._sens;
                }
                BigInteger I1 , I2; int logneg = 0;
                for (int i = 0; true; i++)
                {
                    I1 = (integer / 10); 
                    I2= I1 * 10;
                    if (I2 == integer)
                    {
                        integer = I1;
                    }
                    else { logneg = i; break; }
                }
                if (logneg <= Log)
                {
                    Log -= logneg;
                }
                else
                {
                    integer *= BigInteger.Pow(10, logneg - Log);
                    Log = 0;
                }
                this.numerator = integer;
                this.Log10Denominator = Log;
            }
        }
        
        public static Real operator +(Real R1, Real R2)
        {
            if (R1.IsPositive && R2.IsPositive)
            {
                if (R1.Log10Denominator > R2.Log10Denominator)
                {
                    BigInteger B = R2.numerator * BigInteger.Pow(10, R1.Log10Denominator - R2.Log10Denominator);
                    B += R1.numerator;
                    return new Real(B, R1.Log10Denominator);
                }
                else if (R1.Log10Denominator < R2.Log10Denominator)
                {
                    BigInteger B = R1.numerator * BigInteger.Pow(10, R2.Log10Denominator - R1.Log10Denominator);
                    B += R2.numerator;
                    return new Real(B, R2.Log10Denominator);
                }
                else
                {
                    return new Real(R1.numerator + R2.numerator, R1.Log10Denominator);
                }
            }
            else if (R1.IsPositive && !R2.IsPositive) { return R1 - (-R2); }
            else if (!R1.IsPositive && R2.IsPositive) { return R2 - (-R1); }
            else { return -((-R1) + (-R2)); }
            

        }
        public static Real operator -(Real R1, Real R2)
        {
            if (R1.IsPositive && R2.IsPositive)
            {
                if (R1.Log10Denominator > R2.Log10Denominator)
                {
                    BigInteger B = R2.numerator * BigInteger.Pow(10, R1.Log10Denominator - R2.Log10Denominator);
                    B -= R1.numerator;
                    return new Real(-B, R1.Log10Denominator);
                }
                else if (R1.Log10Denominator < R2.Log10Denominator)
                {
                    BigInteger B = R1.numerator * BigInteger.Pow(10, R2.Log10Denominator - R1.Log10Denominator);
                    B -= R2.numerator;
                    return new Real(B, R2.Log10Denominator);
                }
                else
                {
                    return new Real(R1.numerator - R2.numerator, R1.Log10Denominator);
                }
            }
            else if (R1.IsPositive && !R2.IsPositive) { return R1 + (-R2); }
            else if (!R1.IsPositive && R2.IsPositive) { return -(R2 - R1); }
            else { return (-R2) - (-R1); }
        }
        public static Real operator *(Real R1, Real R2)
        {
            if (R1._isPositive == R2._isPositive) 
                return new Real(R1.numerator * R2.numerator, R1.Log10Denominator + R2.Log10Denominator);
            else
                return new Real(-R1.numerator * R2.numerator, R1.Log10Denominator + R2.Log10Denominator);
        }
        public static Real operator /(Real R1, Real R2)
        {
            BigInteger Rem, numerator = BigInteger.DivRem(R1.numerator, R2.numerator, out Rem);
            for (int i = 0; i < Real._sens; i++)
            {
                numerator *= 10;
                Rem *= 10;
                numerator += BigInteger.DivRem(Rem, R2.numerator, out Rem);
            }
            return new Real(numerator, Real._sens + (R1.Log10Denominator - R2.Log10Denominator));
        }

        public static implicit operator Real(BigInteger B)
        {
            return new Real(B, 0);
        }

        public void ToFraction(out Real numerator, out Real denominator)
        {
            BigInteger denom = BigInteger.Pow(10, this.Log10Denominator);
            BigInteger GCD = BigInteger.GreatestCommonDivisor(this.numerator, denom);
            numerator = new Real(this.numerator / GCD, 0);
            denominator = new Real(denom / GCD, 0);
        }

        public static Real Ceiling(Real real)
        {
            if (real.Log10Denominator == 0) { return real; }
            else
            {
                if (real.IsPositive)
                {
                    BigInteger B = real.numerator / BigInteger.Pow(10, real.Log10Denominator);
                    return new Real(B + 1, 0);
                }
                else
                {
                    BigInteger B = real.numerator / BigInteger.Pow(10, real.Log10Denominator);
                    return new Real(-B, 0);
                }
            }
        }
        public static Real Floor(Real real)
        {
            if (real.Log10Denominator == 0) { return real; }
            else
            {
                if (real.IsPositive)
                {
                    BigInteger B = real.numerator / BigInteger.Pow(10, real.Log10Denominator);
                    return new Real(B, 0);
                }
                else
                {
                    BigInteger B = real.numerator / BigInteger.Pow(10, real.Log10Denominator);
                    return new Real(-B - 1, 0);
                }
            }
        }
        public static Real Round(Real real)
        {
            BigInteger Int = real.numerator / BigInteger.Pow(10, real.Log10Denominator);
            BigInteger T = Int * BigInteger.Pow(10, real.Log10Denominator);
            BigInteger five = 5 * BigInteger.Pow(10, real.Log10Denominator - 1);
            T = real.numerator - T;
            if(real.IsPositive)
            return new Real (Int + (T / five), 0);
            else return new Real(-Int - (T / five), 0);
        }
        public static Real Pow(Real real, int exponent)
        {
            return new Real(BigInteger.Pow(real.numerator, exponent), real.Log10Denominator * exponent);
        }
        public static Real Factorial(uint factorial)
        {
            BigInteger I = 1;
            for (uint i = 2; i <= factorial; i++)
            {
                I *= i;
            }
            return new Real(I, 0);
        }
        public static Real Factorial(int factorial)
        {
            BigInteger I = 1;
            for (int i = 2; i <= factorial; i++)
            {
                I *= i;
            }
            return new Real(I, 0);
        }

        public static Real Exp(Real exponent)
        {
            Real R = new Real("1") + exponent;
            for (int i = 2; i < 52; i++)
            {
                R += Real.Pow(exponent, i) / Real.Factorial(i);
            }
            return R;
        }

        public static Real operator -(Real R)
        {
            R._isPositive = !R._isPositive;
            return R;
        }
        public override string ToString()
        {
            if (this.numerator.IsZero) return "0";
            BigInteger B = BigInteger.Pow(10, this.Log10Denominator);
            BigInteger Int = this.numerator / B;
            if (Int != 0)
            {
                BigInteger Dec = this.numerator - Int * B;
                if(this.IsPositive)
                {
                    string zerodec = "";
                    for (int i = 0; i < this.Log10Denominator - Dec.ToString().Length; i++)
                    {
                        zerodec += 0;
                    }
                        if (Dec != 0)
                            return Int.ToString() + "," + zerodec +Dec.ToString();
                        else return Int.ToString();
                }
                else
                {
                    string zerodec = "";
                    for (int i = 0; i < this.Log10Denominator - Dec.ToString().Length; i++)
                    {
                        zerodec += 0;
                    }
                    if (Dec != 0)
                        return "-" + Int.ToString() + "," + zerodec + Dec.ToString();
                    else return "-" + Int.ToString();
                }

            }
            else
            {
                int zerotime = this.Log10Denominator - this.numerator.ToString().Length;
                string zeros = "0,";
                for (int i = 0; i < zerotime; i++)
                {
                    zeros += "0";
                }
                if (this.IsPositive) 
                    return zeros + this.numerator.ToString();
                else
                    return "-" + zeros + this.numerator.ToString();
            }
            
        }
        public int CompareTo(Real other)
        {
            if (this.IsPositive && !other.IsPositive) return 1;
            else if (!this.IsPositive && other.IsPositive) return -1;
            else if (this.IsPositive && other.IsPositive)
            {
                if (this.PartOfInt.numerator > other.PartOfInt.numerator) return 1;
                else if (this.PartOfInt.numerator < other.PartOfInt.numerator) return -1;
                else
                {
                    BigInteger thisDec, otherDec;
                    if (this.Log10Denominator > other.Log10Denominator)
                    {
                        otherDec = other.PartOfDecimal.numerator * BigInteger.Pow(10, this.Log10Denominator - other.Log10Denominator);
                        thisDec = this.PartOfDecimal.numerator;
                    }
                    else if (this.Log10Denominator < other.Log10Denominator)
                    {
                        thisDec = this.PartOfDecimal.numerator * BigInteger.Pow(10, other.Log10Denominator - this.Log10Denominator);
                        otherDec = other.PartOfDecimal.numerator;
                    }
                    else
                    {
                        thisDec = this.PartOfDecimal.numerator;
                        otherDec = other.PartOfDecimal.numerator;
                    }

                    if (thisDec > otherDec) return 1;
                    else if (thisDec < otherDec) return -1;
                    else return 0;
                }
            }
            else
            {
                if (this.PartOfInt.numerator > other.PartOfInt.numerator) return -1;
                else if (this.PartOfInt.numerator < other.PartOfInt.numerator) return 1;
                else
                {
                    BigInteger thisDec, otherDec;
                    if (this.Log10Denominator > other.Log10Denominator)
                    {
                        otherDec = other.PartOfDecimal.numerator * BigInteger.Pow(10, this.Log10Denominator - other.Log10Denominator);
                        thisDec = this.PartOfDecimal.numerator;
                    }
                    else if (this.Log10Denominator < other.Log10Denominator)
                    {
                        thisDec = this.PartOfDecimal.numerator * BigInteger.Pow(10, other.Log10Denominator - this.Log10Denominator);
                        otherDec = other.PartOfDecimal.numerator;
                    }
                    else
                    {
                        thisDec = this.PartOfDecimal.numerator;
                        otherDec = other.PartOfDecimal.numerator;
                    }

                    if (thisDec > otherDec) return 1;
                    else if (thisDec < otherDec) return -1;
                    else return 0;
                }
            }
        }
    }
}
