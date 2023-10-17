using System;

namespace Lab3.Examples.Hospital;

public class Patient : IEquatable<Patient>
{
    public Guid Id { get; } = Guid.NewGuid();

    public PatientKind OriginalKind { get; }

    public PatientKind Kind { get; set; }

    public Patient(PatientKind kind)
    {
        OriginalKind = kind;
        Kind = kind;
    }

    public bool Equals(Patient? other)
    {
        if (other is null)
            return false;

        return ReferenceEquals(this, other) || Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        return ReferenceEquals(this, obj) || Equals(obj as Patient);
    }

    public override int GetHashCode() => Id.GetHashCode();
}
