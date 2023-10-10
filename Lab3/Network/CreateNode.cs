using Lab3.Network.Selectors;
using Lab3.Network.TimeProviders;

namespace Lab3.Network;

public class CreateNode : NetworkNode
{
    private float _completionTime;

    private readonly IProcessingTimeProvider _timeProvider;

    private readonly INetworkNodeSelector _nodeSelector;

    public CreateNode(IProcessingTimeProvider timeProvider, INetworkNodeSelector nodeSelector)
    {
        _timeProvider = timeProvider;
        _nodeSelector = nodeSelector;
    }

    public override float GetCompletionTime() => _completionTime;

    public override void Enter() => throw new NotSupportedException();

    public override void Exit()
    {
        base.Exit();

        _completionTime = _currentTime + _timeProvider.GetProcessingTime();
        var nextNode = _nodeSelector.GetNext();
        nextNode.Enter();
    }

    public override void DebugPrint(bool verbose = false)
    {
        base.DebugPrint(verbose);
        Console.WriteLine($"Created items: {_processedCount}");
    }
}
