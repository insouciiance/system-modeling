using System.Collections.Immutable;

namespace Lab3.Network.Selectors;

public class PriorityNodeSelector : INetworkNodeSelector
{
    private readonly ImmutableArray<(NetworkNode Node, float Priority)> _nodes;

    public PriorityNodeSelector(params (NetworkNode Node, float Priority)[] nodes)
        : this(nodes.ToImmutableArray())
    { }

    public PriorityNodeSelector(ImmutableArray<(NetworkNode Node, float Priority)> nodes)
    {
        _nodes = nodes;
    }


    public NetworkNode GetNext()
    {
        throw new NotImplementedException();
    }
}
