namespace Lab4.Network.Selectors;

public class ConstantNodeSelector<T> : INetworkNodeSelector<T>
{
    private readonly NetworkNode<T> _node;

    public ConstantNodeSelector(NetworkNode<T> node) => _node = node;

    public NetworkNode<T> GetNext(ref T _) => _node;
}
