namespace Lab3.Network;

public abstract class NetworkNode
{
    protected float _currentTime;

    protected int _processedCount;

    public event Action? OnEnter;

    public event Action? OnExit;

    public string? DebugName { get; init; }

    public abstract float GetCompletionTime();

    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    public virtual void Exit()
    {
        OnExit?.Invoke();
        _processedCount++;
    }

    public virtual void CurrentTimeUpdated(float currentTime) => _currentTime = currentTime;

    public virtual void DebugPrint(bool verbose = false) => Console.WriteLine(DebugName);
}
