using System;

using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Steps
{
    public sealed class ShowTextStep :
        ISequenceStep
    {
        private readonly string _text;
        private readonly ISequenceTextPresenter _presenter;

        public bool IsCompleted { get; private set; }

        public ShowTextStep(
            string text,
            ISequenceTextPresenter presenter)
        {
            _text =
                text;

            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public void Enter()
        {
            _presenter.ShowText(
                _text);

            IsCompleted =
                true;
        }
    }
}