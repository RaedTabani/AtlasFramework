using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.Currency.Interfaces
{
    public interface ICurrencyService
    {
        IReadOnlyDictionary<string, int> Balances { get; }
        int GetBalance(string currencyId);

        bool Add(
            string currencyId,
            int amount);

        bool Spend(
            string currencyId,
            int amount);
    }
}