using System;

namespace DeviGames.Atlas.Core.Save.Events
{
    /// <summary>
    /// Published by SaveService when a save operation throws.
    /// The original exception is still rethrown to the caller;
    /// this event exists for systems that want to react (UI, logging, telemetry)
    /// without needing to wrap every SaveAsync call in a try/catch.
    /// </summary>
    public readonly struct SaveFailedEvent
    {
        public string Key { get; }
        public string ErrorMessage { get; }

        public SaveFailedEvent(string key, string errorMessage)
        {
            Key = key;
            ErrorMessage = errorMessage;
        }

        public SaveFailedEvent(string key, Exception exception)
            : this(key, exception?.Message ?? string.Empty)
        {
        }
    }
}