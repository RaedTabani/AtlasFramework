using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Sequence.Events;
using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class SequencePlayer :
        ISequencePlayer
    {
        public SequenceRuntime ActiveSequence { get; private set; }

        public bool IsPlaying =>
            ActiveSequence != null;

        public bool Play(
            SequenceRuntime sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(
                    nameof(sequence));
            }

            if (IsPlaying)
            {
                return false;
            }

            if (!sequence.Start())
            {
                return false;
            }

            ActiveSequence =
                sequence;

            Process();

            return true;
        }

        public bool Advance()
        {
            if (ActiveSequence == null)
            {
                return false;
            }

            Process();

            return true;
        }

        public bool Complete()
        {
            if (ActiveSequence == null)
            {
                return false;
            }

            return TryCompleteActiveSequence();
        }

        private void Process()
        {
            while (ActiveSequence != null)
            {
                ISequenceStep step =
                    ActiveSequence.CurrentStep;

                if (step == null)
                {
                    TryCompleteActiveSequence();
                    return;
                }

                if (!ActiveSequence.HasCurrentStepEntered)
                {
                    ActiveSequence.EnterCurrentStep();
                }

                if (!step.IsCompleted)
                {
                    return;
                }

                ActiveSequence.MoveNext();
            }
        }

        public bool Continue()
        {
            if (ActiveSequence?.CurrentStep is not IContinuableSequenceStep continuableStep)
            {
                return false;
            }

            if (!continuableStep.Continue())
            {
                return false;
            }

            Process();

            return true;
        }
        
        private bool TryCompleteActiveSequence()
        {
            if (ActiveSequence == null ||
                !ActiveSequence.Complete())
            {
                return false;
            }

            SequenceRuntime completedSequence =
                ActiveSequence;

            ActiveSequence =
                null;

            EventBus.Publish(
                new SequenceCompletedEvent(
                    completedSequence.Id));

            return true;
        }
    }
}