namespace Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Example");
            UniqueID.Example();
            Console.WriteLine("Benchmark");
            UniqueID.Start();

            Console.ReadLine();
        }
    }
}
