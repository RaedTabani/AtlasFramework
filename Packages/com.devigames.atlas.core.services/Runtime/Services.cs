namespace DeviGames.Atlas.Core.Services
{
    public static class Services
    {
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
    }
}