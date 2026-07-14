namespace DeviGames.Atlas.Core.Services
{
    internal static class ServiceProvider
    {
        private static ServiceRegistry _current = new();

        public static ServiceRegistry Current => _current;

        internal static void Set(ServiceRegistry registry)
        {
            if (registry == null)
                throw new System.ArgumentNullException(nameof(registry));

            if (!ReferenceEquals(_current, registry))
            {
                _current.Shutdown();
            }

            _current = registry;
        }

        internal static void Reset()
        {
            _current.Shutdown();
            _current = new ServiceRegistry();
        }
    }
}