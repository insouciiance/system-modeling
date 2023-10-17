using System.Collections.Frozen;
using System.Collections.Generic;
using Lab3.Network;
using Lab3.Network.Selectors;

namespace Lab3.Examples.Hospital;

public class PatientNodeSelector : INetworkNodeSelector<Patient>
{
    private readonly FrozenDictionary<PatientKind, NetworkNode<Patient>> _nodes;

    public PatientHandler? Handler { get; init; }

    public PatientNodeSelector(params (PatientKind Kind, NetworkNode<Patient> Node)[] nodes)
    {
        _nodes = nodes.ToFrozenDictionary(x => x.Kind, x => x.Node);
    }

    public NetworkNode<Patient> GetNext(ref Patient item)
    {
        var node = _nodes[item.Kind];
        Handler?.Invoke(ref item);
        return node;
    }

    public delegate void PatientHandler(ref Patient item);
}
