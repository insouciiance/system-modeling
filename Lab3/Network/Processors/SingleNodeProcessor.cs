using Lab3.Network.TimeProviders;

namespace Lab3.Network.Processors;

public class SingleNodeProcessor : INetworkNodeProcessor
{
    public float CompletionTime { get; private set; }
 
    private bool _processing;

    private float _currentTime;

    private readonly IProcessingTimeProvider _timeProvider;

    public SingleNodeProcessor(IProcessingTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        CompletionTime = float.PositiveInfinity;
    }

    public bool TryEnter()
    {
        if (_processing)
            return false;
        
        _processing = true;
        CompletionTime = _currentTime + _timeProvider.GetProcessingTime();
        return true;
    }

    public bool TryExit()
    {
        if (!_processing)
            return false;
        
        _processing = false;
        CompletionTime = float.PositiveInfinity;
        return true;
    }

    public void CurrentTimeUpdated(float currentTime) => _currentTime = currentTime;
}
