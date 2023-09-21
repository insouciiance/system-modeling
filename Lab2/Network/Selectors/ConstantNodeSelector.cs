namespace Lab2.Network.Selectors;

public class ConstantNodeSelector : INetworkNodeSelector
{
    private readonly NetworkNode _node;

    public ConstantNodeSelector(NetworkNode node) => _node = node;

    public NetworkNode GetNext() => _node;
}
