using System;
using Lab2.Network.TimeProviders;

namespace Lab2.Network;

public class CreateNode : NetworkNode
{
    private float _completionTime;

    private readonly NetworkNode _nextNode;

    private readonly IProcessingTimeProvider _timeProvider;

    public CreateNode(IProcessingTimeProvider timeProvider, NetworkNode nextNode)
    {
        _timeProvider = timeProvider;
        _nextNode = nextNode;
    }

    public override float GetCompletionTime() => _completionTime;

    public override void Enter() => throw new NotSupportedException();

    public override void Exit()
    {
        base.Exit();

        _completionTime = _currentTime + _timeProvider.GetProcessingTime();
        _nextNode.Enter();
    }

    public override void DebugPrint(bool verbose = false)
    {
        base.DebugPrint(verbose);
        Console.WriteLine($"Created items: {_processedCount}");
    }
}
