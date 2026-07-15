using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;

namespace DeviGames.Atlas.Core.Services
{
    public sealed class ServiceRegistry
    {
        private readonly Dictionary<Type, object> _services = new();
        private readonly List<object> _registrationOrder = new();
        private readonly List<object> _initializedServices = new();

        public bool IsInitialized { get; private set; }

        public IReadOnlyCollection<Type> RegisteredTypes =>
            _services.Keys;
        
        public IReadOnlyCollection<object> InitializedServices =>
            _initializedServices;
        public void Register<TService>(TService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            if (IsInitialized)
            {
                throw new InvalidOperationException(
                    "Services cannot be registered after initialization.");
            }

            Type serviceType = typeof(TService);

            if (_services.ContainsKey(serviceType))
            {
                throw new InvalidOperationException(
                    $"Service '{serviceType.Name}' is already registered.");
            }

            if (ContainsInstance(service))
            {
                throw new InvalidOperationException(
                    $"The service instance '{service.GetType().Name}' " +
                    "is already registered under another type.");
            }

            _services.Add(serviceType, service);
            _registrationOrder.Add(service);
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
                $"Service '{typeof(TService).Name}' is not registered.");
        }

        public bool IsRegistered<TService>()
        {
            return _services.ContainsKey(typeof(TService));
        }

        public void Unregister<TService>()
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException(
                    "Services cannot be unregistered after initialization.");
            }

            Type serviceType = typeof(TService);

            if (!_services.TryGetValue(serviceType, out object service))
                return;

            _services.Remove(serviceType);
            _registrationOrder.Remove(service);
        }

        public async Task InitializeAsync()
        {
            if (IsInitialized)
                return;

            try
            {
                foreach (object service in _registrationOrder)
                {
                    if (service is IAsyncInitializable asyncInitializable)
                    {
                        await asyncInitializable.InitializeAsync();
                        _initializedServices.Add(service);
                    }
                    else if (service is IInitializable initializable)
                    {
                        initializable.Initialize();
                        _initializedServices.Add(service);
                    }
                }
                IsInitialized = true;
            }
            catch
            {
                ShutdownInitializedServices();
                throw;
            }
        }

        public void Shutdown()
        {
            ShutdownInitializedServices();
            IsInitialized = false;
        }

        public void Clear()
        {
            if (IsInitialized || _initializedServices.Count > 0)
                Shutdown();

            _services.Clear();
            _registrationOrder.Clear();
        }

        private void ShutdownInitializedServices()
        {
            for (int i = _initializedServices.Count - 1; i >= 0; i--)
            {
                if (_initializedServices[i] is IShutdownable shutdownable)
                {
                    shutdownable.Shutdown();
                }
            }

            _initializedServices.Clear();
        }

        private bool ContainsInstance(object instance)
        {
            foreach (object registered in _registrationOrder)
            {
                if (ReferenceEquals(registered, instance))
                    return true;
            }

            return false;
        }
    }
}