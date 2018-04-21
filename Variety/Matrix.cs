using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class Matrix : ICloneable
    {
        private double[,] matrix;
        private enum SquareMatrixConstructorsOptions { Zeros, Ones, Identity, Random, RandomUpperTriangular, RandomLowerTriangular, RandomDiagonal }
        private enum MatrixConstructorsOptions { Zeros, Ones, Random }
        public double GetValue(int i, int j)
        {
            if (i <= 0 || j <= 0 || i > this.matrix.GetLength(0) || j > this.matrix.GetLength(1))
                return double.NaN;
            return this.matrix[i - 1, j - 1];
        }
        public double this[int i, int j] { set { this.matrix[i - 1, j - 1] = value; } }
        
        
        #region: Properties

        public int RowsCount { get { return this.matrix.GetLength(0); } }
        public int ColumnsCount { get { return this.matrix.GetLength(1); } }
        public int Degree { get { return this.matrix.GetLength(1) * this.matrix.GetLength(0); } }
        public string MatrixString { get { return this.GetMatrixString(); } }
        public bool IsZeros { get { return this.isZeros(); } }
        public bool IsOnes { get { return this.isOnes(); } }
        public bool IsSameElements { get { return this.isSameElements(); } }
        public bool IsNaN { get { return this.isNaN(); } }
        public bool IsSquareMatrix { get { return this.RowsCount == this.ColumnsCount; } }
        public bool IsOrthogonal { get { return this.Inverse().Equals(this.Transpose()); } }
        public bool IsSymmetric { get { return this.Equals(this.Transpose()); } }
        public bool IsAsymmetric { get { return this.Equals((-1 * this.Transpose())); } }
        public bool IsIdentity { get { return this.isIdentity(); } }
        public bool IsUpperTriangular { get { return this.isUpperTriangular(); } }
        public bool IsLowerTriangular { get { return this.isLowerTriangular(); } }
        public bool IsDiagonal { get { return this.isLowerTriangular() && this.isUpperTriangular(); } }
        public bool IsTriangular { get { return this.isLowerTriangular() || this.isUpperTriangular(); } }
        
        #endregion

        #region: Constructors

        public Matrix()
        {
            throw new Exception("Boş bir matris belirtilemez.");
        }
        public Matrix(int i, int j)
        {
            this.matrix = new double[i, j];
            for (int x = 0; x < i; x++)
                for (int y = 0; y < j; y++)
                    this.matrix[x, y] = double.NaN;
        }
        public Matrix(double[,] matrix)
        {
            this.matrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0);i++ )
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    double intD = Math.Round(matrix[i, j]);
                    if (matrix[i, j] < intD + Math.Pow(10, -10) && matrix[i, j] > intD - Math.Pow(10, -10))
                        this.matrix[i, j] = intD;
                    else this.matrix[i, j] = matrix[i, j];
                }
        }
        public Matrix(string Elements)
        {
            string[] rows = new string[Elements.Split('|').Length];
            for (int i = 0; i < Elements.Split('|').Length; i++)
                rows[i] = Elements.Split('|')[i];
            bool IsMatrix = true;
            int ColumnsCount = rows[0].Split(';').Length;
            int RowsCount = rows.Length;
            for (int i = 1; i < rows.Length; i++)
                IsMatrix = rows[i].Split(';').Length == ColumnsCount;
            if (!IsMatrix) throw new Exception("Girilen dize, bir matris dizesi değil.");
            else
            {
                this.matrix = new double[RowsCount, ColumnsCount];

                for (int i = 0; i < RowsCount; i++)
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        try { this.matrix[i, j] = double.Parse(rows[i].Split(';')[j]); }
                        catch (Exception) { throw new Exception("Dize değeri boş veya doğru değil."); }
                    }
                        
                        
            }
        }
        public Matrix(params Row[] rows)
        {
            bool validity = true;
            for (int i = 1; i < rows.Length; i++) { if (rows[0].Length != rows[i].Length) validity = false; }
            if (!validity) throw new Exception("Satır matris dizisinin elemanları birbiri ile uyumsuz.");
            else
            {
                this.matrix = new double[rows.Length, rows[0].Length];
                for (int i = 0; i < rows.Length; i++)
                {
                    for (int j = 0; j < rows[0].Length; j++)
                    {
                        try { matrix[i, j] = rows[i].GetValue(j + 1); }
                        catch (Exception) { throw new Exception("Satır matrislerinden birisinin değeri boş veya doğru değil."); }
                        
                    }
                }
            }
        }
        public Matrix(params Column[] columns)
        {
            bool validity = true;
            for (int i = 1; i < columns.Length; i++) { if (columns[0].Length != columns[i].Length) validity = false; }
            if (!validity) this.matrix = new double[0, 0];
            else
            {
                this.matrix = new double[columns[0].Length, columns.Length];
                for (int i = 0; i < columns[0].Length; i++)
                {
                    for (int j = 0; j < columns.Length; j++)
                    {
                        try { matrix[i, j] = columns[j].GetValue(i + 1); }
                        catch (Exception) { throw new Exception("Sütun matrislerinden birisinin değeri boş veya doğru değil."); }
                        
                    }
                }
            }
        }

        private Matrix(int i, SquareMatrixConstructorsOptions options)
        {
            if (options == SquareMatrixConstructorsOptions.Ones)
            {
                this.matrix = new double[i, i];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                        this.matrix[x, y] = 1;
            }
            if (options == SquareMatrixConstructorsOptions.Zeros)
            {
                this.matrix = new double[i, i];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                        this.matrix[x, y] = 0;
            }
            if (options == SquareMatrixConstructorsOptions.Identity)
            {
                this.matrix = new double[i, i];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                    {
                        if (x == y) { this.matrix[x, y] = 1; }
                        else this.matrix[x, y] = 0;
                    }
            }
            if (options == SquareMatrixConstructorsOptions.Random)
            {
                this.matrix = new double[i, i];
                Random rnd = new Random();
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                    {
                        this.matrix[x, y] = (double)rnd.Next(1000) / 1000;
                    }
            }
            if (options == SquareMatrixConstructorsOptions.RandomLowerTriangular)
            {
                this.matrix = new double[i, i];
                Random rnd = new Random();
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                    {
                        if (x >= y)
                            this.matrix[x, y] = (double)rnd.Next(1000) / 1000;
                        else this.matrix[x, y] = 0;
                    }
            }
            if (options == SquareMatrixConstructorsOptions.RandomUpperTriangular)
            {
                this.matrix = new double[i, i];
                Random rnd = new Random();
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                    {
                        if (x <= y)
                            this.matrix[x, y] = (double)rnd.Next(1000) / 1000;
                        else this.matrix[x, y] = 0;
                    }
            }
            if (options == SquareMatrixConstructorsOptions.RandomDiagonal)
            {
                Random rnd = new Random();
                this.matrix = new double[i, i];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < i; y++)
                    {
                        if (x == y) { this.matrix[x, y] = (double)rnd.Next(1000) / 1000; }
                        else this.matrix[x, y] = 0;
                    }
            }
        }
        private Matrix(int i, int j, MatrixConstructorsOptions options)
        {
            if (options == MatrixConstructorsOptions.Ones)
            {
                this.matrix = new double[i, j];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                        this.matrix[x, y] = 1;
            }
            if (options == MatrixConstructorsOptions.Zeros)
            {
                this.matrix = new double[i, j];
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                        this.matrix[x, y] = 0;
            }
            if (options == MatrixConstructorsOptions.Random)
            {
                this.matrix = new double[i, j];
                Random rnd = new Random();
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                        this.matrix[x, y] = (double)rnd.Next(1000) / 1000;
            }
        }


        #endregion

        #region: Methods For Static Constructor

        public static Matrix Random(int RowCount, int ColumnCount)
        {
            return new Matrix(RowCount, ColumnCount, MatrixConstructorsOptions.Random);
        }
        public static Matrix Ones(int RowCount, int ColumnCount)
        {
            return new Matrix(RowCount, ColumnCount, MatrixConstructorsOptions.Ones);
        }
        public static Matrix Zeros(int RowCount, int ColumnCount)
        {
            return new Matrix(RowCount, ColumnCount, MatrixConstructorsOptions.Zeros);
        }

        public static Matrix RandomUpperTriangular(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.RandomUpperTriangular);
        }
        public static Matrix RandomLowerTriangular(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.RandomLowerTriangular);
        }
        public static Matrix RandomDiagonalMatrix(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.RandomDiagonal);
        }
        public static Matrix SquareRandom(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.Random);
        }
        public static Matrix SquareOnes(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.Ones);
        }
        public static Matrix SquareZeros(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.Zeros);
        }
        public static Matrix Identity(int Length)
        {
            return new Matrix(Length, SquareMatrixConstructorsOptions.Identity);
        }
        public static Matrix Diag(params double[] elements)
        {
            double[,] matrix = new double[elements.Length, elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                for (int j = 0; j < elements.Length; j++)
                {
                    if (i == j) matrix[i, j] = elements[i];
                    else matrix[i, j] = 0;
                }
            }
            return new Matrix(matrix);
        }

        public static Matrix MatrixOfRotate2DAxis(Degrees Theta)
        {
            return new Matrix(new double[,] { { Math.Cos(Theta.Radian), Math.Sin(-Theta.Radian) }, { Math.Sin(Theta.Radian), Math.Cos(Theta.Radian) } });
        }

        #endregion

        #region: Operational Methods

        public double Determinant()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Determinant()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin determinantı hesaplanabilir.\nMethod: Determinant()");
            if (this.RowsCount == 1) return this.GetValue(1, 1);
            else if (this.RowsCount == 2) return this.GetValue(1, 1) * this.GetValue(2, 2) - this.GetValue(1, 2) * this.GetValue(2, 1);
            else
            {
                double det = 0;
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    det += Math.Pow(-1, j - 1) * this.GetValue(1, j) * this.DisplaceRowOrColumn(1, j).Determinant();
                }
                return det;
            }
        }
        public double Trace()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Trace()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin izi bulunabilir.\nMethod: Trace()");
            double trace = 0;
            for (int i = 1; i <= this.RowsCount; i++) trace += this.GetValue(i, i);
            return trace;
        }
        
        public Matrix Transpose()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Transpose()");
            double[,] copyMatrix = new double[this.matrix.GetLength(1), this.matrix.GetLength(0)];

            for (int i = 0; i < copyMatrix.GetLength(0); i++)
                for (int j = 0; j < copyMatrix.GetLength(1); j++)
                    copyMatrix[i, j] = this.matrix[j, i];
            return new Matrix(copyMatrix);
        }
        public Matrix Minor()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Minor()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin minor matrisi bulunabilir.\nMethod: Minor()");
            double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];

            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                {
                    copyMatrix[i, j] = this.DisplaceRowOrColumn(i + 1, j + 1).Determinant();
                }
            return new Matrix(copyMatrix);
        }
        public Matrix Cofactor()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Cofactor()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin işaretli minor matrisi bulunabilir.\nMethod: Cofactor() ");
            double[,] copyMatrix = new double[this.matrix.GetLength(0), this.matrix.GetLength(1)];
            Matrix minor = this.Minor();
            for (int i = 0; i < this.matrix.GetLength(0); i++)
                for (int j = 0; j < this.matrix.GetLength(1); j++)
                    copyMatrix[i, j] = Math.Pow(-1, i + j) * minor.matrix[i, j];
            return new Matrix(copyMatrix);
        }
        public Matrix Adjoint()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Adjoint()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin ek matrisi bulunabilir.\nMethod: Adjoint()");
            return this.Cofactor().Transpose();
        }
        public Matrix Inverse()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Inverse()");
            if (!this.IsSquareMatrix) throw new Exception("Yalnız kare matrisin ters matrisi bulunabilir.\nMethod: Inverse()");
            return this.Adjoint() / this.Determinant();
        }

        #endregion

        #region: Methods of Elementry Operations & Row And Column Operations

        public Matrix MultiplyByAsElementer(Matrix matrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: MultiplyByAsElementer()");
            if (this.ColumnsCount == matrix.ColumnsCount && this.RowsCount == matrix.RowsCount)
            {
                double[,] TempMatrix = new double[this.RowsCount, matrix.ColumnsCount];
                for (int i = 1; i <= this.RowsCount; i++)
                    for (int j = 1; j <= matrix.ColumnsCount; j++)
                        TempMatrix[i - 1, j - 1] = this.GetValue(i, j) * matrix.GetValue(i, j);
                return new Matrix(TempMatrix);
            }
            else throw new Exception("Elementer çarpım için iki matrisin de mertebesi aynı olmalı.");
        }
        public Matrix DividedByAsElementer(Matrix matrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: DividedByAsElementer()");
            if (this.ColumnsCount == matrix.ColumnsCount && this.RowsCount == matrix.RowsCount)
            {
                double[,] TempMatrix = new double[this.RowsCount, matrix.ColumnsCount];
                for (int i = 1; i <= this.RowsCount; i++)
                    for (int j = 1; j <= matrix.ColumnsCount; j++)
                    {
                        try
                        {
                            TempMatrix[i - 1, j - 1] = this.GetValue(i, j) / matrix.GetValue(i, j);
                        }
                        catch (DivideByZeroException) { throw new DivideByZeroException("Bölen matrisin elemanlarından hiç birisi 0 olamaz."); }
                    }
                return new Matrix(TempMatrix);
            }
            else throw new Exception("Elementer bölme için iki matrisin de mertebesi aynı olmalı.");
        }
        public Matrix SubMatrix(int StartRow, int StartColumn, int RowsCount, int ColumnsCount)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: SubMatrix()");
            if (RowsCount == 0 || ColumnsCount == 0) throw new Exception("Boş bir matris belirtilemez.\nMethod: SubMatrix()");
            else if (StartRow <= 0 || StartColumn <= 0) throw new Exception("Başlangıç satırı ve sütunu değeri en az 1 olabilir.\nMethod: SubMatrix()");
            Matrix matrix = (Matrix)this.Clone();

            for (int i = 1; i <= (StartRow - 1); i++) { matrix = matrix.DisplaceRowOrColumn(1, 0); }

            for (int j = 1; j <= (StartColumn - 1); j++) { matrix = matrix.DisplaceRowOrColumn(0, 1); }

            double[,] SubMatrix = new double[RowsCount, ColumnsCount];

            for (int i = 0; i < RowsCount; i++)
                for (int j = 0; j < ColumnsCount; j++)
                {
                    SubMatrix[i, j] = matrix.GetValue(i + 1, j + 1);
                }
            return new Matrix(SubMatrix);
        }
        public Matrix PlusOrderToOrder(Row rowMatrix, int index)
        {
            if (index > this.RowsCount || index <= 0) throw new Exception("Matris, toplanacak satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: PlusOrderToOrder()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == i) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] + rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: PlusOrderToOrder()");
        }
        public Matrix PlusOrderToOrder(Column columnMatrix, int index)
        {
            if (index > this.ColumnsCount || index <= 0) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: PlusOrderToOrder()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == j) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] + columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: PlusOrderToOrder()");
        }
        public Matrix PlusOrderToOrder(Row rowMatrix, int index, double multiplier)
        {
            if (index > this.RowsCount || index <= 0) throw new Exception("Matris, toplanacak satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: PlusOrderToOrder()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == i) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] + multiplier * rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: PlusOrderToOrder()");
        }
        public Matrix PlusOrderToOrder(Column columnMatrix, int index, double multiplier)
        {
            if (index > this.ColumnsCount || index <= 0) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: PlusOrderToOrder()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == j) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] + multiplier * columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: PlusOrderToOrder()");
        }
        public Matrix MultiplyByOrderToOrder(Row rowMatrix, int index)
        {
            if (index > this.RowsCount || index <= 0) throw new Exception("Matris, toplanacak satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: MultiplyByOrderToOrder()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == i) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] * rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: MultiplyByOrderToOrder()");
        }
        public Matrix MultiplyByOrderToOrder(Column columnMatrix, int index)
        {
            if (index > this.ColumnsCount || index <= 0) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: MultiplyByOrderToOrder()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == j) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] * columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: MultiplyByOrderToOrder()");
        }
        public Matrix MultiplyByOrderToOrder(Row rowMatrix, int index, double multiplier)
        {
            if (index > this.RowsCount || index <= 0) throw new Exception("Matris, toplanacak satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: MultiplyByOrderToOrder()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == i) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] * multiplier * rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: MultiplyByOrderToOrder()");
        }
        public Matrix MultiplyByOrderToOrder(Column columnMatrix, int index, double multiplier)
        {
            if (index > this.ColumnsCount || index <= 0) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: MultiplyByOrderToOrder()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (index == j) copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1] * multiplier * columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: MultiplyByOrderToOrder()");
        }
        public Matrix MultiplyByRowThis(int i, double multiplier)
        {
            return Replace(this.GetRow(i), i, multiplier);
        }
        public Matrix MultiplyByColumnThis(int i, double multiplier)
        {
            return Replace(this.GetColumn(i), i, multiplier);
        }
        public Matrix DisplaceRow(int i)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: DisplaceRow()");
            else if (this.matrix.GetLength(0) == 1 && i > 0) throw new Exception("Tek satır içeren matristen satır silinemez.\nMethod: DisplaceRow()");
            else if (i > this.matrix.GetLength(0) || i < 0) throw new Exception("Matris dizisi girilen satırı içermiyor.\nMethod: DisplaceRow()");
            return this.DisplaceRowOrColumn(i, 0);
        }
        public Matrix DisplaceColumn(int j)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: DisplaceColumn()");
            else if (this.matrix.GetLength(1) == 1 && j > 0) throw new Exception("Tek sütun içeren matristen sütun silinemez.\nMethod: DisplaceColumn()");
            else if (j > this.matrix.GetLength(1) || j < 0) throw new Exception("Matris dizisi girilen sütunu içermiyor.\nMethod: DisplaceColumn()");
            return this.DisplaceRowOrColumn(0, j);
        }
        public Matrix ClearZerosRows()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: ClearZerosRow()");
            return this.ClearZerosRowOrZerosColumn(true, false);
        }
        public Matrix ClearOnesRows()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: ClearOnesRow()");
            return this.ClearOnesRowOrOnesColumn(true, false);
        }
        public Matrix ClearZerosColumns()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: ClearZerosColumn()");
            return this.ClearZerosRowOrZerosColumn(false, true);
        }
        public Matrix ClearOnesColumns()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: ClearOnesColumn()");
            return this.ClearOnesRowOrOnesColumn(false, true);
        }
        public Matrix Insert(Row rowMatrix, int RowPosition)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Insert()");
            if (RowPosition > this.RowsCount + 1) throw new Exception("Ekleme işleminde arada boş satır bırakılamaz.\nMethod: Insert()");
            else if (RowPosition <= 0) throw new Exception("Sokulacak satır, 1 ve " + (this.RowsCount + 1) + " arasında olmalı.\nMethod: Insert()");
            else if (rowMatrix.Length == this.ColumnsCount)
            {
                int k = 0;
                bool IsCopiedVector = false;
                double[,] copyMatrix = new double[this.RowsCount + 1, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                {
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (RowPosition == i)
                        {
                            copyMatrix[i - 1, j - 1] = rowMatrix.GetValue(j);
                            IsCopiedVector = true;
                        }
                        else copyMatrix[i - 1, j - 1] = this.matrix[k, j - 1];
                    }
                    if (IsCopiedVector) { IsCopiedVector = false; k--; }
                    k++;
                }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.");
        }
        public Matrix Insert(Column columnMatrix, int ColumnPosition)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Insert()");
            if (ColumnPosition > this.ColumnsCount + 1) throw new Exception("Ekleme işleminde arada boş satır bırakılamaz.\nMethod: Insert()");
            else if (ColumnPosition <= 0) throw new Exception("Sokulacak sütun, 1 ve " + (this.ColumnsCount + 1) + " arasında olmalı.\nMethod: Insert()");
            if (columnMatrix.Length == this.RowsCount)
            {
                int k = 0;
                bool IsCopiedVector = false;
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount + 1];
                for (int i = 1; i <= copyMatrix.GetLength(1); i++)
                {
                    for (int j = 1; j <= copyMatrix.GetLength(0); j++)
                    {
                        if (ColumnPosition == i)
                        {
                            copyMatrix[j - 1, i - 1] = columnMatrix.GetValue(j);
                            IsCopiedVector = true;
                        }
                        else copyMatrix[j - 1, i - 1] = this.matrix[j - 1, k];
                    }
                    if (IsCopiedVector) { IsCopiedVector = false; k--; }
                    k++;
                }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.");
        }
        public Matrix Add(Row rowMatrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Add()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                return Insert(rowMatrix, this.RowsCount + 1);
            }
            else return this;
        }
        public Matrix Add(Column columnMatrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Add()");
            if (columnMatrix.Length == this.RowsCount)
            {
                return Insert(columnMatrix, this.ColumnsCount + 1);
            }
            else return this;
        }
        public Matrix Replace(Row rowMatrix, int RowPosition)
        {
            if (RowPosition > this.RowsCount) throw new Exception("Matris, yer değiştirilecek satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: Replace()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (RowPosition == i) copyMatrix[i - 1, j - 1] = rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: Replace()");
        }
        public Matrix Replace(Column columnMatrix, int ColumnPosition)
        {
            if (ColumnPosition > this.ColumnsCount) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: Replace()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (ColumnPosition == j) copyMatrix[i - 1, j - 1] = columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: Replace()");
        }
        public Matrix Replace(Row rowMatrix, int RowPosition, double multiplier)
        {
            if (RowPosition > this.RowsCount) throw new Exception("Matris, yer değiştirilecek satırı içermiyor. 1 ve " + this.RowsCount + " arasında olabilir.\nMethod: Replace()");
            if (rowMatrix.Length == this.ColumnsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (RowPosition == i) copyMatrix[i - 1, j - 1] = multiplier * rowMatrix.GetValue(j);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Satır matrisinin uzunluğu, matrisin sütun sayısına eşit olmalı.\nMethod: Replace()");
        }
        public Matrix Replace(Column columnMatrix, int ColumnPosition, double multiplier)
        {
            if (ColumnPosition > this.ColumnsCount) throw new Exception("Matris, yer değiştirilecek sütunu içermiyor. 1 ve " + this.ColumnsCount + " arasında olabilir.\nMethod: Replace()");
            if (columnMatrix.Length == this.RowsCount)
            {
                double[,] copyMatrix = new double[this.RowsCount, this.ColumnsCount];
                for (int i = 1; i <= copyMatrix.GetLength(0); i++)
                    for (int j = 1; j <= copyMatrix.GetLength(1); j++)
                    {
                        if (ColumnPosition == j) copyMatrix[i - 1, j - 1] = multiplier * columnMatrix.GetValue(i);
                        else copyMatrix[i - 1, j - 1] = this.matrix[i - 1, j - 1];
                    }
                return new Matrix(copyMatrix);
            }
            else throw new Exception("Sütun matrisinin uzunluğu, matrisin satır sayısına eşit olmalı.\nMethod: Replace()");
        }
        public Matrix ReLocateRow(int firstRow, int secondRow)
        {
            if (firstRow > this.RowsCount || firstRow <= 0 || secondRow > this.RowsCount || secondRow <= 0) throw new Exception("Girilen satır numarası aşkın.\nMethod: ReLocateRow()");
            Row RM1 = this.GetRow(firstRow);
            Row RM2 = this.GetRow(secondRow);
            Matrix M = (Matrix)this.Clone();
            M = M.Replace(RM1, secondRow);
            M = M.Replace(RM2, firstRow);
            return M;
        }
        public Matrix ReLocateColumn(int firstColumn, int secondColumn)
        {
            if (firstColumn > this.ColumnsCount || firstColumn <= 0 || secondColumn > this.ColumnsCount || secondColumn <= 0) throw new Exception("Girilen satır numarası aşkın.\nMethod: ReLocateColumn()");
            Column CM1 = this.GetColumn(firstColumn);
            Column CM2 = this.GetColumn(secondColumn);
            Matrix M = (Matrix)this.Clone();
            M = M.Replace(CM1, secondColumn);
            M = M.Replace(CM2, firstColumn);
            return M;
        }
        public Matrix Eliminate()
        {
            Matrix ResultMatrix = (Matrix)this.Clone();
            for (int i = 1; i < ResultMatrix.RowsCount; i++)
            {
                if (ResultMatrix.GetValue(i, i) == 0)
                {
                    for (int x = i + 1; x <= ResultMatrix.RowsCount + 1 - i; x++)
                    {
                        if (ResultMatrix.GetValue(x, i) != 0)
                        {
                            ResultMatrix = ResultMatrix.ReLocateRow(i, x);
                            break;
                        }
                    }
                }
                for (int y = i + 1; y <= ResultMatrix.RowsCount; y++)
                    ResultMatrix = ResultMatrix.PlusOrderToOrder(ResultMatrix.GetRow(i), y, -ResultMatrix.GetValue(y, i) / ResultMatrix.GetValue(i, i));
            }
            return ResultMatrix;

        }
        public Row GetRow(int i)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: GetRow()");
            if (i > this.RowsCount) throw new Exception("Matris, bu kadar çok satır içermiyor.\nMethod: GetRow()");
            double[] copyMatrix = new double[this.ColumnsCount];
            for (int x = 0; x < this.ColumnsCount; x++)
                copyMatrix[x] = this.matrix[i - 1, x];
            return new Row(copyMatrix);
        }
        public Column GetColumn(int j)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: GetColumn()");
            if (j > this.ColumnsCount) throw new Exception("Matris, bu kadar çok sütun içermiyor.\nMethod: GetColumn()");
            double[] copyMatrix = new double[this.RowsCount];
            for (int x = 0; x < this.RowsCount; x++)
                copyMatrix[x] = this.matrix[x, j - 1];
            return new Column(copyMatrix);
        }

        #endregion

        #region: Characteristic Methods

        private bool isZeros()
        {
            bool result = true;
            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                {
                    if (this.GetValue(i + 1, j + 1) != 0)
                        result = false;
                }
            return result;
        }
        private bool isNaN()
        {
            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                {
                    if (this.GetValue(i + 1, j + 1).Equals(double.NaN))
                        return true;
                }
            return false;
        }
        private bool isOnes()
        {
            bool result = true;
            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                {
                    if (this.GetValue(i + 1, j + 1) != 1)
                        result = false;
                }
            return result;
        }
        private bool isSameElements()
        {
            if (this.GetValue(1, 1) == 0) return this.isZeros();
            else
            {
                double factor = this.GetValue(1, 1);
                Matrix mtrx = this / factor;
                return mtrx.isOnes();
            }
        }
        private string GetMatrixString()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi bir string içermez.\nProperty: MatrixString");
            string MatrixString = "";
            for (int i = 1; i <= this.RowsCount; i++)
            {
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    MatrixString += this.GetValue(i, j) + ";";
                }
                MatrixString = MatrixString.Substring(0, MatrixString.Length - 1);
                MatrixString += "|";
            }
            try
            {
                return MatrixString.Substring(0, MatrixString.Length - 1);
            }
            catch (Exception) { return ""; }
        }
        public override bool Equals(object otherMatrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Equals()");
            Matrix matrix = (Matrix)otherMatrix;
            if (matrix.GetHashCode() == this.GetHashCode()) return true;
            if (!(this.RowsCount == matrix.RowsCount && this.ColumnsCount == matrix.ColumnsCount)) return false;
            else
            {
                bool Result = true;
                for (int i = 1; i <= this.RowsCount; i++)
                {
                    for (int j = 1; j <= this.ColumnsCount; j++)
                    {
                        if (this.GetValue(i, j) - Math.Pow(10, -10) > matrix.GetValue(i, j) || this.GetValue(i, j) + Math.Pow(10, -10) < matrix.GetValue(i, j))
                        {
                            Result = false;
                            break;
                        }
                    }
                    if (!Result) break;
                }
                return Result;
            }
        }
        public object Clone()
        {
            return new Matrix(this.MatrixString);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        private bool isIdentity()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nProperty: IsIdentity");
            if (!this.IsSquareMatrix) return false;
            Matrix unitMatrix = new Matrix(this.RowsCount, SquareMatrixConstructorsOptions.Identity);
            return this.Equals(unitMatrix);
        }
        private bool isLowerTriangular()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nProperty: IsLowerTriangular");
            if (!this.IsSquareMatrix) throw new Exception("Kare olmayan matrisler üçgensel olmaz.\nProperty: IsLowerTriangular");
            for (int i = 1; i <= this.RowsCount; i++)
            {
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    if (i < j) { if (this.GetValue(i, j) != 0) return false; }
                }
            }
            return true;
        }
        private bool isUpperTriangular()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nProperty: IsUpperTriangular");
            if (!this.IsSquareMatrix) return false;
            for (int i = 1; i <= this.RowsCount; i++)
            {
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    if (i > j) { if (this.GetValue(i, j) != 0) return false; }
                }
            }
            return true;
        }
        public double Norm()
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: Norm()");
            if (!this.IsSquareMatrix) throw new Exception("Normu bulunacak matris kare olmalıdır.\nMethod: Norm()");
            double result = 0;
            for (int i = 1; i <= this.RowsCount; i++)
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    result += Math.Abs(this.GetValue(i, j));
                }
            return Math.Sqrt(result);
        }
        public bool IsSimilar(Matrix matrix)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: IsSimilar()");
            if (!this.IsSquareMatrix || matrix.IsSquareMatrix) throw new Exception("Benzerlik yalnız kare matrisler için geçerlidir.\nMethod: IsSimilar()");
            if (this.Degree != matrix.Degree) return false;
            return (this.Trace() == matrix.Trace() && this.Determinant() == matrix.Determinant());
        }
        public int CompareTo(Matrix other)
        {
            if (this.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nMethod: CompareTo()");
            if (this.Degree != other.Degree) throw new Exception("Matrislerin mertebeleri aynı değil.\nMethod: CompareTo()");
            if (this.Determinant() > other.Determinant()) return 1;
            else if (this.Determinant() < other.Determinant()) return -1;
            else return 0;
        }

        #endregion

        #region: Operator Overloads

        public static Matrix operator +(Matrix M1, Matrix M2)
        {
            if (M1.IsNaN || M2.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: +");
            if (M1.ColumnsCount == M2.ColumnsCount && M1.RowsCount == M2.RowsCount)
            {
                double[,] TempMatrix = new double[M1.RowsCount, M2.ColumnsCount];
                for (int i = 1; i <= M1.RowsCount; i++)
                    for (int j = 1; j <= M2.ColumnsCount; j++)
                        TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) + M2.GetValue(i, j);
                return new Matrix(TempMatrix);
            }
            else throw new Exception("Mertebeleri aynı olmayan matrisler toplanamazlar.\nOperator: +");
        }
        public static Matrix operator +(Matrix M1, double A)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: +");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) + A;
            return new Matrix(TempMatrix);
        }
        public static Matrix operator +(double A, Matrix M1)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: +");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) + A;
            return new Matrix(TempMatrix);

        }

        public static Matrix operator -(Matrix M1, Matrix M2)
        {
            if (M1.IsNaN || M2.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: -");
            if (M1.ColumnsCount == M2.ColumnsCount && M1.RowsCount == M2.RowsCount)
            {
                double[,] TempMatrix = new double[M1.RowsCount, M2.ColumnsCount];
                for (int i = 1; i <= M1.RowsCount; i++)
                    for (int j = 1; j <= M2.ColumnsCount; j++)
                        TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) - M2.GetValue(i, j);
                return new Matrix(TempMatrix);
            }
            else throw new Exception("Mertebeleri aynı olmayan matrisler çıkarılamazlar.\nOperator: -");
        }
        public static Matrix operator -(Matrix M1, double A)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: -");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) - A;
            return new Matrix(TempMatrix);
        }
        public static Matrix operator -(double A, Matrix M1)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: -");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = A - M1.GetValue(i, j);
            return new Matrix(TempMatrix);

        }

        public static Matrix operator *(Matrix M1, Matrix M2)
        {
            if (M1.IsNaN || M2.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: *");
            double[,] TempArray = new double[0, 0];
            if (M1.ColumnsCount == M2.RowsCount)
            {
                TempArray = new double[M1.RowsCount, M2.ColumnsCount];
                double tempValue = 0;
                for (int i = 1; i <= M1.RowsCount; i++)
                {
                    for (int j = 1; j <= M2.ColumnsCount; j++)
                    {
                        for (int k = 1; k <= M1.ColumnsCount; k++)
                            tempValue += M1.GetValue(i, k) * M2.GetValue(k, j);
                        TempArray[i - 1, j - 1] = tempValue;
                        tempValue = 0;
                    }
                }
                return new Matrix(TempArray);
            }
            else throw new Exception("Birincinin sütun sayısı ile ikincinin satır sayısı aynı olmayan matrisler çarpılamaz.\nOperator: *");
        }
        public static Matrix operator *(Matrix M1, double A)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: *");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) * A;
            return new Matrix(TempMatrix);
        }
        public static Matrix operator *(double A, Matrix M1)
        {
            if (M1.IsNaN) throw new Exception("NaN matrisi ile işlem yapılamaz.\nOperator: *");
            double[,] TempMatrix = new double[M1.RowsCount, M1.ColumnsCount];
            for (int i = 1; i <= M1.RowsCount; i++)
                for (int j = 1; j <= M1.ColumnsCount; j++)
                    TempMatrix[i - 1, j - 1] = M1.GetValue(i, j) * A;
            return new Matrix(TempMatrix);
        }

        public static Matrix operator /(Matrix M1, Matrix M2)
        {
            return M1 * M2.Inverse();
        }
        public static Matrix operator /(Matrix M1, double A)
        {
            return M1 * (1.0 / A);
        }
        

        public static implicit operator Matrix(Row R)
        {
            double[,] matrix = new double[1, R.Length];
            for (int i = 0; i < R.Length; i++) matrix[0, i] = R.GetValue(i + 1);
            return new Matrix(matrix);
        }
        public static implicit operator Matrix(Column C)
        {
            double[,] matrix = new double[C.Length, 1];
            for (int i = 0; i < C.Length; i++) matrix[i, 0] = C.GetValue(i + 1);
            return new Matrix(matrix);
        }
        public static implicit operator Matrix(string Elements)
        {
            double[,] copyMatrix;
            string[] rows = new string[Elements.Split('|').Length];
            for (int i = 0; i < Elements.Split('|').Length; i++)
                rows[i] = Elements.Split('|')[i];
            bool IsMatrix = true;
            int ColumnsCount = rows[0].Split(';').Length;
            int RowsCount = rows.Length;
            for (int i = 1; i < rows.Length; i++)
                IsMatrix = rows[i].Split(';').Length == ColumnsCount;
            if (!IsMatrix) throw new Exception("Girilen dize, bir matris dizesi değil.");
            else
            {
                copyMatrix = new double[RowsCount, ColumnsCount];

                for (int i = 0; i < RowsCount; i++)
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        try { copyMatrix[i, j] = double.Parse(rows[i].Split(';')[j]); }
                        catch (Exception) { throw new Exception("Dize değeri boş veya doğru değil."); }
                    }
            }
            return new Matrix(copyMatrix);
        }
        public static implicit operator Matrix(double[,] D)
        {
            return new Matrix(D);
        }

        public static Matrix operator ~(Matrix M)
        {
            return M.Transpose();
        }
        public static Matrix operator -(Matrix M)
        {
            return (-1) * M;
        }
        public static Matrix operator !(Matrix M)
        {
            return M.Inverse();
        }

        public static bool operator ==(Matrix M1, Matrix M2)
        {
            if (!(M1.RowsCount == M2.RowsCount && M1.ColumnsCount == M2.ColumnsCount)) return false;
            else
            {
                bool Result = true;
                for (int i = 1; i <= M1.RowsCount; i++)
                {
                    for (int j = 1; j <= M1.ColumnsCount; j++)
                    {
                        if (M1.GetValue(i, j) - Math.Pow(10, -10) > M1.GetValue(i, j) || M1.GetValue(i, j) + Math.Pow(10, -10) < M1.GetValue(i, j))
                        {
                            Result = false;
                            break;
                        }
                    }
                    if (!Result) break;
                }
                return Result;
            }
        }
        public static bool operator !=(Matrix M1, Matrix M2)
        {
            return !(M1 == M2);
        }

        #endregion

        #region: Converter Methods

        public override string ToString()
        {
            string Lines = "";
            for (int i = 1; i <= this.RowsCount; i++)
            {
                for (int j = 1; j <= this.ColumnsCount; j++)
                {
                    Lines += this.GetValue(i, j).ToString("0.000") + "  " + "\t";
                }
                Lines += "\n";
            }
            return Lines;
        }
        
        #endregion

        #region: Helper Methods

        private Matrix ClearZerosRowOrZerosColumn(bool isClearRow, bool isClearColumn)
        {
            Matrix copyMatrix1 = (Matrix)this.Clone();
            int Loop;
            if (isClearRow)
            {
                Loop = this.RowsCount;
                for (int i = 1; i <= Loop; i++)
                {
                    if (copyMatrix1.GetRow(i).IsZeros)
                    {
                        copyMatrix1 = copyMatrix1.DisplaceRowOrColumn(i, 0);
                        i--; Loop--;
                    }
                }
            }
            if (isClearColumn)
            {
                Loop = this.ColumnsCount;
                for (int j = 1; j <= Loop; j++)
                {
                    if (copyMatrix1.GetColumn(j).IsZeros)
                    {
                        copyMatrix1 = copyMatrix1.DisplaceRowOrColumn(0, j);
                        j--; Loop--;
                    }
                }
            }
            return copyMatrix1;
        }
        private Matrix ClearOnesRowOrOnesColumn(bool isClearRow, bool isClearColumn)
        {
            Matrix copyMatrix1 = (Matrix)this.Clone();
            int Loop;
            if (isClearRow)
            {
                Loop = this.RowsCount;
                for (int i = 1; i <= Loop; i++)
                {
                    if (copyMatrix1.GetRow(i).IsOnes)
                    {
                        copyMatrix1 = copyMatrix1.DisplaceRowOrColumn(i, 0);
                        i--; Loop--;
                    }
                }
            }
            if (isClearColumn)
            {
                Loop = this.ColumnsCount;
                for (int j = 1; j <= Loop; j++)
                {
                    if (copyMatrix1.GetColumn(j).IsOnes)
                    {
                        copyMatrix1 = copyMatrix1.DisplaceRowOrColumn(0, j);
                        j--; Loop--;
                    }
                }
            }
            return copyMatrix1;
        }
        private Matrix DisplaceRowOrColumn(int i, int j)
        {
            double[,] copyMatrix;
            if (i == 0 && j == 0) copyMatrix = new double[this.matrix.GetLength(0), this.matrix.GetLength(1)];
            else if (i == 0 && j > 0) copyMatrix = new double[this.matrix.GetLength(0), this.matrix.GetLength(1) - 1];
            else if (j == 0 && i > 0) copyMatrix = new double[this.matrix.GetLength(0) - 1, this.matrix.GetLength(1)];
            else copyMatrix = new double[this.matrix.GetLength(0) - 1, this.matrix.GetLength(1) - 1];
            int m = 0, n = 0;
            for (int x = 0; x < this.matrix.GetLength(0); x++)
            {
                if (x != i - 1)
                {
                    n = 0;
                    for (int y = 0; y < this.matrix.GetLength(1); y++)
                    {
                        if (y != j - 1)
                        {
                            copyMatrix[m, n] = this.matrix[x, y];
                            n++;
                        }
                    }
                    m++;
                }
            }
            return new Matrix(copyMatrix);
        }
        

        #endregion

        public class Column
        {
            private double[] columnMatrix;
            public double GetValue(int j)
            {
                if (j <= 0 || j > this.columnMatrix.GetLength(0))
                    return double.NaN;
                return this.columnMatrix[j - 1];
            }
            public double this[int i] { set { this.columnMatrix[i - 1] = value; } }
            private enum ColumnMatrixConstructorOptions { Ones, Zeros }


            #region: Properties

            public int Length { get { return this.columnMatrix.Length; } }
            public bool IsZeros { get { return this.isZeros(); } }
            public bool IsNaN { get { return this.isNaN(); } }
            public bool IsSameElements { get { return this.isSameElements(); } }
            public bool IsOnes { get { return this.isOnes(); } }

            #endregion

            #region: Constructors

            public Column()
            {
                this.columnMatrix = new double[1] { double.NaN };
            }
            public Column(int Length)
            {
                this.columnMatrix = new double[Length];
                for (int i = 0; i < Length; i++) { this.columnMatrix[i] = double.NaN; }
            }
            public Column(params double[] Elements)
            {
                this.columnMatrix = new double[Elements.Length];
                for (int i = 0; i < Elements.Length; i++) { this.columnMatrix[i] = Elements[i]; }
            }
            private Column(int Length, ColumnMatrixConstructorOptions options)
            {
                this.columnMatrix = new double[Length];
                if (options == ColumnMatrixConstructorOptions.Ones)
                    for (int i = 0; i < Length; i++) { this.columnMatrix[i] = 1; }
                else if (options == ColumnMatrixConstructorOptions.Zeros)
                    for (int i = 0; i < Length; i++) { this.columnMatrix[i] = 0; }
            }

            #endregion

            #region: Static Constructors Method

            public static Column Zeros(int Length)
            {
                return new Column(Length, ColumnMatrixConstructorOptions.Zeros);
            }
            public static Column Ones(int Length)
            {
                return new Column(Length, ColumnMatrixConstructorOptions.Ones);
            }

            #endregion

            #region: Operational Methods

            public Row Transpose()
            {
                double[] RMArray = new double[this.Length];
                for (int i = 0; i < this.Length; i++) RMArray[i] = this.GetValue(i + 1);
                return new Row(RMArray);
            }
            public Matrix MultiplyBy(Row rowMatrix)
            {
                if (this.Length == rowMatrix.Length)
                {
                    double[,] matrix = new double[this.Length, this.Length];
                    for (int i = 0; i < this.Length; i++)
                    {
                        for (int j = 0; j < this.Length; j++)
                        {
                            matrix[i, j] = this.GetValue(i + 1) * rowMatrix.GetValue(j + 1);
                        }
                    }
                    return new Matrix(matrix);
                }
                else return new Matrix(new double[0, 0]);
            }
            public Column Unit() 
            {
                double L = 0;
                for (int i = 1; i <= this.Length; i++)
                {
                    L += Math.Pow(this.GetValue(i), 2);
                }
                L = Math.Sqrt(L);
                return this * (1.0 / L);
            }

            #endregion

            #region: Characteristic Methods

            private bool isZeros()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1) != 0)
                        return false;
                return true;
            }
            private bool isNaN()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1).Equals(double.NaN))
                        return true;

                return false;
            }
            private bool isOnes()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1) != 1)
                        return false;
                return true;
            }
            private bool isSameElements()
            {
                for (int i = 0; i < this.Length - 1; i++)
                    if (this.GetValue(i + 2) != this.GetValue(1))
                        return false;
                return true;
            }

            #endregion

            #region: Operator Overloads

            public static Column operator +(Column CM1, Column CM2)
            {
                if (CM1.Length == CM2.Length)
                {
                    double[] copyCM = new double[CM1.Length];
                    for (int i = 0; i < CM1.Length; i++) copyCM[i] = CM1.GetValue(i + 1) + CM2.GetValue(i + 1);
                    return new Column(copyCM);
                }
                else return new Column(new double[0]);
            }
            public static Column operator -(Column CM1, Column CM2)
            {
                if (CM1.Length == CM2.Length)
                {
                    double[] copyCM = new double[CM1.Length];
                    for (int i = 0; i < CM1.Length; i++) copyCM[i] = CM1.GetValue(i + 1) - CM2.GetValue(i + 1);
                    return new Column(copyCM);
                }
                else return new Column(new double[0]);
            }
            public static Column operator *(Matrix M, Column CM)
            {
                double[] TempArray = new double[0];
                if (M.ColumnsCount == CM.Length)
                {
                    TempArray = new double[M.RowsCount];
                    double tempValue = 0;
                    for (int i = 1; i <= M.RowsCount; i++)
                    {
                        for (int j = 1; j <= M.ColumnsCount; j++)
                        {
                            tempValue += M.GetValue(i, j) * CM.GetValue(j);
                        }
                        TempArray[i - 1] = tempValue;
                        tempValue = 0;
                    }
                    return new Column(TempArray);
                }
                else return new Column(TempArray);
            }
            public static Column operator *(Column CM, double d) 
            {
                double[] array = new double[CM.Length];
                for (int i = 1; i <= CM.Length; i++)
                {
                    array[i - 1] = CM.GetValue(i) * d;
                }
                return new Column(array);
            }

            public static implicit operator Column(Vector V)
            {
                return new Column(V.X, V.Y, V.Z);
            }
            public static implicit operator Column(Quaternion Q)
            {
                return new Column(Q.W, Q.X, Q.Y, Q.Z);
            }


            #endregion

            #region: Converter Methods

            public override string ToString()
            {
                string result = "";
                for (int i = 0; i < this.Length; i++) result += this.GetValue(i + 1).ToString() + " \n";
                return result + "\n";
            }

           
            #endregion
        }
        public class Row
        {
            private double[] rowMatrix;
            public double GetValue(int i)
            {
                if (i <= 0 || i > this.rowMatrix.GetLength(0))
                    return double.NaN;
                return this.rowMatrix[i - 1];
            }
            public double this[int i] { set { this.rowMatrix[i - 1] = value; } }
            private enum RowMatrixConstructorOptions { Ones, Zeros }


            #region: Properties

            public int Length { get { return this.rowMatrix.Length; } }
            public bool IsZeros { get { return this.isZeros(); } }
            public bool IsNaN { get { return this.isNaN(); } }
            public bool IsSameElements { get { return this.isSameElements(); } }
            public bool IsOnes { get { return this.isOnes(); } }

            #endregion

            #region: Constructors

            public Row()
            {
                this.rowMatrix = new double[1] { double.NaN };
            }
            public Row(int Length)
            {
                this.rowMatrix = new double[Length];
                for (int i = 0; i < Length; i++) { this.rowMatrix[i] = double.NaN; }
            }
            public Row(params double[] Elements)
            {
                this.rowMatrix = new double[Elements.Length];
                for (int i = 0; i < Elements.Length; i++) { this.rowMatrix[i] = Elements[i]; }
            }
            private Row(int Length, RowMatrixConstructorOptions options)
            {
                this.rowMatrix = new double[Length];
                if (options == RowMatrixConstructorOptions.Ones)
                    for (int i = 0; i < Length; i++) { this.rowMatrix[i] = 1; }
                else if (options == RowMatrixConstructorOptions.Zeros)
                    for (int i = 0; i < Length; i++) { this.rowMatrix[i] = 0; }
            }

            #endregion

            #region: Static Constructors Method

            public static Row Zeros(int Length)
            {
                return new Row(Length, RowMatrixConstructorOptions.Zeros);
            }
            public static Row Ones(int Length)
            {
                return new Row(Length, RowMatrixConstructorOptions.Ones);
            }


            #endregion

            #region: Operational Methods

            public Column Transpose()
            {
                double[] CMArray = new double[this.Length];
                for (int i = 0; i < this.Length; i++) CMArray[i] = this.GetValue(i + 1);
                return new Column(CMArray);
            }
            public double MultiplyBy(Column columnMatrix)
            {
                if (this.Length == columnMatrix.Length)
                {
                    double product = 0;
                    for (int i = 0; i < this.Length; i++)
                    {
                        product += this.GetValue(i + 1) * columnMatrix.GetValue(i + 1);
                    }
                    return product;
                }
                else return double.NaN;

            }

            #endregion

            #region: Characteristic Methods

            private bool isZeros()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1) != 0)
                        return false;
                return true;
            }
            private bool isNaN()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1).Equals(double.NaN))
                        return true;

                return false;
            }
            private bool isOnes()
            {
                for (int i = 0; i < this.Length; i++)
                    if (this.GetValue(i + 1) != 1)
                        return false;
                return true;
            }
            private bool isSameElements()
            {
                for (int i = 0; i < this.Length - 1; i++)
                    if (this.GetValue(i + 2) != this.GetValue(1))
                        return false;
                return true;
            }

            #endregion

            #region: Operator Overloads

            public static Row operator +(Row RM1, Row RM2)
            {
                if (RM1.Length == RM2.Length)
                {
                    double[] copyRM = new double[RM1.Length];
                    for (int i = 0; i < RM1.Length; i++) copyRM[i] = RM1.GetValue(i + 1) + RM2.GetValue(i + 1);
                    return new Row(copyRM);
                }
                else return new Row(new double[0]);
            }
            public static Row operator -(Row RM1, Row RM2)
            {
                if (RM1.Length == RM2.Length)
                {
                    double[] copyRM = new double[RM1.Length];
                    for (int i = 0; i < RM1.Length; i++) copyRM[i] = RM1.GetValue(i + 1) - RM2.GetValue(i + 1);
                    return new Row(copyRM);
                }
                else return new Row(new double[0]);
            }
            public static Row operator *(Row RM, Matrix M)
            {
                if (RM.Length == M.RowsCount)
                {
                    double TempValue = 0;
                    double[] RMArray = new double[M.ColumnsCount];
                    for (int i = 1; i <= M.ColumnsCount; i++)
                    {
                        for (int j = 1; j <= M.RowsCount; j++)
                        {
                            TempValue += M.GetValue(j, i) * RM.GetValue(j);
                        }
                        RMArray[i - 1] = TempValue;
                        TempValue = 0;
                    }
                    return new Row(RMArray);
                }
                else return new Row(new double[0]);
            }

            public static implicit operator Row(Vector V)
            {
                return new Row(V.X, V.Y, V.Z);
            }
            public static implicit operator Row(Quaternion Q)
            {
                return new Row(Q.W, Q.X, Q.Y, Q.Z);
            }
            


            #endregion

            #region: Converter Methods

            public override string ToString()
            {
                string result = "";
                for (int i = 0; i < this.Length; i++) result += this.GetValue(i + 1).ToString() + " \t";
                return result + "\n";
            }

            #endregion

        }

    }  
}
#warning Eliminate yöntemi sıkıntılı.