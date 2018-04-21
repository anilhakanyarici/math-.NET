using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class Complex : AllowToGenerics<Vector, Complex>, AllowToGenerics<Matrix, Complex>
    {

        #region: Properties

        private double _real;
        private double _imaginary;
        public double Real { get { return this._real; } set { this._real = value; } }
        public double Imaginary { get { return this._imaginary; } set { this._imaginary = value; } }
        public static Complex i { get { return new Complex(0, 1); } }
        public static Complex One { get { return new Complex(1, 0); } }
        public static Complex Zero { get { return new Complex(0, 0); } }
        public static Complex NaN { get { return new Complex(double.NaN, double.NaN); } }
        public static Complex Random 
        { 
            get 
            {
                Random rnd = new Random();
                double r1 = (double)rnd.Next(1000) / 1000;
                double r2 = (double)rnd.Next(1000) / 1000;
                return new Complex(r1, r2);
            } 
        }
        public double Magnitude { get { return Math.Sqrt(this.Imaginary * this.Imaginary + this.Real * this.Real); } }
        public Degrees Phase 
        { 
            get 
            {
                if (this.Real < 0) 
                {
                    Degrees T = new Degrees(180);
                    return T + new Degrees(Math.Atan(this.Imaginary / this.Real) * 180 / Math.PI);
                }
                else 
                return new Degrees(Math.Atan(this.Imaginary / this.Real) * 180 / Math.PI); 
            } 
        }
        public Complex CiS { get { return this.GetCiS(); } }
        

        #endregion

        #region: Constructors

        public Complex(double real, double imaginary)
        {
            double intR = Math.Round(real);
            double intI = Math.Round(imaginary);
            if (real < intR + Math.Pow(10, -10) && real > intR - Math.Pow(10, -10)) this.Real = intR;
            else this.Real = real;
            if (imaginary < intI + Math.Pow(10, -10) && imaginary > intI - Math.Pow(10, -10)) this.Imaginary = intI;
            else this.Imaginary = imaginary;

        }
        public Complex(double magnitude, Degrees phase)
        {
            if (magnitude == 0)
            {
                this.Real = 0; this.Imaginary = 0;
            }
            else
            {
                this.Real = (magnitude * Complex.Exp(phase.Radian * Complex.i)).Real;
                this.Imaginary = (magnitude * Complex.Exp(phase.Radian * Complex.i)).Imaginary;
                double intR = Math.Round(this.Real);
                double intI = Math.Round(this.Imaginary);
                if (this.Real < intR + Math.Pow(10, -10) && this.Real > intR - Math.Pow(10, -10)) this.Real = intR;
                if (this.Imaginary < intI + Math.Pow(10, -10) && this.Imaginary > intI - Math.Pow(10, -10)) this.Imaginary = intI;
            }
        }

        #endregion

        #region: Static Function

        public static Complex Exp(Complex C)
        {
            return (new Complex(Math.Exp(C.Real) * Math.Cos(C.Imaginary), Math.Exp(C.Real) * Math.Sin(C.Imaginary)));
        }
        public static Complex Ln(Complex Z)
        {
            return new Complex(Math.Log(Z.Magnitude, Math.E), Z.Phase.Radian);
        }
        public static Complex Log(double Base, Complex Z)
        {
            return Complex.Ln(Z) / Math.Log(Base, Math.E);
        }
        public static Complex Sqrt(Complex Z)
        {
            return Z.Pow(0.5);
        }
        public static Complex Pow(Complex Z, double P)
        {
            return Z.Pow(P);
        }
        public static Complex Pow(Complex Z1, Complex Z2)
        {
            return Z1.Pow(Z2);
        }
        public static Complex Pow(double D, Complex P)
        {
            Complex Z = new Complex(D, 0);
            return Z.Pow(P);
        }
        
        public static Complex Sin(Complex Z)
        {
            return (0.5 / i) * (Complex.Exp(Complex.i * Z) - Complex.Exp((-1) * Complex.i * Z));
        }
        public static Complex Cos(Complex Z)
        {
            return 0.5 * (Complex.Exp(Complex.i * Z) + Complex.Exp((-1) * Complex.i * Z));
        }
        public static Complex Tan(Complex Z)
        {
            return Complex.Sin(Z) / Complex.Cos(Z);
        }
        public static Complex Cot(Complex Z)
        {
            return Complex.Cos(Z) / Complex.Sin(Z);
        }
        public static Complex Sec(Complex Z)
        {
            return 1.0 / Complex.Cos(Z);
        }
        public static Complex Csc(Complex Z)
        {
            return 1.0 / Complex.Sin(Z);
        }

        public static Complex Sinh(Complex Z)
        {
            return (0.5) * (Complex.Exp(Z) - Complex.Exp((-1) * Z));
        }
        public static Complex Cosh(Complex Z)
        {
            return 0.5 * (Complex.Exp(Z) + Complex.Exp((-1) * Z));
        }
        public static Complex Tanh(Complex Z)
        {
            return Complex.Sinh(Z) / Complex.Cosh(Z);
        }
        public static Complex Coth(Complex Z)
        {
            return Complex.Cosh(Z) / Complex.Sinh(Z);
        }
        public static Complex Sech(Complex Z)
        {
            return 1 / Complex.Cosh(Z);
        }
        public static Complex Csch(Complex Z)
        {
            return 1 / Complex.Sinh(Z);
        }

        public static Complex Acos(Complex Z)
        {
            Complex i = Complex.i;
            Complex innerLog = i * Z + Complex.Sqrt(1 - Z * Z);
            return Math.PI / 2 + i * Complex.Ln(innerLog);
        }
        public static Complex Asin(Complex Z)
        {
            Complex i = Complex.i;
            Complex innerLog = i * Z + Complex.Sqrt(1 - Z * Z);
            return -1 * i * Complex.Ln(innerLog);
        }
        public static Complex Atan(Complex Z)
        {
            return (0.5) * Complex.i * (Complex.Ln(1 - Complex.i * Z) - Complex.Ln(1 + Complex.i * Z)); 
        }
        public static Complex Acot(Complex Z)
        {
            return (-0.5) * Complex.i * ( Complex.Ln(Complex.i + Z) - Complex.Ln(Z - Complex.i)); 
        }
        public static Complex Asec(Complex Z)
        {
            return Complex.Acos(1.0 / Z);
        }
        public static Complex Acsc(Complex Z)
        {
            return Complex.Asin(1.0 / Z);
        }

        public static Complex Asinh(Complex Z)
        {
            return Complex.Ln(Z + Complex.Sqrt(1 + Z * Z));
        }
        public static Complex Acosh(Complex Z)
        {
            return Complex.Ln(Z + Complex.Sqrt(Z * Z - 1));
        }
        public static Complex Atanh(Complex Z)
        {
            return Complex.Ln((1 + Z) / (1 - Z)) / 2.0;
        }
        public static Complex Acoth(Complex Z)
        {
            return Complex.Atanh(1.0 / Z);
        }
        public static Complex Asech(Complex Z)
        {
            return Complex.Acosh(1.0 / Z);
        }
        public static Complex Acsch(Complex Z)
        {
            return Complex.Asinh(1.0 / Z);
        }

        #endregion

        #region: Methods Of Operations

        public Complex Reciprocal() 
        {
            return (1 / this) / this.Magnitude; 
        }
        public Complex Conjugate()
        {
            return new Complex(this.Real, -this.Imaginary);
        }
        public Complex Pow(double P)
        {
            double newMagnitude = Math.Pow(this.Magnitude, P);
            Degrees newPhase = this.Phase * P;
            return new Complex(newMagnitude, newPhase);
        }
        public Complex Pow(Complex P)
        {
            Complex Z = new Complex(Math.Log(this.Magnitude, Math.E) * P.Real - this.Phase.Radian * P.Imaginary, Math.Log(this.Magnitude, Math.E) * P.Imaginary + this.Phase.Radian * P.Real);
            return Complex.Exp(Z);
        }
        private Complex GetCiS()
        {
            return this / this.Magnitude;
        }
        private Complex Add(Complex Z)
        {
            return this + Z;
        }
        private Complex Sub(Complex Z)
        {
            return this - Z;
        }
        private Complex Mul(Complex Z)
        {
            return this * Z;
        }
        private Complex Div(Complex Z)
        {
            return this / Z;
        }

        #endregion

        #region: Converter Methods

        public override string ToString()
        {
            if (this.Real != 0 && this.Imaginary > 0 )
                return "<" + this.Real + "+" + this.Imaginary + "i>";
            else if (this.Real != 0 && this.Imaginary == 0)
                return "<" + this.Real + ">";
            else if (this.Real == 0 && this.Imaginary == 0 )
                return "<" + "0" + ">";
            else if (this.Real == 0 && this.Imaginary > 0)
                return "<" + "+" + this.Imaginary + "i>";
            else if (this.Real == 0 && this.Imaginary < 0 )
                return "<" + this.Imaginary + "i>";
            else return "<" + this.Real + this.Imaginary + "i>";

        }

        #endregion

        #region: Operator Overloads

        public static Complex operator +(Complex C1, Complex C2)
        {
            return new Complex(C1.Real + C2.Real, C1.Imaginary + C2.Imaginary);
        }
        public static Complex operator +(Complex C, double D)
        {
            return new Complex(C.Real + D, C.Imaginary);
        }
        public static Complex operator +(double D, Complex C)
        {
            return new Complex(C.Real + D, C.Imaginary);
        }

        public static Complex operator -(Complex C1, Complex C2)
        {
            return new Complex(C1.Real - C2.Real, C1.Imaginary - C2.Imaginary);
        }
        public static Complex operator -(Complex C, double D)
        {
            return new Complex(C.Real - D, C.Imaginary);
        }
        public static Complex operator -(double D ,Complex C)
        {
            return new Complex(D - C.Real, - C.Imaginary);
        }

        public static Complex operator *(Complex C1, Complex C2)
        {
            return new Complex(C1.Real * C2.Real - C1.Imaginary * C2.Imaginary, C1.Real * C2.Imaginary + C1.Imaginary * C2.Real);
        }
        public static Complex operator *(Complex C, double D)
        {
            return new Complex(C.Real * D, C.Imaginary * D);
        }
        public static Complex operator *(double D, Complex C)
        {
            return new Complex(C.Real * D, C.Imaginary * D);
        }

        public static Complex operator /(Complex C1, Complex C2)
        {
            double Divider = C2.Real * C2.Real + C2.Imaginary * C2.Imaginary;
            double X = C1.Real * C2.Real + C1.Imaginary * C2.Imaginary;
            double Y = C1.Imaginary * C2.Real - C1.Real * C2.Imaginary;
            return new Complex(X / Divider, Y / Divider);
        }
        public static Complex operator /(Complex C, double D)
        {
            return new Complex(C.Real / D, C.Imaginary / D);
        }
        public static Complex operator /(double D, Complex C)
        {
            double Divider = C.Real * C.Real + C.Imaginary * C.Imaginary;
            double X = C.Real * D;
            double Y = - C.Imaginary * D;
            return new Complex(X / Divider, Y / Divider);
        }

        public static implicit operator Complex(double D)
        {
            return new Complex(D, 0);
        }

        public static bool operator ==(Complex C1, Complex C2)
        {
            return (C1.Real == C2.Real) && (C1.Imaginary == C2.Imaginary);
        }
        public static bool operator !=(Complex C1, Complex C2)
        {
            return !(C1 == C2);
        }
        public static bool operator ==(Complex Z, double D)
        {
            return (Z.Real == D) && (Z.Imaginary == 0);
        }
        public static bool operator !=(Complex Z, double D)
        {
            return !(Z == D);
        }
        public static bool operator ==(double D, Complex Z)
        {
            return Z == D;
        }
        public static bool operator !=(double D, Complex Z)
        {
            return !(Z != D);
        }

        public static bool operator < (Complex C1, Complex C2)
        {
            return C1.CompareTo(C2) == -1;
        }
        public static bool operator >(Complex C1, Complex C2)
        {
            return C1.CompareTo(C2) == 1;
        }
        public static bool operator >=(Complex C1, Complex C2)
        {
            return C1.CompareTo(C2) == 1 || C1 == C2;
        }
        public static bool operator <=(Complex C1, Complex C2)
        {
            return C1.CompareTo(C2) == -1 || C1 == C2;
        }

        #endregion

        #region: C#

        public int CompareTo(Complex other)
        {
            if (this.Real < other.Real) return -1;
            else if (this.Real > other.Real) return 1;
            else
            {
                if (this.Imaginary < other.Imaginary) return -1;
                else if (this.Imaginary > other.Imaginary) return 1;
                else return 0;
            }
        }
        public override bool Equals(object complex)
        {
            Complex Z = (Complex)complex;
            if (this.Imaginary == Z.Imaginary && this.Real == Z.Real) return true;
            else return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        Complex AllowToGenerics<Vector, Complex>.Add(Complex operand)
        {
            return this.Add(operand);
        }
        Complex AllowToGenerics<Vector, Complex>.Sub(Complex operand)
        {
            return this.Sub(operand);
        }
        Complex AllowToGenerics<Vector, Complex>.Mul(Complex operand)
        {
            return this.Mul(operand);
        }
        Complex AllowToGenerics<Vector, Complex>.Div(Complex operand)
        {
            return this.Div(operand);
        }
        Complex AllowToGenerics<Matrix, Complex>.Add(Complex operand)
        {
            return this.Add(operand);
        }
        Complex AllowToGenerics<Matrix, Complex>.Sub(Complex operand)
        {
            return this.Sub(operand);
        }
        Complex AllowToGenerics<Matrix, Complex>.Mul(Complex operand)
        {
            return this.Mul(operand);
        }
        Complex AllowToGenerics<Matrix, Complex>.Div(Complex operand)
        {
            return this.Div(operand);
        }
        Complex IDivisible<Complex>.Add(Complex operand)
        {
            return this.Add(operand);
        }
        Complex IDivisible<Complex>.Sub(Complex operand)
        {
            return this.Sub(operand);
        }
        Complex IDivisible<Complex>.Mul(Complex operand)
        {
            return this.Mul(operand);
        }
        Complex IDivisible<Complex>.Div(Complex operand)
        {
            return this.Div(operand);
        }
        Complex IOperable<Complex>.Add(Complex operand)
        {
            return this.Add(operand);
        }
        Complex IOperable<Complex>.Sub(Complex operand)
        {
            return this.Sub(operand);
        }
        Complex IOperable<Complex>.Mul(Complex operand)
        {
            return this.Mul(operand);
        }
        
    }
}
