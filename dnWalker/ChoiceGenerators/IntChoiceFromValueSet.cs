﻿using System;
using System.Collections.Generic;
using System.Linq;
using MMC.State;
using MMC.Util;

namespace dnWalker.ChoiceGenerators
{
    public class IntChoiceFromValueSet : IChoiceGenerator
    {
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly List<int> _numbers;
        private SchedulingData _schedulingData;
        private CollapsedStateDelta _delta;
        private SparseElement _threads;
        private CollapsedState _state;

        public SchedulingData SchedulingData => _schedulingData;

        public IntChoiceFromValueSet(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _numbers = Enumerable.Range(minValue, maxValue + 1).ToList();
        }

        public object GetNextChoice()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(_numbers.Count - 1) % _numbers.Count;
            var nextChoice = _numbers[index];
            _numbers.RemoveAt(index);
            return nextChoice;
        }

        public bool HasMoreChoices => _numbers.Count != 0;

        IChoiceGenerator IChoiceGenerator.Previous { get; set; }

        void IChoiceGenerator.SetContext(ExplicitActiveState activeState)
        {
            _schedulingData = activeState.CollapseOnly().Clone();
            _delta = _schedulingData.Delta.Clone();
            _state = _schedulingData.State;
            //_schedulingData = new SchedulingData
            /*{
                Delta = collapsedCurrent.Delta
            };
            _delta = _schedulingData.Delta;// collapsedCurrent.GetDelta();
            _schedulingData.ID = collapsedCurrent.ID;
            if (_delta.Threads != null)
            {
                //_delta.Threads = new SparseElement(_delta.Threads.Index, _delta.Threads.DeltaVal, null);
                _threads = new SparseElement(_delta.Threads.Index, _delta.Threads.DeltaVal, null);
            }*/
        }

        SchedulingData IChoiceGenerator.Advance()
        {
            //_schedulingData.Delta = _state.GetDelta();
            return _schedulingData;
        }
    }
}
