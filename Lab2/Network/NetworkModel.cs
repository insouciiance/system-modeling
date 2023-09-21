using System;
using System.Collections.Immutable;
using System.Linq;

namespace Lab2.Network;

public class NetworkModel
{
    private float _currentTime;

    private readonly ImmutableArray<NetworkNode> _nodes;

    public NetworkModel(params NetworkNode[] nodes)
        : this(nodes.ToImmutableArray())
    { }

    public NetworkModel(ImmutableArray<NetworkNode> nodes)
    {
        _nodes = nodes;
    }

    public void Simulate(float simulationTime)
    {
        while (_currentTime < simulationTime)
        {
            var nextNode = _nodes.MinBy(n => n.CompletionTime)!;
            _currentTime = nextNode.CompletionTime;

            Console.WriteLine("\n*************** Current Time Update ***************");
            Console.WriteLine($"Current time: {_currentTime}");

            foreach (var node in _nodes)
                node.CurrentTimeUpdated(_currentTime);

            foreach (var node in _nodes)
            {
                if (Math.Abs(node.CompletionTime - _currentTime) < .0001f)
                    node.End();
            }
        }

        Console.WriteLine("\n*************** Simulation Finished ***************");

        foreach (var node in _nodes)
        {
            Console.WriteLine();
            node.DebugPrint();
        }
    }
}
