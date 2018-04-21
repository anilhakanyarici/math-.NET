using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class Function
    {
        public static Matrix SeperateMultiplier(int number) 
        {
            List<double> Column1 = new List<double>();
            List<double> Column2 = new List<double>();
            for (int i = 1; true; i++) 
            {
                if ((number / i) * i == number) 
                {
                    Column1.Add(i);
                    Column2.Add(number / i);
                }
                if (number / i == 0) break;
            }
            int Loop;
            if (Column1.Count % 2 == 0) Loop = Column1.Count / 2;
            else Loop = (Column1.Count + 1) / 2;
            double[,] matrix = new double[Loop, 2];
            for (int i = 0; i < Loop; i++) 
            {
                matrix[i, 0] = Column1[i];
                matrix[i, 1] = Column2[i];
            }
            return new Matrix(matrix);
        }
        public static int Factorial(int number)
        {
            int result = 1;
            for (int i = 1; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }
        public static int Permutation(int firstNumber, int secondNumber)
        {
            if (firstNumber < secondNumber) return 0;
            else if (firstNumber == secondNumber) return Factorial(firstNumber);
            else return Factorial(firstNumber) / Factorial(firstNumber - secondNumber);
        }
        public static int Combination(int firstNumber, int secondNumber)
        {
            if (firstNumber == secondNumber) return 1;
            else return (Permutation(firstNumber, secondNumber) / Factorial(secondNumber));
        }

        public static double Derivative(Func<double, double> function, double apsis)
        {
            return (function.Invoke(apsis + Math.Pow(10, -10)) - function.Invoke(apsis)) / (Math.Pow(10, -10));
        }
    }
}
