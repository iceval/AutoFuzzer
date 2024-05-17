using CalculatorExtension;

namespace Calculator
{
    public class SimpleOperations
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }

        public int Sub(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Divide(int a, int b)
        {
            return a / b; ;
        }

        public int DivideExtension(int a, int b)
        {
            var newOperations = new NewOperations();

            return newOperations.DivideWithSum(a,b);
        }

        public int Glitches(int a)
        {
            while (a == 12) {
                
            }

            if (a == 0) {
                a = 1 / a;
            }

            return a;
        }
    }
}