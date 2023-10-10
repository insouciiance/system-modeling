using Lab3.Network;
using Lab3.Network.Selectors;

namespace Lab3.Bank;

public class BankWindowSelector : INetworkNodeSelector
{
    private readonly ProcessNode _node1;

    private readonly ProcessNode _node2;

    public BankWindowSelector(ProcessNode node1, ProcessNode node2)
    {
        _node1 = node1;
        _node2 = node2;

        _node1.OnExit += () => OnExitQueueCheck(_node1, _node2);
        _node2.OnExit += () => OnExitQueueCheck(_node2, _node1);
    }

    public NetworkNode GetNext() => (_node1.QueueSize - _node2.QueueSize) switch
    {
        <= 0 => _node1,
        > 0 => _node2,
    };

    private void OnExitQueueCheck(ProcessNode target, ProcessNode other)
    {
        if (target.QueueSize - other.QueueSize >= 2)
        {
            Console.WriteLine($"Moving from {target.DebugName} to {other.DebugName}");

            target.QueueSize--;
            other.QueueSize++;
        }
    }
}
