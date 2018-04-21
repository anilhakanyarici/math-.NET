using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    
    class Vector : ICloneable
    {
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        private enum VectorConstructorOptions { UnitX, UnitY, UnitZ, RandomUnit } 
        
        
        #region: Properties

        
        public double Normalize { get { return this.GetNormalize(); } }
        public double Norm { get { return this.GetNorm(); } }
        public bool IsUnitVector { get { return this.GetNormalize() == 1; } }

        public Position StartPosition { get; set; }
        public Position EndPosition { get { return StartPosition + this; } }

        public static Vector UnitX { get { return new Vector(VectorConstructorOptions.UnitX); } }
        public static Vector UnitY { get { return new Vector(VectorConstructorOptions.UnitY); } }
        public static Vector UnitZ { get { return new Vector(VectorConstructorOptions.UnitZ); } }
        public static Vector RandomUnit { get { return new Vector(VectorConstructorOptions.RandomUnit); } }

        #endregion

        #region: Constructors

        
        public Vector(double x, double y, double z) 
        {

            if (Math.Abs(x) < 0.0000001) x = 0;
            if (Math.Abs(y) < 0.0000001) y = 0;
            if (Math.Abs(z) < 0.0000001) z = 0;

            this.X = x; this.Y = y; this.Z = z;
            this.StartPosition = Position.Origin;
        }
        public Vector(Position startPosition, Position endPosition)
        {
            this.X = endPosition.X - startPosition.X;
            this.Y = endPosition.Y - startPosition.Y;
            this.Z = endPosition.Z - startPosition.Z;
        }
        public Vector(Position startPosition, double x, double y, double z) 
        {
            if (Math.Abs(x) < 0.0000001) x = 0;
            if (Math.Abs(y) < 0.0000001) y = 0;
            if (Math.Abs(z) < 0.0000001) z = 0;
            this.StartPosition = startPosition;
            this.X = x; this.Y = y; this.Z = z;
        }

        private Vector(VectorConstructorOptions options)
        {
            if (options == VectorConstructorOptions.UnitX) { this.X = 1; this.Y = 0; this.Z = 0; }
            else if (options == VectorConstructorOptions.UnitY) { this.X = 0; this.Y = 1; this.Z = 0; }
            else if (options == VectorConstructorOptions.UnitZ) { this.X = 0; this.Y = 0; this.Z = 1; }
            else if (options == VectorConstructorOptions.RandomUnit) 
            {
                Random rnd = new Random();
                double i, j, k;

                while (true) 
                {
                    i = rnd.Next(100); j = rnd.Next(100); k = rnd.Next(100);
                    double Length = Math.Sqrt(i * i + j * j + k * k);
                    i = i / Length; j = j / Length; k = k / Length;
                    if (i * i + j * j + k * k == 1)
                    {
                        this.X = i; this.Y = j; this.Z = k;
                        break;
                    }
                }
                
                    
            } 
        }

        #endregion

        #region: Operational Methods

        private double GetNormalize() 
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
        private double GetNorm() 
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }
        public Vector Unit()
        {
            return this / this.GetNormalize();
        }
        public Vector Reciprocal() 
        {
            return this / this.Norm;
        }
        public Matrix ProjectMatrix()
        {
            Vector Unit = this.Unit();
            Matrix.Row RM = new Matrix.Row(Unit.X, Unit.Y, Unit.Z);
            Matrix.Column CM = RM.Transpose();
            return CM.MultiplyBy(RM);
        }


        public static double Cos(Vector V1, Vector V2)
        {
            return ~V2 * ~V1;
        }
        public static double Sin(Vector V1, Vector V2)
        {
            return Math.Sqrt(1 - Math.Pow(Vector.Cos(V1, V2), 2));
        }
        public static Vector LERP(Vector vector, Vector target, double ratio)
        {
            return vector + (target - vector) * ratio;
        }
        public static Vector SLERP(Vector vector, Vector target, double ratio)
        {
            double theta = Math.Acos(vector * target / (vector.Normalize * target.Normalize));
            double U = Math.Sin((1 - ratio) * theta) / Math.Sin(theta);
            double V = Math.Sin(ratio * theta) / Math.Sin(theta);
            return U * vector + V * target;
        }
        public static Vector RotateAroundAxis(Vector vector, Vector axis, Degrees degree)
        {
            Quaternion Q = new Quaternion(degree / 2, axis);
            Quaternion P = (Q * vector) * Q.Inverse();
            return P.Vector;
        }
        public static Degrees AngleBetween(Vector vector1, Vector vector2)
        {
            return Math.Acos(Vector.Cos(vector1, vector2)) * 180 / Math.PI;
        }
        public static Vector Project(Vector V, Vector baseVector)
        {
            Vector Result = (V * ~baseVector) * ~baseVector;
            Result.StartPosition = baseVector.StartPosition;
            return Result;
        }
        public static Vector Reflect(Vector vector, Vector normal)
        {
            Vector U = Vector.Project(vector, normal);
            Vector V = vector - U;
            return V - U;
        }
        public static Quaternion Exp(Vector V)
        {
            Vector vv = V.Unit();
            Quaternion Q = new Quaternion(Math.Cos(0.5), Math.Sin(0.5) * vv.X, Math.Sin(0.5) * vv.Y, Math.Sin(0.5) * vv.Z);
            return Q * V.Normalize;
        }
        

        #endregion

        #region: Operator Overloads

        public static Vector operator +(Vector V1, Vector V2)
        {
            return new Vector(V1.StartPosition, V1.X + V2.X, V1.Y + V2.Y, V1.Z + V2.Z);
        }
        public static Vector operator -(Vector V1, Vector V2)
        {
            return new Vector(V2.EndPosition, V1.X - V2.X, V1.Y - V2.Y, V1.Z - V2.Z);
        }
        public static double operator *(Vector V1, Vector V2) 
        {
            return V1.X * V2.X + V1.Y * V2.Y + V1.Z * V2.Z;
        } //Dot Product
        public static Vector operator *(Vector V, double d)
        {
            return new Vector(V.StartPosition, V.X * d, V.Y * d, V.Z * d);
        }
        public static Vector operator *(double d, Vector V)
        {
            return new Vector(V.StartPosition, V.X * d, V.Y * d, V.Z * d);

        }
        public static Vector operator *(Matrix M, Vector V)
        {
            Matrix.Column MC = new Matrix.Column(V.X, V.Y, V.Z);
            MC = M * MC;
            return new Vector(MC.GetValue(1), MC.GetValue(2), MC.GetValue(3));
        }
        public static Vector operator *(Vector V, Matrix M)
        {
            return M * V;
        }

        public static Vector operator %(Vector V1, Vector V2)
        {
            double i = V1.Y * V2.Z - V1.Z * V2.Y;
            double j = V1.X * V2.Z - V1.Z * V2.X;
            double k = V1.X * V2.Y - V1.Y * V2.X;
            return new Vector(V1.StartPosition, i, -j, k);
        } //Cross Product
        public static Vector operator /(Vector V, double d)
        {
            return new Vector(V.StartPosition, V.X / d, V.Y / d, V.Z / d);

        }
        

       
        public static implicit operator Vector(Position P)
        {
            return new Vector(Position.Origin, P);
        }

       
       
        public static Vector operator -(Vector V)
        {
            return (-1) * V;
        }
        public static Vector operator ~(Vector V)
        {
            return V.Unit();
        } //Unit

        public static bool operator ==(Vector V1, Vector V2)
        {
            return (V1.X == V2.X) && (V1.Y == V2.Y) && (V1.Z == V2.Z);
        }
        public static bool operator !=(Vector V1, Vector V2)
        {
            return !(V1 == V2);
        }
        public static bool operator |(Vector V1, Vector V2)
        {
            return ~V1 == ~V2;
        } // Is Same Aspect?
        

        #endregion

        #region: Converter Methods

        public override string ToString()
        {
            return "V(" + this.X + ", " + this.Y + ", " + this.Z + ")";
        }
        
        public static Vector[] GramSchmidtOrthogonalization(params Vector[] Vectors)
        {
            Vector[] OrthVec = new Vector[Vectors.Length];
            Vector TempV = new Vector(0, 0, 0);
            OrthVec[0] = ~Vectors[0];
            OrthVec[1] = (Vectors[1] - Vector.Project(Vectors[1], OrthVec[0]));
            for (int k = 2; k < Vectors.Length; k++)
            {
                for (int i = 0; i < k; i++)
                {
                    TempV += Vector.Project(Vectors[k], ~OrthVec[i]);
                }
                OrthVec[k] = ~(Vectors[k] - TempV);
            }
            return OrthVec;
        }


        #endregion

        #region: C#

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public object Clone()
        {
            return new Vector(this.StartPosition, this.X, this.Y, this.Z);
        }

        #endregion

        public class Position
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }


            public Position(double x, double y, double z)
            {
                this.X = x; this.Y = y; this.Z = z;
            }

            public static Position operator +(Position P, Vector V)
            {
                return new Position(P.X + V.X, P.Y + V.Y, P.Z + V.Z);
            }
            public static Position operator -(Position P, Vector V)
            {
                return new Position(P.X - V.X, P.Y - V.Y, P.Z - V.Z);
            }
            public static Position Origin { get { return new Position(0, 0, 0); } }

            public static Vector Vector(Position initialPosition, Position finalPosition)
            {
                return new Vector(initialPosition, finalPosition);
            }

            public double DistanceTo(Position position)
            {
                return (new Vector(this, position)).Normalize;
            }
            public double DistanceTo(Vector vector)
            {
                Position O = vector.StartPosition;
                Mathematics.Vector OP = new Vector(O, this);
                Mathematics.Vector OP2 = Mathematics.Vector.Project(OP, vector);
                return (OP2 - OP).Normalize;
            }

            public Position Symmetry(Position position)
            {
                Mathematics.Vector dif = new Vector(this, position);
                return position + dif;
            }
            public Position Symmetry(Vector vector)
            {
                Position O = vector.StartPosition;
                Mathematics.Vector OP = new Vector(O, this);
                Mathematics.Vector PQ = Mathematics.Vector.Project(OP, vector) - OP;
                return PQ.EndPosition + PQ;
            }

            public override string ToString()
            {
                return "P(" + this.X + ", " + this.Y + ", " + this.Z + ")";
            }


        }
    }
}
