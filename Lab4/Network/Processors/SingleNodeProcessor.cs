using System;
using System.Diagnostics;
using Lab4.Network.TimeProviders;

namespace Lab4.Network.Processors;

public class SingleNodeProcessor<T> : INetworkNodeProcessor<T>
{
    private bool _processing;

    private float _currentTime;

    private float _workingTime;

    private readonly IProcessingTimeProvider<T> _timeProvider;

    public float AverageLoad => _workingTime / _currentTime;

    public T? Current { get; private set; }

    public float CompletionTime { get; private set; }

    public SingleNodeProcessor(IProcessingTimeProvider<T> timeProvider)
    {
        _timeProvider = timeProvider;
        CompletionTime = float.PositiveInfinity;
    }

    public bool TryEnter(T item)
    {
        if (_processing)
            return false;
        
        float delay = _timeProvider.GetProcessingTime(item);
        Current = item;
        CompletionTime = _currentTime + delay;
        _workingTime += delay;
        
        _processing = true;
        return true;
    }

    public bool TryExit()
    {
        if (!_processing)
            return false;
        
        _processing = false;
        Current = default;
        CompletionTime = float.PositiveInfinity;
        return true;
    }

    public void CurrentTimeUpdated(float currentTime) => _currentTime = currentTime;

    public void DebugPrint()
    {
        Debug.WriteLine($"Processor average working load: {AverageLoad}");
    }
}
