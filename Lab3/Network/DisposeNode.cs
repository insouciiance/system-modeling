using System;

namespace Lab3.Network;

public class DisposeNode<T> : NetworkNode<T>
{
    private float _deltaTimeSum;

    private float _previousEnterTime;

    public float AverageDeltaTime => _deltaTimeSum / ProcessedCount;

    public override float GetCompletionTime() => float.PositiveInfinity;

    public override void Enter(T item)
    {
        base.Enter(item);
        _deltaTimeSum += _currentTime - _previousEnterTime;
        _previousEnterTime = _currentTime;

        ProcessedCount++;
    }

    public override void Exit() => throw new InvalidOperationException();

    public override void DebugPrint(bool verbose = false)
    {
        if (verbose)
            Console.WriteLine($"Average delta time: {AverageDeltaTime}");

        base.DebugPrint(verbose);
    }
}
