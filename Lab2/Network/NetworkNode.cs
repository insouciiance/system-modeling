using System;

namespace Lab2.Network;

public abstract class NetworkNode
{
    protected float _currentTime;

    public string? DebugName { get; init; }

    public abstract float CompletionTime { get; }

    public virtual void Begin()
    {
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine($"[{DebugName}] Begin");
        DebugPrint();
    }

    public virtual void End()
    {
        Console.WriteLine($"[{DebugName}] End");
    }

    public virtual void CurrentTimeUpdated(float currentTime)
    {
        _currentTime = currentTime;
    }

    public virtual void DebugPrint()
    {
        Console.WriteLine($"[{DebugName}]");
    }
}
