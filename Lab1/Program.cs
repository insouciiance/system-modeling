using System;

namespace Lab1;

internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        NumberGenerator1 generator = new();

        NumberGeneratorProcessor processor = new(generator);
        processor.Run();
    }
}
