using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class ExtensionMethods
{
    public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
            action(item);
    }
}

public interface IHandle { }

public interface IHandle<TMessage> : IHandle
{
    void Handle(TMessage message);
}

public interface IEventAggregator
{

    Action<System.Action> PublicationThreadMarshaller { get; set; }
    void Subscribe(object instace);
    void Unsubscribe(object instance);
    void Publish(object message);
    void Publish(object message, Action<System.Action> marshal); 
}

public class EventAggregator : IEventAggregator
{
    protected readonly List<Handler> handlers = new List<Handler>();

    public static Action<System.Action> DefaultPublicationThreadMarshaller = action => action();

    public static Action<object, object> HandlerResultProcessing = (target, result) => { };

    public Action<System.Action> PublicationThreadMarshaller { get; set; }

    public EventAggregator()
    {
        PublicationThreadMarshaller = DefaultPublicationThreadMarshaller;
    }

    public virtual void Subscribe(object instance)
    {
        lock (handlers)
        {
            if (handlers.Any(x => x.Matches(instance)))
                return;
            handlers.Add(new Handler(instance));
        }
    }

    public bool isSubscribed(object instance)
    {
        lock (handlers)
        {
            if (handlers.Any(x => x.Matches(instance)))
                return true;
        }
        return false;
    }

    public virtual void UnsubscribeAllHandlers()
    {
        lock (handlers)
            handlers.Clear();
    }

    public virtual bool HandlesMessage(object message)
    {
        lock (handlers)
        {
            if (handlers.Any(x => x.HasHandler(message)))
                return true;
        }
        return false;
    }

    public virtual void Unsubscribe(object instance)
    {
        lock (handlers)
        {
            var found = handlers.FirstOrDefault(x => x.Matches(instance));
            if (found != null)
                handlers.Remove(found);
        }
    }

    public virtual void Publish(object message)
    {
        Publish(message, PublicationThreadMarshaller);
    }

    public virtual void Publish(object message, Action<System.Action> marshal)
    {
        Handler[] toNotify;
        lock (handlers)
            toNotify = handlers.ToArray();
        marshal(() =>
            {
                var messageType = message.GetType();

                var dead = toNotify.Where(handler => !handler.Handle(messageType, message)).ToList();

                if (dead.Any())
                    lock (handlers)
                        dead.Apply(x => handlers.Remove(x));

            });
    }

    public class Handler
    {
        readonly WeakReference reference;
        readonly Dictionary<Type, MethodInfo> supportedHandlers = new Dictionary<Type, MethodInfo>();

        public Handler(object handler)
        {
            reference = new WeakReference(handler);
            var interfaces = handler.GetType().GetInterfaces().Where(x => typeof(IHandle).IsAssignableFrom(x) && x.IsGenericType);

            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                var method = @interface.GetMethod("Handle");
                supportedHandlers[type] = method;
            }
        }

        public bool HasHandler(object message)
        {
            foreach (Type type in supportedHandlers.Keys)
            {
                if (type.IsAssignableFrom(message.GetType()))
                    return true;
            }
            return false;
        }

        public bool Matches(object instance)
        {
            return reference.Target == instance;
        }

        public bool Handle(Type messageType, object message)
        {
            var target = reference.Target;
            if (target == null)
                return false;

            foreach (var pair in supportedHandlers)
            {
                if (pair.Key.IsAssignableFrom(messageType))
                {
                    var result = pair.Value.Invoke(target, new[] { message });
                    if (result != null)
                    {
                        HandlerResultProcessing(target, result);
                    }
                    return true;
                }
            }
            return true;
        }

    }
}

public class EventAggregatorManager
{
    private static object m_Lock = new object();

    private static List<EventAggregator> m_EventAggregatorList = new List<EventAggregator>();

    public static void AddEventAggregator(EventAggregator eventAggregator)
    {
        lock (m_Lock)
        {
            if (!m_EventAggregatorList.Contains(eventAggregator))
                m_EventAggregatorList.Add(eventAggregator);
        }
    }

    public static void RemoveEventAggregator(EventAggregator eventAggregator)
    {
        lock (m_Lock)
        {
            if (m_EventAggregatorList.Contains(eventAggregator))
            {
                eventAggregator.UnsubscribeAllHandlers();
                m_EventAggregatorList.Remove(eventAggregator);
            }
        }
    }

    public static void Publish(object message)
    {
        lock (m_Lock)
        {
            foreach (EventAggregator Event in m_EventAggregatorList)
            {
                if (Event.HandlesMessage(message))
                    Event.Publish(message);
            }
        }
    }

}

public class UTEventAggregator : EventAggregator
{
    List<object> m_Messages;

    public UTEventAggregator()
    {
        m_Messages = new List<object>();
    }

    public override void Publish(object message)
    {
        lock (m_Messages)
            m_Messages.Add(message);
    }

    public virtual void Update()
    {
        Handler[] toNotify;
        lock (handlers)
            toNotify = handlers.ToArray();

        List<object> WorkingList;

        lock (m_Messages)
        {
            WorkingList = new List<object>(m_Messages);
            m_Messages.Clear();
        }
        while (WorkingList.Count > 0)
        {
            object message = WorkingList[0];
            Type messageType = message.GetType();

            List<Handler> dead = toNotify.Where(handler => !handler.Handle(messageType, message)).ToList();

            if (dead.Any())
            {
                lock (handlers)
                    dead.Apply(x => handlers.Remove(x));
            }
            WorkingList.RemoveAt(0);
        }
    }
}

public class GameEventAggregator : UTEventAggregator
{
    public GameEventAggregator() { }
    public static GameEventAggregator GameMessenger = new GameEventAggregator();
}