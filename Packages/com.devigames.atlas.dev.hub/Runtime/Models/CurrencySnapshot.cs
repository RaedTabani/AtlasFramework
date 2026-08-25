namespace DeviGames.Atlas.Dev.Hub.Models
{
    public sealed class CurrencySnapshot
    {
        public string CurrencyId { get; }

        public int Balance { get; }

        public CurrencySnapshot(
            string currencyId,
            int balance)
        {
            CurrencyId = currencyId;
            Balance = balance;
        }
    }
}