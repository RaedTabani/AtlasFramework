namespace DeviGames.Atlas.Core.Events.Tests
{
    public readonly struct TestEvent
    {
        public readonly int Value;

        public TestEvent(int value)
        {
            Value = value;
        }
    }
}