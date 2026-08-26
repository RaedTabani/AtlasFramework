using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Gameplay.Currency.Interfaces;
using DeviGames.Atlas.Gameplay.Currency.Models;

namespace DeviGames.Atlas.Gameplay.Currency.Services
{
    public sealed class CurrencySaveCoordinator : ISaveParticipant
    {
        private readonly ICurrencyService _currencyService;
        private readonly SaveService _saveService;

        public string Key => "currency";

        public CurrencySaveCoordinator(
            ICurrencyService currencyService,
            SaveService saveService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
            _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
        }

        public async Task SaveAsync()
        {
            CurrencyData data =
                _currencyService.CreateSnapshot();

            await _saveService.SaveAsync(
                Key,
                data);
        }

        public async Task LoadAsync()
        {
            bool exists =
                await _saveService.ExistsAsync(
                    Key);

            if (!exists)
            {
                return;
            }

            CurrencyData data =
                await _saveService.LoadAsync<CurrencyData>(
                    Key);

            _currencyService.Load(
                data);
        }
    }
}