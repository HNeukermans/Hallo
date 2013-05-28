using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Hallo.Component.Logic;

namespace Hallo.Component.Logic.Reactive
{
    public class EventAggregator : IEventAggregator
    {
        private static readonly ConcurrentDictionary<Type, object> subjects
            = new ConcurrentDictionary<Type, object>();

        private static EventAggregator _instance = null;

        private EventAggregator()
        {
        }

        public static EventAggregator Instance
        {
            get
            { 
                if(_instance == null) _instance = new EventAggregator();
                return _instance;
            }
        }

        public IObservable<TEvent> GetEvent<TEvent>()
        {
            var subject =
                (ISubject<TEvent>)subjects.GetOrAdd(typeof(TEvent),
                                                    t => new Subject<TEvent>());
            return subject.AsObservable();
        }

        public void Publish<TEvent>(TEvent sampleEvent)
        {
            object subject;
            if (subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>)subject)
                    .OnNext(sampleEvent);
            }
        }
    }
}