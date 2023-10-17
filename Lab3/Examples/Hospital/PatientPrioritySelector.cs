using Lab3.Collections;

namespace Lab3.Examples.Hospital;

public class PatientPrioritySelector : IPrioritySelector<Patient, int>
{
    public int GetPriority(Patient item) => item.Kind switch
    {
        PatientKind.Hospital => 1,
        _ => 2
    };
}
