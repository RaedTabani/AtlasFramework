namespace DeviGames.Atlas.Core.Events
{
    /// <summary>
    /// TEST ONLY utility.
    /// Never use in gameplay code.
    /// </summary>
    public static class EventBusTestUtility
    {
        public static void Reset()
        {
            EventBusProvider.Reset();
        }
    }
}