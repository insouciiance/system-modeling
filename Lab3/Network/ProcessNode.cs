using System;
using System.Diagnostics;
using System.Text;
using Lab3.Collections;
using Lab3.Network.Processors;
using Lab3.Network.Selectors;

namespace Lab3.Network;

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
            Console.WriteLine($"{DebugName} Started processing");
            return;
        }

        if (Queue.TryEnqueue(item))
        {
            Console.WriteLine($"{DebugName} Queued an item");
            return;
        }

        Console.WriteLine($"{DebugName} Failure!");
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
            Console.WriteLine($"{DebugName} -> {nextNode.DebugName}");
            nextNode.Enter(current);
        }

        if (Queue.TryPeek(out var item) && _nodeProcessor.TryEnter(item))
        {
            Debug.Assert(Queue.TryDequeue(out _));

            Console.WriteLine($"{DebugName} Queue not empty, new item processing");
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

        Console.WriteLine($"Queue size: {Queue.Count} ({prettyQueue})");
        Console.WriteLine($"Failures: {FailuresCount}");

        if (verbose)
        {
            Console.WriteLine($"Processed items: {ProcessedCount}");
            Console.WriteLine($"Average queue size: {QueueAverageSize}");
            Console.WriteLine($"Failure probability: {(float)FailuresCount / (FailuresCount + ProcessedCount)}");

            Queue.DebugPrint();
            _nodeProcessor.DebugPrint();
        }
    }
}
