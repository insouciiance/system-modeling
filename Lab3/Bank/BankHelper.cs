using Lab3.Network;
using Lab3.Network.Processors;
using Lab3.Network.TimeProviders;

namespace Lab3.Bank;

public static class BankHelper
{
    public static NetworkModel CreateModel()
    {
        ExponentialTimeProvider windowTimeProvider = new(0.3f);

        ProcessNode window1 = new(new SingleNodeProcessor(windowTimeProvider), 3)
        {
            DebugName = "Window 1"
        };

        ProcessNode window2 = new(new SingleNodeProcessor(windowTimeProvider), 3)
        {
            DebugName = "Window 2"
        };

        BankWindowSelector windowSelector = new(window1, window2);

        ExponentialTimeProvider carTimeProvider = new(0.5f);

        CreateNode cars = new(carTimeProvider, windowSelector)
        {
            DebugName = "Cars"
        };

        NetworkModel model = new(cars, window1, window2);
        return model;
    }
}
