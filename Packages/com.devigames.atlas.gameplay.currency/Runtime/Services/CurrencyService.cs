using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.Currency.Events;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Models;

namespace DeviGames.Atlas.Gameplay.Currency.Services
{
    public sealed class CurrencyService :
        ICurrencyService
    {
        private readonly Dictionary<string, int> _balances =
            new(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, int> Balances => _balances;
        
        public int GetBalance(
            string currencyId)
        {
            ValidateCurrencyId(currencyId);

            return _balances.TryGetValue(currencyId, out int balance)
                ? balance
                : 0;
        }

        public bool Add(
            string currencyId,
            int amount)
        {
            ValidateCurrencyId(currencyId);

            if (amount <= 0)
            {
                return false;
            }

            int previousBalance =
                GetBalance(currencyId);

            int currentBalance =
                previousBalance + amount;

            _balances[currencyId] =
                currentBalance;

            EventBus.Publish(
                new CurrencyChangedEvent(
                    currencyId,
                    previousBalance,
                    currentBalance,
                    amount));

            return true;
        }

        public bool Spend(
            string currencyId,
            int amount)
        {
            ValidateCurrencyId(currencyId);

            if (amount <= 0)
            {
                return false;
            }

            int previousBalance =
                GetBalance(currencyId);

            if (previousBalance < amount)
            {
                return false;
            }

            int currentBalance =
                previousBalance - amount;

            _balances[currencyId] =
                currentBalance;

            EventBus.Publish(
                new CurrencyChangedEvent(
                    currencyId,
                    previousBalance,
                    currentBalance,
                    -amount));

            return true;
        }

        private static void ValidateCurrencyId(
            string currencyId)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
            {
                throw new ArgumentException(
                    "Currency ID cannot be empty.",
                    nameof(currencyId));
            }
        }

        public CurrencyData CreateSnapshot()
        {
            var data = new CurrencyData();

            foreach (KeyValuePair<string, int> pair in _balances)
            {
                data.Balances.Add( new CurrencyBalanceData(pair.Key,pair.Value));
            }

            return data;
        }

        public void Load(CurrencyData data)
        {
            _balances.Clear();

            if (data?.Balances == null)
            {
                return;
            }

            foreach (CurrencyBalanceData balance in data.Balances)
            {
                if (string.IsNullOrWhiteSpace(balance.CurrencyId))
                {
                    continue;
                }

                if (balance.Balance < 0)
                {
                    continue;
                }

                _balances[balance.CurrencyId] =
                    balance.Balance;
            }
        }
    }
}