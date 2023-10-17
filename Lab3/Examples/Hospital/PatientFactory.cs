using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Lab3.Network.Factories;

namespace Lab3.Examples.Hospital;

public class PatientFactory : IJobFactory<Patient>
{
    private readonly ImmutableArray<(PatientKind Kind, float Weight)> _weights;

    public PatientFactory(params (PatientKind Kind, float Weight)[] weights)
    {
        if (Math.Abs(weights.Sum(x => x.Weight) - 1) > 0.0001f)
            throw new ArgumentException(nameof(weights));

        _weights = weights.ToImmutableArray();
    }

    public Patient Create()
    {
        float acc = 0;
        float rndWeight = Random.Shared.NextSingle();

        foreach (var (kind, weight) in _weights)
        {
            acc += weight;

            if (rndWeight < acc)
                return new(kind);
        }

        throw new UnreachableException();
    }
}
