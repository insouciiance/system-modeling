namespace Lab3.Examples.Hospital;

public class LabStatsRecorder
{
    private float _prevTime;

    private float _cumulativeTime;

    private int _entersCount;

    public float AverageEnterDelta => _cumulativeTime / _entersCount;

    public void RecordEnter(float enterTime)
    {
        _cumulativeTime += enterTime - _prevTime;
        _prevTime = enterTime;
        _entersCount++;
    }
}
