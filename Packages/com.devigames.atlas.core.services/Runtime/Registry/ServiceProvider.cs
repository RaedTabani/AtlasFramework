namespace DeviGames.Atlas.Core.Services
{
    internal static class ServiceProvider
    {
        private static ServiceRegistry _current = new();

        public static ServiceRegistry Current => _current;

        internal static void Set(ServiceRegistry registry)
        {
            _current = registry;
        }

        internal static void Reset()
        {
            _current = new ServiceRegistry();
        }
    }
}