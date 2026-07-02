using System;
using System.Runtime.CompilerServices; 

[assembly: InternalsVisibleTo("DeviGames.Atlas.Core.Events.EditorTests")]

namespace DeviGames.Atlas.Core.Events
{
    internal static class EventBusProvider
    {
        private static IEventBus _current = new DefaultEventBus();

        public static IEventBus Current => _current;

        internal static void Reset()
        {
            _current = new DefaultEventBus();
        }
    }
}