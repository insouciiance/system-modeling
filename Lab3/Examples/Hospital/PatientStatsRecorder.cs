using System.Collections.Generic;

namespace Lab3.Examples.Hospital;

public class PatientStatsRecorder
{
    private readonly Dictionary<Patient, float> _entryTimes = new();

    private readonly Dictionary<PatientKind, float> _totalTimeByKind = new();

    private readonly Dictionary<PatientKind, int> _countByKind = new();

    public void RecordEnter(Patient patient, float entryTime)
    {
        _entryTimes.TryAdd(patient, entryTime);
    }

    public void RecordExit(Patient patient, float exitTime)
    {
        _entryTimes.Remove(patient, out float entryTime);
        float hospitalTime = exitTime - entryTime;

        _totalTimeByKind[patient.OriginalKind] = _totalTimeByKind.GetValueOrDefault(patient.OriginalKind) + hospitalTime;
        _countByKind[patient.OriginalKind] = _countByKind.GetValueOrDefault(patient.OriginalKind) + 1;
    }

    public float GetAverageHospitalTime(PatientKind kind)
    {
        var totalTime = _totalTimeByKind[kind];
        var count = _countByKind[kind];

        return totalTime / count;
    }
}
