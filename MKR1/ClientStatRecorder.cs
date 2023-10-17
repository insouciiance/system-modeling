namespace MKR1;

public class ClientStatRecorder
{
    private readonly Dictionary<Guid, float> _clientEntryTimes = new();

    private float _cumulativeTime;

    private int _clientCount;

    public float AverageClientTime => _cumulativeTime / _clientCount;

    public void RecordEnter(Client client, float time)
    {
        _clientEntryTimes.Add(client.Id, time);
    }

    public void RecordExit(Client client, float time)
    {
        float entryTime = _clientEntryTimes[client.Id];
        _cumulativeTime += time - entryTime;
        _clientCount++;
    }
}
