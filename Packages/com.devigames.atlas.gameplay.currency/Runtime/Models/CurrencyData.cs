using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Gameplay.Currency.Models
{
    [Serializable]
    public sealed class CurrencyData
    {
        public int Version = 1;

        public List<CurrencyBalanceData> Balances =
            new();
    }
}