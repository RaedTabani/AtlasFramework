using System;

namespace DeviGames.Atlas.Gameplay.Currency.Models
{
    [Serializable]
    public sealed class CurrencyBalanceData
    {
        public string CurrencyId;
        public int Balance;

        public CurrencyBalanceData()
        {
        }

        public CurrencyBalanceData(
            string currencyId,
            int balance)
        {
            CurrencyId = currencyId;
            Balance = balance;
        }
    }
}