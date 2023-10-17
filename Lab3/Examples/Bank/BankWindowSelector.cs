using Lab3.Network;
using Lab3.Network.Selectors;

namespace Lab3.Examples.Bank;

public class BankWindowSelector<T> : INetworkNodeSelector<T>
{
    private readonly ProcessNode<T> _node1;

    private readonly ProcessNode<T> _node2;

    public BankWindowSelector(ProcessNode<T> node1, ProcessNode<T> node2)
    {
        _node1 = node1;
        _node2 = node2;
    }

    public NetworkNode<T> GetNext(ref T _) => (_node1.Queue.Count - _node2.Queue.Count) switch
    {
        <= 0 => _node1,
        > 0 => _node2,
    };
}
