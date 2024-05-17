namespace Calculator.FuzzerTests
{
    public class Tests
    {
        public void SimpleOperations_Sum(string data)
        {
            // Stupid, but it worked
            var parts = data.Split(':');
            if (parts.Length != 2)
            {
                return;
            }

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.Sum(Int32.Parse(parts[0]), Int32.Parse(parts[1]));
        }

        public void SimpleOperations_Sub(string data)
        {
            // Stupid, but it worked
            var parts = data.Split(':');
            if (parts.Length != 2)
            {
                return;
            }

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.Sub(Int32.Parse(parts[0]), Int32.Parse(parts[1]));
        }

        public void SimpleOperations_Multiply(string data)
        {
            // Stupid, but it worked
            var parts = data.Split(':');
            if (parts.Length != 2)
            {
                return;
            }

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.Multiply(Int32.Parse(parts[0]), Int32.Parse(parts[1]));
        }

        public void SimpleOperations_Divide(string data)
        {
            // Stupid, but it worked
            var parts = data.Split(':');
            if (parts.Length != 2)
            {
                return;
            }

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.Divide(Int32.Parse(parts[0]), Int32.Parse(parts[1]));
        }

        public void SimpleOperations_DivideExtension(string data)
        {
            // Stupid, but it worked
            var parts = data.Split(':');
            if (parts.Length != 2)
            {
                return;
            }

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.DivideExtension(Int32.Parse(parts[0]), Int32.Parse(parts[1]));
        }

        public void SimpleOperations_Glitches(string data)
        {

            var simpleOperations = new SimpleOperations();

            _ = simpleOperations.Glitches(Int32.Parse(data));
        }
    }
}