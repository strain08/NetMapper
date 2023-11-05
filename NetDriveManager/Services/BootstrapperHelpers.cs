using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetMapper.Attributes;
using Splat;

internal static class BootstrapperHelpers
{
    /// <summary>
    ///     Creates a new class, autoresolving it's constructor with Splat
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resolver"></param>
    /// <returns>Created class</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T CreateWithConstructorInjection<T>(this IReadonlyDependencyResolver resolver) where T : class
    {
        object GetService(Type type)
        {
            if (resolver.GetService(type) is object obj)
                return obj;

            throw new InvalidOperationException(
                $"Unable to create required dependency of type {type.FullName}: IReadonlyDependencyResolver.GetService() returned null");
        }

        ConstructorInfo? resolveConstructor = null;
        var constructors = typeof(T).GetConstructors();

        if (constructors.Length == 0)
            throw new InvalidOperationException($"{typeof(T).FullName} has no constructors.");

        // locate constructor with attribute [ResolveThis]
        foreach (var constructor in constructors)
        {
            var customAttributes = constructor.CustomAttributes;
            if (customAttributes.Any())
                if (customAttributes.Where(a => a.AttributeType == typeof(ResolveThis)).FirstOrDefault() != null)
                {
                    resolveConstructor = constructor;
                    break;
                }
        }

        if (resolveConstructor == null)
            throw new InvalidOperationException(
                $"{typeof(T).FullName}: Can not find constructor with attribute [{typeof(ResolveThis).Name}]");

        // resolveConstructor not null
        IEnumerable<Type> types = resolveConstructor.GetParameters().Select(p => p.ParameterType).ToArray();
        if (Activator.CreateInstance(typeof(T), types.Select(GetService).ToArray()) is T t)
            return t;

        throw new InvalidOperationException(
            $"Unable to create required dependency of type {typeof(T).FullName}: Activator.CreateInstance() returned null");
    }

    /// <summary>
    ///     Returns a service or throws
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="resolver"></param>
    /// <returns>class</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
        where TService : class
    {
        var service = resolver.GetService<TService>() ??
                      throw new InvalidOperationException(
                          $"Splat failed to resolve object of type {typeof(TService)}"); // throw error with detailed description

        return service; // return instance if not null
    }
}