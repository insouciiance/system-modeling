using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Lab3.Network.Processors;

public class CompositeNodeProcessor<T> : INetworkNodeProcessor<T>
{
    public T? Current => _nodes.MinBy(n => n.CompletionTime)!.Current;

    public float CompletionTime => _nodes.Min(n => n.CompletionTime);

    private readonly ImmutableArray<INetworkNodeProcessor<T>> _nodes;

    public CompositeNodeProcessor(params INetworkNodeProcessor<T>[] nodes)
        : this(nodes.ToImmutableArray()) { }

    public CompositeNodeProcessor(ImmutableArray<INetworkNodeProcessor<T>> nodes)
    {
        _nodes = nodes;
    }

    public bool TryEnter(T item)
    {
        foreach (var node in _nodes)
        {
            if (node.TryEnter(item))
                return true;
        }

        return false;
    }

    public bool TryExit()
    {
        foreach (var node in _nodes)
        {
            if (Math.Abs(node.CompletionTime - CompletionTime) < 0.0001f)
                return node.TryExit();
        }

        throw new UnreachableException();
    }

    public void CurrentTimeUpdated(float currentTime)
    {
        foreach (var node in _nodes)
            node.CurrentTimeUpdated(currentTime);
    }
}
