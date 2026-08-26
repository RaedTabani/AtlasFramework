using System.Collections.Generic;

using DeviGames.Atlas.Gameplay.Currency.Models;

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

        CurrencyData CreateSnapshot();

        void Load(CurrencyData data);
    }
}