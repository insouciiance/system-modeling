using System;

namespace Lab3.Network;

public abstract class NetworkNode<T>
{
    protected float _currentTime;

    public int ProcessedCount { get; protected set; }

    public string? DebugName { get; init; }

    public event Action<T, float>? OnEnter;

    public event Action<float>? OnExit;

    public abstract float GetCompletionTime();

    public virtual void Enter(T item)
    {
        OnEnter?.Invoke(item, _currentTime);
    }

    public virtual void Exit()
    {
        OnExit?.Invoke(_currentTime);
        ProcessedCount++;
    }

    public virtual void CurrentTimeUpdated(float currentTime) => _currentTime = currentTime;

    public virtual void DebugPrint(bool verbose = false)
    {
        Console.WriteLine(DebugName);
    }
}
