using System;

namespace Hallo.Component.Logic.Reactive
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent sampleEvent);
        IObservable<TEvent> GetEvent<TEvent>();
    }
}