using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"ServiceLocator: {type.Name} is already registered. Overwriting.");
        }
        _services[type] = service;
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }
        throw new Exception($"ServiceLocator: Service {type.Name} not found. Did you forget to register it?");
    }

    public static void Clear()
    {
        _services.Clear();
    }
}
