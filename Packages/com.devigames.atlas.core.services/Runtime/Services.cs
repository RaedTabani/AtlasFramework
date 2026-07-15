using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Services
{
    public static class Services
    {
        public static IReadOnlyCollection<Type> RegisteredTypes =>
            ServiceProvider.Current.RegisteredTypes;

        public static IReadOnlyCollection<object> InitializedServices =>
            ServiceProvider.Current.InitializedServices;

        public static bool IsInitialized =>
            ServiceProvider.Current.IsInitialized;

        public static void Register<T>(T service)
        {
            ServiceProvider.Current.Register(service);
        }

        public static T Resolve<T>()
        {
            return ServiceProvider.Current.Resolve<T>();
        }

        public static bool TryResolve<T>(out T service)
        {
            return ServiceProvider.Current.TryResolve(out service);
        }

        public static bool IsRegistered<T>()
        {
            return ServiceProvider.Current.IsRegistered<T>();
        }

        public static void Unregister<T>()
        {
            ServiceProvider.Current.Unregister<T>();
        }

        public static Task InitializeAsync()
        {
            return ServiceProvider.Current.InitializeAsync();
        }

        public static void Shutdown()
        {
            ServiceProvider.Current.Shutdown();
        }

        public static void SetRegistry(ServiceRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            ServiceProvider.Set(registry);
        }
    }
}