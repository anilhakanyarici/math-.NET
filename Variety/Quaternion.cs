using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    class Quaternion
    {
        public double W { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector Vector { get { return new Vector(this.X, this.Y, this.Z); } }
        public double Scalar { get { return this.W; } }

        public Matrix QuaternionMatrix { get { return new Matrix(new double[4, 4] { { this.W, -this.X, -this.Y, -this.Z }, { this.X, this.W, -this.Z, this.Y }, { this.Y, this.Z, this.W, -this.X }, { this.Z, -this.Y, this.X, this.W } }); } }
        public Matrix RotationMatrix
        {
            get
            {
                double R11 = 1 - 2 * (this.Y * this.Y + this.Z * this.Z);
                double R22 = 1 - 2 * (this.X * this.X + this.Z * this.Z);
                double R33 = 1 - 2 * (this.X * this.X + this.Y * this.Y);

                double R12 = 2 * (this.X * this.Y + this.W * this.Z);
                double R13 = 2 * (this.X * this.Z - this.W * this.Y);
                double R21 = 2 * (this.X * this.Y - this.W * this.Z);
                double R23 = 2 * (this.Z * this.Y + this.W * this.X);
                double R31 = 2 * (this.X * this.Z + this.W * this.Y);
                double R32 = 2 * (this.Z * this.Y - this.W * this.X);

                return new Matrix(new double[3, 3] { { R11, R12, R13 }, { R21, R22, R23 }, { R31, R32, R33 } });
            }
        }

        public Quaternion(double w, double x, double y, double z)
        {
            if (Math.Abs(x) < 0.0000001) x = 0;
            if (Math.Abs(y) < 0.0000001) y = 0;
            if (Math.Abs(z) < 0.0000001) z = 0;
            if (Math.Abs(w) < 0.0000001) w = 0;

            this.W = w; this.X = x; this.Y = y; this.Z = z;
        }
        public Quaternion(double w, Vector V)
        {
            if (Math.Abs(w) < 0.0000001) w = 0;
            this.W = w; this.X = V.X; this.Y = V.Y; this.Z = V.Z;
        }
        public Quaternion(Degrees D, Vector V)
        {
            this.W = Math.Cos(D.Radian);
            V = V.Unit();
            double sinD = Math.Sin(D.Radian);
            V = V * sinD;
            this.X = V.X;
            this.Y = V.Y;
            this.Z = V.Z;
        }


        public static Quaternion operator +(Quaternion Q1, Quaternion Q2)
        {
            return new Quaternion(Q1.W + Q2.W, Q1.X + Q2.X, Q1.Y + Q2.Y, Q1.Z + Q2.Z);
        }
        public static Quaternion operator -(Quaternion Q1, Quaternion Q2)
        {
            return new Quaternion(Q1.W - Q2.W, Q1.X - Q2.X, Q1.Y - Q2.Y, Q1.Z - Q2.Z);
        }
        public static Quaternion operator *(Quaternion Q1, Quaternion Q2)
        {
            return new Quaternion(Q1.W * Q2.W - Q1.Vector * Q2.Vector, Q1.W * Q2.Vector + Q2.W * Q1.Vector + Q1.Vector % Q2.Vector);
        }
        public static Quaternion operator *(Quaternion Q, Vector V)
        {
            Quaternion newQ = new Quaternion(new Degrees(90), V);
            return Q * newQ;
        }
        public static Quaternion operator *(Vector V, Quaternion Q)
        {
            Quaternion newQ = new Quaternion(new Degrees(90), V);
            return newQ * Q;
        }
        public static Quaternion operator *(Quaternion Q, double D)
        {
            return new Quaternion(Q.W * D, Q.Vector * D);
        }
        public static Quaternion operator *(double D, Quaternion Q)
        {
            return new Quaternion(Q.W * D, Q.Vector * D);
        }
        public static Quaternion operator /(Quaternion Q, double D)
        {
            return new Quaternion(Q.W / D, Q.Vector / D);
        }

        public static implicit operator Quaternion(Vector V) 
        {
            return new Quaternion(new Degrees(0), V);
        }

        public Quaternion Conjuge()
        {
            return new Quaternion(this.W, -this.Vector);
        }
        public double Norm() 
        {
            return Math.Sqrt(this.W * this.W + this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
        public Quaternion Inverse()
        {
            return this.Conjuge() / (this.W * this.W + this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
        public Quaternion Unit()
        {
            return this / this.Norm();
        }

        public override string ToString()
        {
            return this.W + " + " + this.Vector.ToString();
        }
    }
}