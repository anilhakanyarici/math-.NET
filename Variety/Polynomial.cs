using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    class  Polynomial : AllowToGenerics<Vector, Polynomial>, AllowToGenerics<Matrix, Polynomial>
    {
        private class Term
        {
            private double _coefficient;
            public double Coefficient { get { return this._coefficient; } }
            public bool IsZero { get { return this._coefficient == 0; } }
            private int _force;
            public int Force { get { return this._force; } }

            public Term(double coefficient, int force)
            {
                this._coefficient = coefficient;
                this._force = force;
                if (force < 0) { this._coefficient = 0; this._force = 0; }
            }
            public Term(string term)
            {
                string[] components = term.Split('x');

                if (components[0] == "") this._coefficient = 1;
                else if (components[0] == "-") this._coefficient = -1;
                else this._coefficient = double.Parse(components[0]);                
                try
                {
                    if (components[1] == "") this._force = 1;
                    else this._force = int.Parse(components[1]);
                }
                catch (IndexOutOfRangeException)
                {
                    this._force = 0;
                }

                if (this._force < 0) { this._coefficient = 0; this._force = 0; }
            }
            public override string ToString()
            {
                if (this._coefficient == 1)
                {
                    if (this._force == 0) return "1";
                    else if (this._force == 1) return "x";
                    else return "x^" + this._force.ToString();
                }
                else if (this._coefficient == -1)
                {
                    if (this._force == 0) return "-1";
                    else if (this._force == 1) return "-x";
                    else return "-x^" + this._force.ToString();
                }
                else if (this._coefficient == 0) return "";
                else
                {
                    if (this._force == 0) return this._coefficient.ToString();
                    else if (this._force == 1) return this._coefficient.ToString() + "x";
                    else return this._coefficient.ToString() + "x^" + this._force.ToString();
                }
            }


            public Term MultipliedBy(double rational)
            {
                return new Term(this._coefficient * rational, this._force);
            }
            public Term MultipliedBy(Term otherPolynomialTerm)
            {
                double R = this._coefficient * otherPolynomialTerm._coefficient;
                int F = this._force + otherPolynomialTerm._force;
                return new Term(R, F);
            }
            public Term DividedBy(double rational)
            {
                return new Term(this._coefficient / rational, this._force);
            }
            public Term DividedBy(Term otherPolynomialTerm)
            {
                if (otherPolynomialTerm._coefficient == 0) throw new Exception("0'a bölme yapılamaz.");
                double R = this._coefficient / otherPolynomialTerm._coefficient;
                int F = this._force - otherPolynomialTerm._force;
                return new Term(R, F);
            }

            public Polynomial ReplaceX(Polynomial polynom)
            {
                if (this.IsZero || polynom.IsZero) return Polynomial.Zero;
                Polynomial P = Polynomial.Pow(polynom, this._force);
                P = P * this._coefficient;
                return P;
            }
            public double TermValue(double x)
            {
                return this._coefficient * Math.Pow(x, this._force);
            }
            public Term Derivative()
            {
                return new Term(this._coefficient * this._force, this._force - 1);
            }
            public double Derivative(double apsis)
            {
                return this.Derivative().TermValue(apsis);
            }
            public Term Integral()
            {
                return new Term(this._coefficient / (this._force + 1), this._force + 1);
            }
            public double Integral(double region1, double region2)
            {
                Term STerm = this.Integral();
                return STerm.TermValue(region2) - STerm.TermValue(region1);
            }
            public void IncreaseForce(int i) { this._force += i; }
        }

        private Term[] terms;
        public int Degree { get { return this.terms[0].Force; } }
        public double GetCoefficient(int force)
        {
            for (int i = 0; i < this.terms.Length; i++)
            {
                if (this.terms[i].Force == force) return this.terms[i].Coefficient;
            }
            return 0;
        }
        public bool IsZero { get { return this.terms.Length == 0; } }
        public static Polynomial Zero { get { return new Polynomial(0); } }

        public Polynomial(params double[] coefficients)
        {
            List<int> forces = new List<int>(); List<double> newCoE = new List<double>();

            for (int i = 0; i < coefficients.Length; i++)
            {
                if (!(coefficients[coefficients.Length - 1 - i] == 0)) { forces.Add(i); newCoE.Add(coefficients[coefficients.Length - 1 - i]); }
            }

            this.terms = new Term[forces.Count];

            for (int i = 0; i < this.terms.Length; i++)
            {
                this.terms[this.terms.Length - 1 - i] = new Term(newCoE[i], forces[i]);
            }
        }
        private Polynomial(params Term[] terms)
        {
            terms = Polynomial.Simplify(terms);
            Term temp;
            for (int i = 0; i < terms.Length - 1; i++)
            {
                for (int j = 1; j < terms.Length - i; j++)
                {
                    if (terms[j].Force > terms[j - 1].Force)
                    {
                        temp = terms[j - 1];
                        terms[j - 1] = terms[j];
                        terms[j] = temp;
                    }
                }
            }
            int length = terms.Length;
            for (int i = 0; i < terms.Length; i++)
            {
                if (terms[i].IsZero) { length--; }
            }

            this.terms = new Term[length]; int loops = 0;
            for (int i = 0; i < terms.Length; i++)
            {
                if (!terms[i].IsZero)
                {
                    this.terms[loops] = terms[i];
                    loops++;
                }
            }
        }
        public Polynomial(params string[] termsString)
        {
            Term[] terms = new Term[termsString.Length];
            for (int i = 0; i < terms.Length; i++)
            {
                terms[i] = new Term(termsString[i]);
            }
            terms = Polynomial.Simplify(terms);
            Term temp;
            for (int i = 0; i < terms.Length - 1; i++)
            {
                for (int j = 1; j < terms.Length - i; j++)
                {
                    if (terms[j].Force > terms[j - 1].Force)
                    {
                        temp = terms[j - 1];
                        terms[j - 1] = terms[j];
                        terms[j] = temp;
                    }
                }
            }
            int length = terms.Length;
            for (int i = 0; i < terms.Length; i++)
            {
                if (terms[i].IsZero) { length--; }
            }

            this.terms = new Term[length]; int loops = 0;
            for (int i = 0; i < terms.Length; i++)
            {
                if (!terms[i].IsZero)
                {
                    this.terms[loops] = terms[i];
                    loops++;
                }
            }
        }
        private Polynomial(Term term)
        {
            this.terms = new Term[1] { term };
        }

        public static Polynomial operator -(Polynomial P)
        {
            return P.Mul(-1);
        }

        public static Polynomial operator +(Polynomial P1, Polynomial P2)
        {
            return P1.Add(P2);
        }
        public static Polynomial operator -(Polynomial P1, Polynomial P2)
        {
            return P1.Sub(P2);
        }

        public static Polynomial operator *(Polynomial P1, Polynomial P2)
        {
            return P1.Mul(P2);
        }
        public static Polynomial operator *(Polynomial P, double d)
        {
            return P.Mul(d);
        }
        public static Polynomial operator *(double d, Polynomial P)
        {
            return P.Mul(d);
        }

        public static Polynomial operator /(Polynomial P1, Polynomial P2)
        {
            return P1.Div(P2);
        }
        public static Polynomial operator %(Polynomial P1, Polynomial P2)
        {
            return P1.Rem(P2);
        }

        public static implicit operator Polynomial(double d)
        {
            return new Polynomial(d.ToString());
        }


        public double PolyValue(double x)
        {
            if (this.IsZero) return 0;
            double s = 0;
            for (int i = 0; i < this.terms.Length; i++)
            {
                s += this.terms[i].TermValue(x);
            }
            return s;
        }
        public Polynomial Derivative()
        {
            Term[] dTerm = new Term[this.terms.Length];
            for (int i = 0; i < this.terms.Length; i++)
            {
                dTerm[i] = this.terms[i].Derivative();
            }
            return new Polynomial(dTerm);
        }
        public double Derivative(double apsis) 
        {
            return this.Derivative().PolyValue(apsis);
        }
        public Polynomial Integral()
        {
            Term[] STerm = new Term[this.terms.Length];
            for (int i = 0; i < this.terms.Length; i++)
            {
                STerm[i] = this.terms[i].Integral();
            }
            return new Polynomial(STerm);
        }
        public Polynomial Integral(double constant)
        {
            Term[] STerm = new Term[this.terms.Length + 1];
            for (int i = 0; i < this.terms.Length; i++)
            {
                STerm[i] = this.terms[i].Integral();
            }
            STerm[this.terms.Length] = new Term(constant, 0);
            return new Polynomial(STerm);
        }
        public double Integral(double region1, double region2)
        {
            Polynomial SPoly = this.Integral();
            return SPoly.PolyValue(region2) - SPoly.PolyValue(region1);
        }
        public Polynomial ReplaceX(Polynomial polynom)
        {
            if (this.IsZero || polynom.IsZero) return Polynomial.Zero;
            
            Polynomial P = this.terms[0].ReplaceX(polynom);
            for (int i = 1; i < this.terms.Length; i++)
            {
                P = P.Add(this.terms[i].ReplaceX(polynom));
            }
            return P;
            
        }
        public static Polynomial Pow(Polynomial polynom, int p)
        {
            if (p == 0) return new Polynomial("1");
            else if (p <= 2) { if (p == 1) return polynom; else return polynom.Mul(polynom); }
            else
            {
                bool IsEven = (p & 1) == 0;
                if (IsEven)
                {
                    return Polynomial.Pow(Polynomial.Pow(polynom, p / 2), 2);
                }
                else
                {
                    return Polynomial.Pow(Polynomial.Pow(polynom, p / 2), 2).Mul(polynom);
                }
            }
        }

        private Polynomial Add(Polynomial otherPolynomial)
        {
            if (this.IsZero && otherPolynomial.IsZero) return Polynomial.Zero;
            else if (this.IsZero) return otherPolynomial;
            else if (otherPolynomial.IsZero) return this;
            else
            {
                double[] R;
                if (this.Degree > otherPolynomial.Degree) R = new double[this.Degree + 1];
                else R = new double[otherPolynomial.Degree + 1];
                for (int i = 0; i < R.Length; i++)
                {
                    R[i] = this.GetCoefficient(R.Length - 1 - i) + otherPolynomial.GetCoefficient(R.Length - 1 - i);
                }
                return new Polynomial(R);
            }
        }
        private Polynomial Sub(Polynomial otherPolynomial)
        {
            if (this.IsZero && otherPolynomial.IsZero) return Polynomial.Zero;
            else if (this.IsZero) return -otherPolynomial;
            else if (otherPolynomial.IsZero) return this;
            else
            {
                double[] R;
                if (this.Degree > otherPolynomial.Degree) R = new double[this.Degree + 1];
                else R = new double[otherPolynomial.Degree + 1];
                for (int i = 0; i < R.Length; i++)
                {
                    R[i] = this.GetCoefficient(R.Length - 1 - i) - otherPolynomial.GetCoefficient(R.Length - 1 - i);
                }
                return new Polynomial(R);
            }


        }
        private Polynomial Mul(double rational)
        {
            if (this.IsZero || rational == 0) return Polynomial.Zero;
            else
            {
                Term[] newTerm = new Term[this.terms.Length];
                for (int i = 0; i < newTerm.Length; i++)
                {
                    newTerm[i] = this.terms[i].MultipliedBy(rational);
                }
                return new Polynomial(newTerm);
            }

        }
        private Polynomial Mul(Term term)
        {
            if (this.IsZero || term.IsZero) return Polynomial.Zero;
            else
            {
                double[] R = new double[this.Degree + 1];
                for (int i = 0; i < R.Length; i++)
                {
                    R[i] = this.GetCoefficient(R.Length - 1 - i) * term.Coefficient;
                }
                Polynomial newP = new Polynomial(R);
                newP.IncreaseForces(term.Force);
                return newP;
            }
        }
        private Polynomial Mul(Polynomial otherPolynomial)
        {
            if (this.IsZero || otherPolynomial.IsZero) return Polynomial.Zero;
            else
            {
                Polynomial[] P = new Polynomial[otherPolynomial.terms.Length];
                for (int i = 0; i < P.Length; i++)
                {
                    P[i] = this.Mul(otherPolynomial.terms[i]);
                }
                for (int i = 1; i < P.Length; i++)
                {
                    P[0] = P[0].Add(P[i]);
                }
                return P[0];
            }

        }
        private Polynomial Div(Polynomial otherPolynomial)
        {
            if (otherPolynomial.IsZero) throw new Exception("0'a bölünmez.");
            else if (this.IsZero) return Polynomial.Zero;
            else
            {
                if (this.Degree < otherPolynomial.Degree) return Polynomial.Zero;
                else
                {
                    Polynomial P = this.Clone(); Term[] Tlist = new Term[this.Degree - otherPolynomial.Degree + 1];
                    for (int i = 0; i < this.Degree - otherPolynomial.Degree + 1; i++)
                    {
                        Term T = P.terms[0].DividedBy(otherPolynomial.terms[0]);
                        Tlist[i] = T;
                        P = P.Sub(otherPolynomial.Mul(T));
                    }
                    return new Polynomial(Tlist);
                }

            }
        }
        public Polynomial DivRem(Polynomial otherPolynomial, out Polynomial Rem)
        {
            if (otherPolynomial.IsZero) throw new Exception("0'a bölünmez.");
            else if (this.IsZero) { Rem = Polynomial.Zero; return Polynomial.Zero; }
            else
            {
                if (this.Degree < otherPolynomial.Degree) { Rem = this; return Polynomial.Zero; }
                else
                {
                    Polynomial P = this.Clone(); Term[] Tlist = new Term[this.Degree - otherPolynomial.Degree + 1];
                    for (int i = 0; i < this.Degree - otherPolynomial.Degree + 1; i++)
                    {
                        Term T = P.terms[0].DividedBy(otherPolynomial.terms[0]);
                        Tlist[i] = T;
                        P = P.Sub(otherPolynomial.Mul(T));
                    }
                    Rem = P;
                    return new Polynomial(Tlist);
                }

            }
        }
        private Polynomial Rem(Polynomial otherPolynomial)
        {
            if (otherPolynomial.IsZero) throw new Exception("0'a bölünmez.");
            else if (this.IsZero) { return Polynomial.Zero; }
            else
            {
                if (this.Degree < otherPolynomial.Degree) return this;
                Polynomial P = this.Clone();
                for (int i = 0; i < this.Degree - otherPolynomial.Degree + 1; i++)
                {
                    Term T = P.terms[0].DividedBy(otherPolynomial.terms[0]);
                    P = P.Sub(otherPolynomial.Mul(T));
                }
                return P;
            }
        }
        private void IncreaseForces(int i) 
        {
            for (int j = 0; j < this.terms.Length; j++)
            {
                this.terms[j].IncreaseForce(i);
            }
        }
        public override string ToString()
        {
            if (this.IsZero) return "0";
            string s = "";
            for (int i = 0; i < this.terms.Length; i++)
            {
                if (i == 0) s += this.terms[0];
                else if (this.terms[i].Coefficient > 0 && i != 0) { s += " +" + this.terms[i]; }
                else s += " " + this.terms[i];
            }
            return s;
        }
        private static Term[] Simplify(Term[] terms)
        {
            List<Term> termList = new List<Term>();

            for (int i = 0; i < terms.Length; i++)
            {
                for (int j = i + 1; j < terms.Length; j++)
                {
                    if (terms[i].Force == terms[j].Force)
                    {
                        terms[i] = new Term(terms[i].Coefficient + terms[j].Coefficient, terms[i].Force);
                    }
                }
            }
            for (int i = 0; i < terms.Length; i++)
            {
                bool isExist = false; int sametimes = 0;
                for (int j = 0; j < termList.Count; j++)
                {
                    if (terms[i].Force == termList[j].Force) { isExist = true; sametimes++; }
                }
                if (isExist) continue;
                termList.Add(terms[i]);
            }
            return termList.ToArray();
        }
        public Polynomial Clone()
        {
            if (this.IsZero) return Polynomial.Zero;
            else return new Polynomial(this.terms);
            
        }
        public int CompareTo(Polynomial other)
        {
            if (this.IsZero && other.IsZero) return 0;
            else if (!this.IsZero && other.IsZero) return -1;
            else if (this.IsZero && !other.IsZero) return 1;
            else 
            {
                if (this.Degree > other.Degree) return 1;
                else if (this.Degree < other.Degree) return -1;
                else
                {
                    for (int i = 0; i <= this.Degree; i++)
                    {
                        if (this.GetCoefficient(this.Degree - i) > other.GetCoefficient(other.Degree - i)) return 1;
                        else if (this.GetCoefficient(this.Degree - i) < other.GetCoefficient(other.Degree - i)) return -1;
                    }
                    return 0;
                }
            }
        }



        Polynomial AllowToGenerics<Vector, Polynomial>.Add(Polynomial operand)
        {
            return this.Add(operand);
        }
        Polynomial AllowToGenerics<Vector, Polynomial>.Sub(Polynomial operand)
        {
            return this.Sub(operand);
        }
        Polynomial AllowToGenerics<Vector, Polynomial>.Mul(Polynomial operand)
        {
            return this.Mul(operand);
        }
        Polynomial AllowToGenerics<Vector, Polynomial>.Div(Polynomial operand)
        {
            return this.Div(operand);
        }
        Polynomial AllowToGenerics<Matrix, Polynomial>.Add(Polynomial operand)
        {
            return this.Add(operand);
        }
        Polynomial AllowToGenerics<Matrix, Polynomial>.Sub(Polynomial operand)
        {
            return this.Sub(operand);
        }
        Polynomial AllowToGenerics<Matrix, Polynomial>.Mul(Polynomial operand)
        {
            return this.Mul(operand);
        }
        Polynomial AllowToGenerics<Matrix, Polynomial>.Div(Polynomial operand)
        {
            return this.Div(operand);
        }
        Polynomial IDivisible<Polynomial>.Add(Polynomial operand)
        {
            return this.Add(operand);
        }
        Polynomial IDivisible<Polynomial>.Sub(Polynomial operand)
        {
            return this.Sub(operand);
        }
        Polynomial IDivisible<Polynomial>.Mul(Polynomial operand)
        {
            return this.Mul(operand);
        }
        Polynomial IDivisible<Polynomial>.Div(Polynomial operand)
        {
            return this.Div(operand);
        }
        Polynomial IOperable<Polynomial>.Add(Polynomial operand)
        {
            return this.Add(operand);
        }
        Polynomial IOperable<Polynomial>.Sub(Polynomial operand)
        {
            return this.Sub(operand);
        }
        Polynomial IOperable<Polynomial>.Mul(Polynomial operand)
        {
            return this.Mul(operand);
        }
    }
}
