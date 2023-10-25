namespace Lab4.Network.Selectors;

public interface INetworkNodeSelector<T>
{
    NetworkNode<T> GetNext(ref T item);
}
