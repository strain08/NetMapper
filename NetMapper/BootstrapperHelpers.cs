using Avalonia.Controls;
using NetMapper.Attributes;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
    ///     Creates a control by autoresolving it's constructor with Splat.
    ///     <br>Requires View Class with a ctor marked with [ResolveThis]</br>
    /// </summary>
    /// <param name="resolver">Splat immutable resolver</param>
    /// <param name="classType">View class</param>
    /// <returns>Initialized control</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Control CreateControlWithConstructorInjection(this IReadonlyDependencyResolver resolver, Type classType)
    {
        object GetService(Type type)
        {
            if (resolver.GetService(type) is object obj)
                return obj;

            throw new InvalidOperationException(
                $"Unable to create required dependency of type {type.FullName}: IReadonlyDependencyResolver.GetService() returned null");
        }

        ConstructorInfo?
            resolveConstructor = null,
            ctorParamless = null,
            ctorWithAttr = null;

        var ctorArray = classType.GetConstructors();

        // throw if classType has no constructors
        if (ctorArray.Length == 0)
            throw new InvalidOperationException($"{classType.FullName} has no constructors.");

        // find constructors
        foreach (var ctor in ctorArray)
        {
            // if constructor has no params
            if (ctor.GetParameters().Any() == false)
                ctorParamless = ctor;            

            // locate constructor with attribute [ResolveThis]
            if (ctor.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(ResolveThis)) != null)            
                ctorWithAttr = ctor;            
        }

        // if it has no [ResolveThis] ctor but has a parameterless ctor
        if (ctorParamless is not null && ctorWithAttr is null)
            return (Control)Activator.CreateInstance(classType)!;

        // if it has a constructor with [ResolveThis]
        if (ctorWithAttr is not null)
            resolveConstructor = ctorWithAttr;

        // resolve constructor has not been initialized, throw
        if (resolveConstructor is null)
            throw new InvalidOperationException(
                $"{classType.FullName}: Can not find constructor with attribute [{typeof(ResolveThis).Name}]");

        // resolveConstructor not null
        IEnumerable<Type> types = resolveConstructor.GetParameters().Select(p => p.ParameterType).ToArray();
        if (Activator.CreateInstance(classType, types.Select(GetService).ToArray()) is Control c)
            return c;

        throw new InvalidOperationException(
            $"Unable to create required dependency of type {classType.FullName}: Activator.CreateInstance() returned null");
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