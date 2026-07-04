using System;

namespace DeviGames.Atlas.Core.Bootstrap.Events
{
    public readonly struct BootstrapFailedEvent
    {
        public Exception Exception { get; }

        public BootstrapFailedEvent(Exception exception)
        {
            Exception = exception;
        }
    }
}