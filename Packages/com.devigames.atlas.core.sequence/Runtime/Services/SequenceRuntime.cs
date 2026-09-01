using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class SequenceRuntime
    {
        private readonly IReadOnlyList<ISequenceStep> _steps;

        public SequenceDefinition Definition { get; }

        public string Id =>
            Definition.Id;

        public SequenceState State { get; private set; }

        public int CurrentStepIndex { get; private set; }

        public bool HasCurrentStepEntered { get; private set; }

        public ISequenceStep CurrentStep
        {
            get
            {
                if (CurrentStepIndex < 0 ||
                    CurrentStepIndex >= _steps.Count)
                {
                    return null;
                }

                return _steps[
                    CurrentStepIndex];
            }
        }

        public IReadOnlyList<ISequenceStep> Steps =>
            _steps;

        public SequenceRuntime(
            SequenceDefinition definition,
            IReadOnlyList<ISequenceStep> steps)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));

            if (string.IsNullOrWhiteSpace(definition.Id))
            {
                throw new ArgumentException(
                    "Sequence ID cannot be null or empty.",
                    nameof(definition));
            }

            State =
                SequenceState.Ready;

            CurrentStepIndex =
                -1;

            HasCurrentStepEntered =
                false;
        }

        public SequenceRuntime(
            SequenceDefinition definition)
            : this(
                definition,
                Array.Empty<ISequenceStep>())
        {
        }

        public bool Start()
        {
            if (State != SequenceState.Ready)
            {
                return false;
            }

            State =
                SequenceState.Playing;

            CurrentStepIndex =
                _steps.Count > 0
                    ? 0
                    : -1;

            HasCurrentStepEntered =
                false;

            return true;
        }

        public bool EnterCurrentStep()
        {
            if (State != SequenceState.Playing ||
                CurrentStep == null ||
                HasCurrentStepEntered)
            {
                return false;
            }

            HasCurrentStepEntered =
                true;

            CurrentStep.Enter();

            return true;
        }

        public bool MoveNext()
        {
            if (State != SequenceState.Playing)
            {
                return false;
            }

            if (CurrentStep == null ||
                !CurrentStep.IsCompleted)
            {
                return false;
            }

            CurrentStepIndex++;

            if (CurrentStepIndex >= _steps.Count)
            {
                CurrentStepIndex =
                    -1;
            }

            HasCurrentStepEntered =
                false;

            return true;
        }

        public bool Complete()
        {
            if (State != SequenceState.Playing)
            {
                return false;
            }

            if (CurrentStep != null)
            {
                return false;
            }

            State =
                SequenceState.Completed;

            return true;
        }

        public void Reset()
        {
            State =
                SequenceState.Ready;

            CurrentStepIndex =
                -1;

            HasCurrentStepEntered =
                false;
        }
    }
}