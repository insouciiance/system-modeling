using System;
using Lab3.Network.Factories;
using Lab3.Network.Selectors;
using Lab3.Network.TimeProviders;

namespace Lab3.Network;

public class CreateNode<T> : NetworkNode<T>
{
    private float _completionTime;

    private readonly IJobFactory<T> _factory;
    
    private readonly IProcessingTimeProvider<T> _timeProvider;

    private readonly INetworkNodeSelector<T> _nodeSelector;

    private T _nextItem;

    public CreateNode(
        IJobFactory<T> factory,
        IProcessingTimeProvider<T> timeProvider,
        INetworkNodeSelector<T> nodeSelector,
        float completionTime = 0)
    {
        _factory = factory;
        _timeProvider = timeProvider;
        _nodeSelector = nodeSelector;

        _nextItem = _factory.Create();
        _completionTime = completionTime;
    }

    public override float GetCompletionTime() => _completionTime;

    public override void Enter(T item) => throw new NotSupportedException();

    public override void Exit()
    {
        base.Exit();

        T prev = _nextItem;

        _nextItem = _factory.Create();
        _completionTime = _currentTime + _timeProvider.GetProcessingTime(_nextItem);
        
        var nextNode = _nodeSelector.GetNext(ref prev);
        nextNode.Enter(prev);
    }

    public override void DebugPrint(bool verbose = false)
    {
        base.DebugPrint(verbose);
        Console.WriteLine($"Created items: {ProcessedCount}");
    }
}
