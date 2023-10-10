using Lab3.Bank;
using Lab3.Network;

namespace Lab3;

internal class Program
{
    public static void Main(string[] args)
    {
        NetworkModel model = BankHelper.CreateModel();
        model.Simulate(100);
    }
}
