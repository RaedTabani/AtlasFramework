using System;
using System.Collections.Generic;
using AtlasServices = DeviGames.Atlas.Core.Services.Services;

namespace DeviGames.Atlas.Dev.Hub.Context
{
    /// <summary>
    /// Provides Developer Hub modules with controlled access
    /// to the running Atlas service container.
    /// </summary>
    public sealed class RuntimeContext
    {
        public bool IsRunning => AtlasServices.IsInitialized;

        public IReadOnlyCollection<Type> RegisteredTypes =>
            AtlasServices.RegisteredTypes;

        public bool TryResolve<T>(out T service)
        {
            return AtlasServices.TryResolve(out service);
        }
    }
}