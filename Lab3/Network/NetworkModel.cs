using System.Collections.Immutable;

namespace Lab3.Network;

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
            var nextNode = _nodes.MinBy(n => n.GetCompletionTime())!;
            _currentTime = nextNode.GetCompletionTime();

            Console.WriteLine("\n*************** Current Time Update ***************");
            Console.WriteLine($"Current time: {_currentTime}");

            foreach (var node in _nodes)
            {
                node.CurrentTimeUpdated(_currentTime);
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
    }
}
