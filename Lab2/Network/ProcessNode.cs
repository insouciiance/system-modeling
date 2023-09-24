using System;
using System.Text;
using Lab2.Network.Processors;
using Lab2.Network.Selectors;

namespace Lab2.Network;

public class ProcessNode : NetworkNode
{
    private int _queueSize;

    private int _failures;

    private float _queueAverageDividend;

    private readonly int _maxQueueSize;

    private readonly INetworkNodeProcessor _nodeProcessor;

    public INetworkNodeSelector? NextNodeSelector { get; set; }

    public ProcessNode(INetworkNodeProcessor processor, int maxQueueSize)
    {
        _nodeProcessor = processor;
        _maxQueueSize = maxQueueSize;
    }

    public override float GetCompletionTime() => _nodeProcessor.CompletionTime;

    public override void Enter()
    {
        base.Enter();

        if (_nodeProcessor.TryEnter())
        {
            Console.WriteLine($"{DebugName} Started processing");
            return;
        }

        if (_queueSize < _maxQueueSize)
        {
            Console.WriteLine($"{DebugName} Queued an item");

            _queueSize++;
            return;
        }

        Console.WriteLine($"{DebugName} Failure!");
        _failures++;
    }

    public override void Exit()
    {
        base.Exit();

        _nodeProcessor.TryExit();

        if (NextNodeSelector is not null)
        {
            var nextNode = NextNodeSelector.GetNext();
            Console.WriteLine($"{DebugName} -> {nextNode.DebugName}");
            nextNode.Enter();
        }

        if (_queueSize > 0 && _nodeProcessor.TryEnter())
        {
            _queueSize--;
            Console.WriteLine($"{DebugName} Queue not empty, new item processing");
        }
    }

    public override void CurrentTimeUpdated(float currentTime)
    {
        _nodeProcessor.CurrentTimeUpdated(currentTime);

        float delta = currentTime - _currentTime;

        _queueAverageDividend += delta * _queueSize;

        base.CurrentTimeUpdated(currentTime);

    }

    public override void DebugPrint(bool verbose = false)
    {
        base.DebugPrint(verbose);

        StringBuilder prettyQueue = new();
        prettyQueue.Append(new string('*', _queueSize));

        if (_queueSize < _maxQueueSize)
            prettyQueue.Append(new string('.', _maxQueueSize - _queueSize));

        Console.WriteLine($"Queue size: {_queueSize} ({prettyQueue})");
        Console.WriteLine($"Failures: {_failures}");

        if (verbose)
        {
            Console.WriteLine($"Processed items: {_processedCount}");
            Console.WriteLine($"Average queue size: {_queueAverageDividend / _currentTime}");
            Console.WriteLine($"Failure probability: {(float)_failures / (_failures + _processedCount)}");
        }
    }
}
