using System;

using DeviGames.Atlas.Core.Sequence.Interfaces;

using TMPro;

namespace DeviGames.Playground.Sequence
{
    public sealed class UnitySequenceTextPresenter :
        ISequenceTextPresenter
    {
        private readonly TMP_Text _text;

        public UnitySequenceTextPresenter(
            TMP_Text text)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public void ShowText(
            string text)
        {
            _text.text =
                text ?? string.Empty;
        }
    }
}