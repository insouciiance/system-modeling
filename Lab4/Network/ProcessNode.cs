using System;
using System.Diagnostics;
using System.Text;
using Lab4.Collections;
using Lab4.Network.Processors;
using Lab4.Network.Selectors;

namespace Lab4.Network;

public class ProcessNode<T> : NetworkNode<T>
{
    private readonly INetworkNodeProcessor<T> _nodeProcessor;

    public IQueue<T> Queue { get; }

    public INetworkNodeSelector<T>? NextNodeSelector { get; set; }

    public float QueueAverageSize => QueueWaitingTimeTotal / _currentTime;

    public float QueueWaitingTimeTotal { get; private set; }

    public int FailuresCount { get; private set; }

    public ProcessNode(INetworkNodeProcessor<T> processor, IQueue<T> queue)
    {
        _nodeProcessor = processor;
        Queue = queue;
    }

    public override float GetCompletionTime() => _nodeProcessor.CompletionTime;

    public override void Enter(T item)
    {
        base.Enter(item);

        if (_nodeProcessor.TryEnter(item))
        {
            Debug.WriteLine($"{DebugName} Started processing");
            return;
        }

        if (Queue.TryEnqueue(item))
        {
            Debug.WriteLine($"{DebugName} Queued an item");
            return;
        }

        Debug.WriteLine($"{DebugName} Failure!");
        FailuresCount++;
    }

    public override void Exit()
    {
        base.Exit();

        var current = _nodeProcessor.Current;
        _nodeProcessor.TryExit();

        if (NextNodeSelector is not null)
        {
            Debug.Assert(current is not null);

            var nextNode = NextNodeSelector.GetNext(ref current);
            Debug.WriteLine($"{DebugName} -> {nextNode.DebugName}");
            nextNode.Enter(current);
        }

        if (Queue.TryPeek(out var item) && _nodeProcessor.TryEnter(item))
        {
            Debug.Assert(Queue.TryDequeue(out _));

            Debug.WriteLine($"{DebugName} Queue not empty, new item processing");
        }
    }

    public override void CurrentTimeUpdated(float currentTime)
    {
        _nodeProcessor.CurrentTimeUpdated(currentTime);

        float delta = currentTime - _currentTime;

        QueueWaitingTimeTotal += delta * Queue.Count;

        base.CurrentTimeUpdated(currentTime);
    }

    public override void DebugPrint(bool verbose = false)
    {
        base.DebugPrint(verbose);

        StringBuilder prettyQueue = new();
        prettyQueue.Append(new string('*', Queue.Count));

        Debug.WriteLine($"Queue size: {Queue.Count} ({prettyQueue})");
        Debug.WriteLine($"Failures: {FailuresCount}");

        if (verbose)
        {
            Debug.WriteLine($"Processed items: {ProcessedCount}");
            Debug.WriteLine($"Average queue size: {QueueAverageSize}");
            Debug.WriteLine($"Failure probability: {(float)FailuresCount / (FailuresCount + ProcessedCount)}");

            Queue.DebugPrint();
            _nodeProcessor.DebugPrint();
        }
    }
}
