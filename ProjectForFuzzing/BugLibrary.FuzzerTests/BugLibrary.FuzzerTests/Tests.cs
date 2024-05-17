namespace BugLibrary.FuzzerTests
{
    public class Tests
    {
        public void Tests_ArrayIndexOutOfBounds(string data)
        {

            var functions = new Functions();

            if (int.TryParse(data, out int n)) 
            {
                _ = functions.ArrayIndexOutOfBounds(n);
            }
        }

        public void Tests_NumericRange(string data)
        {

            var functions = new Functions();

            if (int.TryParse(data, out int n))
            {
                _ = functions.NumericRange(n);
            }
        }

        public void Tests_Authentication(string data)
        {
            var functions = new Functions();

            _ = functions.Authentication(data);
        }

        public void Tests_Cycle(string data)
        {
            var functions = new Functions();

            if (int.TryParse(data, out int n))
            {
                _ = functions.Cycle(n);
            }
        }

        public void Tests_Crash(string data) 
        {
            var functions = new Functions();

            _ = functions.Crash(data);
        }
    }
}