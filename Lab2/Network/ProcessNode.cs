using System;
using System.Text;
using Lab2.Network.Selectors;
using Lab2.Network.TimeProviders;

namespace Lab2.Network;

public class ProcessNode : NetworkNode
{
    private bool _processing;

    private int _queueSize;

    private int _failures;

    private float _completionTime;

    private readonly IProcessingTimeProvider _timeProvider;

    private readonly INetworkNodeSelector? _nextNodeSelector;

    private readonly int _maxQueueSize;

    public override float CompletionTime => _completionTime;

    public ProcessNode(IProcessingTimeProvider timeProvider, int maxQueueSize, INetworkNodeSelector? nextNodeSelector = null)
    {
        _timeProvider = timeProvider;
        _maxQueueSize = maxQueueSize;
        _nextNodeSelector = nextNodeSelector;
        _completionTime = float.PositiveInfinity;
    }

    public override void Begin()
    {
        base.Begin();

        if (!_processing)
        {
            Console.WriteLine($"[{DebugName}] Started processing");

            _processing = true;
            _completionTime = _currentTime + _timeProvider.GetProcessingTime();
            return;
        }

        if (_queueSize < _maxQueueSize)
        {
            Console.WriteLine($"[{DebugName}] Queued an item");

            _queueSize++;
            return;
        }

        Console.WriteLine($"[{DebugName}] Failure!");
        _failures++;
    }

    public override void End()
    {
        base.End();

        _processing = false;
        _completionTime = float.PositiveInfinity;

        if (_nextNodeSelector is not null)
        {
            var nextNode = _nextNodeSelector.GetNext();
            Console.WriteLine($"[{DebugName}] -> [{nextNode.DebugName}]");
            nextNode.Begin();
        }

        if (_queueSize > 0)
        {
            Console.WriteLine($"[{DebugName}] Queue not empty, new item processing");

            _queueSize--;
            _processing = true;
            _completionTime = _currentTime + _timeProvider.GetProcessingTime();
        }
    }

    public override void DebugPrint()
    {
        base.DebugPrint();

        StringBuilder prettyQueue = new();
        prettyQueue.Append(new string('*', _queueSize));

        if (_queueSize < _maxQueueSize)
            prettyQueue.Append(new string('.', _maxQueueSize - _queueSize));

        Console.WriteLine($"Queue size: {_queueSize} ({prettyQueue})");
        Console.WriteLine($"Failures: {_failures}");
    }
}
