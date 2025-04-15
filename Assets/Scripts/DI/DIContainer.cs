using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DI
{
    public class DIContainer
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIRegistration> _registations = new();
        private readonly HashSet<(string, Type)> _resolutions = new();

        public DIContainer(DIContainer parentContainer)
        {
            _parentContainer = parentContainer;
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(null, factory);
        }

        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, true);
        }

        public void RegisterTransient<T>(Func<DIContainer, T> factory)
        {
            RegisterTransient(null, factory);
        }

        public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            Register(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }

        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof (T));

            if (_registations.ContainsKey(key))
                throw new Exception($"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registered");

            _registations[key] = new DIRegistration
            {
                Instance = instance,
                IsSingleton = true
            };
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resolutions.Contains(key))
            {
                throw new Exception($"DI: Cyclic dependency for tag {key.Item1} and type {key.Item2.FullName}");
            }

            _resolutions.Add(key);

            try 
            {
                if (_registations.TryGetValue(key, out var registation))
                {
                    if (registation.IsSingleton)
                    {
                        if (registation.Instance == null && registation.Factory != null)
                            registation.Instance = registation.Factory(this);

                        return (T)registation.Instance;
                    }

                    return (T)registation.Factory(this);
                }

                if (_parentContainer != null)
                {
                    return _parentContainer.Resolve<T>(tag);
                }
            }
            finally
            {
                _resolutions.Remove(key);
            }

            throw new Exception($"DI: Couldn't find dependency for tag {key.Item1} and type {key.Item2.FullName}");
        }

        private void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
        {
            if (_registations.ContainsKey(key))
                throw new Exception($"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registered");

            _registations[key] = new DIRegistration
            {
                Factory = c => factory(c),
                IsSingleton = isSingleton
            };
        }
    }
}