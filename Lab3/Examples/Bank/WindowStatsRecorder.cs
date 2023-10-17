namespace Lab3.Examples.Bank;

public class WindowTimeRecorder
{
    private float _prevTime;

    private float _cumulativeTime;

    private int _entersCount;

    public float AverageDelta => _cumulativeTime / _entersCount;

    public void RecordEnter(float enterTime)
    {
        _cumulativeTime += enterTime - _prevTime;
        _prevTime = enterTime;
        _entersCount++;
    }
}
