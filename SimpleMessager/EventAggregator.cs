using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimpleMessager;

public class EventAggregator
{
    record ConditionalWeak<TKey, TValue>(TKey Key, TValue Value)
        where TKey : class
        where TValue : class;

    public delegate void MessageHandler<in TRecipient, in TMessage>(TRecipient recipient, TMessage message)
        where TRecipient : class
        where TMessage : class;

    public static EventAggregator Instance { get; } = new();

    private readonly Dictionary<Type, List<ConditionalWeak<object, object>>> _recipientsMap = new();

    public void Register<TRecipient, TMessage>(TRecipient recipient, MessageHandler<TRecipient, TMessage> handler)
        where TRecipient : class
        where TMessage : class
    {
        var type = typeof(TMessage);

        if (!_recipientsMap.ContainsKey(type))
        {
            _recipientsMap[type] = new();
        }

        // 添加处理程序到列表中
        _recipientsMap[type].Add(new ConditionalWeak<object, object>(recipient, handler));
    }


    public void Send<TMessage>(TMessage message) where TMessage : class
    {
        var type = typeof(TMessage);
        if (!_recipientsMap.TryGetValue(type, out var value))
        {
            return;
        }

        foreach (var rec in value)
        {
            if (message != null)
            {
                var handler = (MessageHandler<object, TMessage>)rec.Value;
                handler.Invoke(rec.Key,message);
                
            }
        }
    }
}