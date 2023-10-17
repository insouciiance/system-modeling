using System;
using System.Collections.Immutable;
using System.Linq;

namespace Lab3.Network;

public class NetworkModel<T>
{
    private float _currentTime;

    private readonly ImmutableArray<NetworkNode<T>> _nodes;

    public Action? DebugPrintExtra { get; set; }

    public NetworkModel(params NetworkNode<T>[] nodes)
        : this(nodes.ToImmutableArray())
    { }

    public NetworkModel(ImmutableArray<NetworkNode<T>> nodes)
    {
        _nodes = nodes;
    }

    public void Simulate(float simulationTime)
    {
        while (_currentTime < simulationTime)
        {
            var nextNode = _nodes.MinBy(n => n.GetCompletionTime())!;
            _currentTime = nextNode.GetCompletionTime();

            Console.WriteLine("\n*************** Current Time Update ***************");
            Console.WriteLine($"Updated from {nextNode.DebugName}");
            Console.WriteLine($"Current time: {_currentTime}");

            foreach (var node in _nodes)
            {
                node.CurrentTimeUpdated(_currentTime);
                
                Console.WriteLine();
                node.DebugPrint();
            }

            foreach (var node in _nodes)
            {
                if (Math.Abs(node.GetCompletionTime() - _currentTime) < .0001f)
                {
                    Console.WriteLine($"Calling {node.DebugName} Exit");
                    node.Exit();
                }
            }
        }

        Console.WriteLine("\n*************** Simulation Finished ***************");

        foreach (var node in _nodes)
        {
            Console.WriteLine();
            node.DebugPrint(true);
        }

        Console.WriteLine();
        DebugPrintExtra?.Invoke();
    }
}
