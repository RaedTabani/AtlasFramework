namespace DeviGames.Atlas.Gameplay.Events
{
    public readonly struct DoorOpenedEvent
    {
        public string DoorId { get; }

        public DoorOpenedEvent(string doorId)
        {
            DoorId = doorId;
        }
    }
}