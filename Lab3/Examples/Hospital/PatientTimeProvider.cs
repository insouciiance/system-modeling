using System.Collections.Frozen;
using Lab3.Network.TimeProviders;

namespace Lab3.Examples.Hospital;

public class PatientTimeProvider : IProcessingTimeProvider<Patient>
{
    private readonly FrozenDictionary<PatientKind, float> _times;

    public PatientTimeProvider(params (PatientKind Kind, float Time)[] times)
    {
        _times = times.ToFrozenDictionary(x => x.Kind, x => x.Time);
    }

    public float GetProcessingTime(Patient item) => _times[item.Kind];
}
