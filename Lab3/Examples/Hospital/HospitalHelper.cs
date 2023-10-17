using System;
using Lab3.Collections;
using Lab3.Network;
using Lab3.Network.Processors;
using Lab3.Network.Selectors;
using Lab3.Network.TimeProviders;
using BCL = System.Collections.Generic;

namespace Lab3.Examples.Hospital;

public static class HospitalHelper
{
    public static NetworkModel<Patient> CreateModel()
    {
        PatientStatsRecorder statRecorder = new();
        LabStatsRecorder labRecorder = new();

        PatientTimeProvider admissionsTimeProvider = new((PatientKind.Hospital, 15), (PatientKind.PreliminaryExaminationThenHospital, 40), (PatientKind.PreliminaryExamination, 30));
        PatientPrioritySelector prioritySelector = new();
        PriorityQueue<Patient, int> admissionsQueue = new(prioritySelector);
        CompositeNodeProcessor<Patient> admissionsProcessor = new(new SingleNodeProcessor<Patient>(admissionsTimeProvider), new SingleNodeProcessor<Patient>(admissionsTimeProvider));
        ProcessNode<Patient> admissions = new(admissionsProcessor, admissionsQueue)
        {
            DebugName = "Admissions"
        };

        admissions.OnEnter += statRecorder.RecordEnter;

        Queue<Patient> wardQueue = new();
        UniformTimeProvider<Patient> wardTimeProvider = new(3, 8);
        CompositeNodeProcessor<Patient> wardProcessor = new(new SingleNodeProcessor<Patient>(wardTimeProvider), new SingleNodeProcessor<Patient>(wardTimeProvider), new SingleNodeProcessor<Patient>(wardTimeProvider));
        ProcessNode<Patient> ward = new(wardProcessor, wardQueue)
        {
            DebugName = "Ward"
        };

        UniformTimeProvider<Patient> labTimeProvider = new(2, 5);
        ThrowingQueue<Patient> labQueue = new();
        PooledNodeProcessor<Patient> labProcessor = new(LabProcessors());
        ProcessNode<Patient> lab = new(labProcessor, labQueue)
        {
            DebugName = "Lab"
        };

        lab.OnEnter += (_, enterTime) => labRecorder.RecordEnter(enterTime);

        ErlangTimeProvider<Patient> registryTimeProvider = new(3, 4.5f);
        Queue<Patient> registryQueue = new();
        SingleNodeProcessor<Patient> registryProcessor = new(registryTimeProvider);
        ProcessNode<Patient> registry = new(registryProcessor, registryQueue)
        {
            DebugName = "Registry"
        };

        ErlangTimeProvider<Patient> waitingRoomProvider = new(2, 4);
        Queue<Patient> waitingRoomQueue = new();
        CompositeNodeProcessor<Patient> waitingRoomProcessor = new(new SingleNodeProcessor<Patient>(waitingRoomProvider), new SingleNodeProcessor<Patient>(waitingRoomProvider));
        ProcessNode<Patient> waitingRoom = new(waitingRoomProcessor, waitingRoomQueue)
        {
            DebugName = "Waiting Room"
        };

        PatientFactory patientFactory = new((PatientKind.Hospital, 0.5f), (PatientKind.PreliminaryExaminationThenHospital, 0.1f), (PatientKind.PreliminaryExamination, 0.4f));
        ExponentialTimeProvider<Patient> patientTimeProvider = new(15);
        ConstantNodeSelector<Patient> patientNodeSelector = new(admissions);
        CreateNode<Patient> patients = new(patientFactory, patientTimeProvider, patientNodeSelector)
        {
            DebugName = "Create"
        };

        DisposeNode<Patient> exit = new()
        {
            DebugName = "Dispose"
        };

        exit.OnEnter += statRecorder.RecordExit;

        admissions.NextNodeSelector = new PatientNodeSelector((PatientKind.Hospital, ward), (PatientKind.PreliminaryExaminationThenHospital, lab), (PatientKind.PreliminaryExamination, lab));
        ward.NextNodeSelector = new ConstantNodeSelector<Patient>(exit);
        lab.NextNodeSelector = new ConstantNodeSelector<Patient>(registry);
        registry.NextNodeSelector = new ConstantNodeSelector<Patient>(waitingRoom);
        waitingRoom.NextNodeSelector = new PatientNodeSelector((PatientKind.PreliminaryExaminationThenHospital, admissions), (PatientKind.PreliminaryExamination, exit))
        {
            Handler = (ref Patient item) =>
            {
                if (item.Kind == PatientKind.PreliminaryExaminationThenHospital)
                    item.Kind = PatientKind.Hospital;
            }
        };

        NetworkModel<Patient> model = new(patients, admissions, ward, lab, registry, waitingRoom, exit);

        model.DebugPrintExtra += () =>
        {
            float avgTimeType1 = statRecorder.GetAverageHospitalTime(PatientKind.Hospital);
            float avgTimeType2 = statRecorder.GetAverageHospitalTime(PatientKind.PreliminaryExaminationThenHospital);
            float avgTimeType3 = statRecorder.GetAverageHospitalTime(PatientKind.PreliminaryExamination);

            Console.WriteLine($"Average time in hospital for Type 1: {avgTimeType1}");
            Console.WriteLine($"Average time in hospital for Type 2: {avgTimeType2}");
            Console.WriteLine($"Average time in hospital for Type 3: {avgTimeType3}");
        };

        model.DebugPrintExtra += () =>
        {
            float averageLabDelta = labRecorder.AverageEnterDelta;
            Console.WriteLine($"Average enter time delta for Lab: {averageLabDelta}");
        };
        
        return model;

        BCL.IEnumerable<INetworkNodeProcessor<Patient>> LabProcessors()
        {
            while (true)
                yield return new SingleNodeProcessor<Patient>(labTimeProvider);
        }
    }
}
