using System;
using Lab2.Network.TimeProviders;

namespace Lab2.Network;

public class CreateNode : NetworkNode
{
    private float _completionTime;

    private readonly NetworkNode _nextNode;

    private readonly IProcessingTimeProvider _timeProvider;

    public override float CompletionTime => _completionTime;

    public CreateNode(IProcessingTimeProvider timeProvider, NetworkNode nextNode)
    {
        _timeProvider = timeProvider;
        _nextNode = nextNode;
    }

    public override void Begin() => throw new NotSupportedException();

    public override void End()
    {
        base.End();

        _completionTime = _currentTime + _timeProvider.GetProcessingTime();
        _nextNode.Begin();
    }
}
