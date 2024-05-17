namespace BugLibrary
{
    public class Functions
    {
        public int ArrayIndexOutOfBounds(int n) 
        {
            int[] numbers = new int[4] { 1, 2, 3, 4 };
            
            if (n < 10) {
                return numbers[n];
            }

            return 0;
        }

        public int NumericRange(int n) 
        {
            int a = int.MaxValue;

            checked
            {
                return a + n;
            }
        }

        public bool Authentication(string password) 
        {
            string excpectedPassword = "1234";
            if (excpectedPassword.Equals(password))
            {
                throw new Exception("password:" + password);
            }
            return true;
        }

        public bool Cycle(int a)
        {
            while (a > 5)
            {
                a++;
                a--;
            }
            return true;
        }

        public bool Crash(string str)
        {
            char[] arr;
            arr = str.ToCharArray();
            if (arr.Length >= 5 && arr.Length <= 9)
                if (arr[0] == 'c')
                    if (arr[1] == 'r')
                        if (arr[2] == 'a')
                            if (arr[3] == 's')
                                if (arr[4] == 'h')
                                {
                                    arr[0] = arr[10];
                                    return false;
                                }
            return true;
        }
    }
}