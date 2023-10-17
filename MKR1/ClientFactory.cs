using Lab3.Network.Factories;

namespace MKR1;

public class ClientFactory : IJobFactory<Client>
{
    public Client Create() => new();
}
