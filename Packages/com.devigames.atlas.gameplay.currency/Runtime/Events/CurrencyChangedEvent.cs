namespace DeviGames.Atlas.Gameplay.Currency.Events
{
    public readonly struct CurrencyChangedEvent
    {
        public string CurrencyId { get; }

        public int PreviousBalance { get; }

        public int CurrentBalance { get; }

        public int Delta { get; }

        public CurrencyChangedEvent(
            string currencyId,
            int previousBalance,
            int currentBalance,
            int delta)
        {
            CurrencyId = currencyId;
            PreviousBalance = previousBalance;
            CurrentBalance = currentBalance;
            Delta = delta;
        }
    }
}