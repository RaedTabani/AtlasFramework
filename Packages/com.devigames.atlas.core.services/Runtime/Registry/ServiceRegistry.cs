using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Services
{
    public sealed class ServiceRegistry
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<TService>(TService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            Type serviceType = typeof(TService);

            if (_services.ContainsKey(serviceType))
            {
                throw new InvalidOperationException(
                    $"Service of type {serviceType.Name} is already registered.");
            }

            _services.Add(serviceType, service);
        }

        public bool TryResolve<TService>(out TService service)
        {
            if (_services.TryGetValue(typeof(TService), out object value))
            {
                service = (TService)value;
                return true;
            }

            service = default;
            return false;
        }

        public TService Resolve<TService>()
        {
            if (TryResolve(out TService service))
                return service;

            throw new InvalidOperationException(
                $"Service of type {typeof(TService).Name} is not registered.");
        }

        public bool IsRegistered<TService>()
        {
            return _services.ContainsKey(typeof(TService));
        }

        public void Unregister<TService>()
        {
            _services.Remove(typeof(TService));
        }

        public void Clear()
        {
            _services.Clear();
        }
    }
}