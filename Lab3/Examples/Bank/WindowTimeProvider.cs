using Lab3.Network.TimeProviders;

namespace Lab3.Examples.Bank;

public class WindowTimeProvider : IProcessingTimeProvider<int>
{
    private readonly IProcessingTimeProvider<int> _next;

    private IProcessingTimeProvider<int> _activeProvider;

    public WindowTimeProvider(IProcessingTimeProvider<int> initial, IProcessingTimeProvider<int> next)
    {
        _activeProvider = initial;
        _next = next;
    }

    public float GetProcessingTime(int item)
    {
        float time = _activeProvider.GetProcessingTime(item);
        _activeProvider = _next;
        return time;
    }
}
