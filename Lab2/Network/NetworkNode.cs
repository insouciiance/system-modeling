using System;

namespace Lab2.Network;

public abstract class NetworkNode
{
    protected float _currentTime;

    protected int _processedCount;

    public string? DebugName { get; init; }

    public abstract float GetCompletionTime();

    public virtual void Enter() { }

    public virtual void Exit() => _processedCount++;

    public virtual void CurrentTimeUpdated(float currentTime) => _currentTime = currentTime;

    public virtual void DebugPrint(bool verbose = false) => Console.WriteLine(DebugName);
}
