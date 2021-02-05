using System;
using System.Collections.Generic;
using System.Linq;
using dnWalker.Traversal;
using MMC.State;
using MMC.Util;

namespace dnWalker.ChoiceGenerators
{
    public class IntChoiceFromValueSet : IChoiceGenerator
    {
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly List<int> _numbers;
        private Path _path;

        public SchedulingData SchedulingData { get; private set; }

        public IntChoiceFromValueSet(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _numbers = Enumerable.Range(minValue, maxValue).ToList();
        }

        public Path Path => _path;

        public object GetNextChoice()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var index = random.Next(_numbers.Count - 1) % _numbers.Count;
            var nextChoice = _numbers[index];
            _numbers.RemoveAt(index);
            System.Diagnostics.Debug.WriteLine($"[{GetHashCode()}] {this} next={nextChoice} ({_numbers.Count()})");
            return nextChoice;
        }

        public bool HasMoreChoices => _numbers.Count != 0;

        IChoiceGenerator IChoiceGenerator.Previous { get; set; }

        void IChoiceGenerator.SetContext(ExplicitActiveState activeState)
        {
            SchedulingData = activeState.Collapse().Clone();

            if (SchedulingData.Delta.Threads.DeltaVal == -20)
            {
                SchedulingData = activeState.Collapse().Clone();
            }

            _path = (Path)((ICloneable)activeState.PathStore.CurrentPath).Clone();
        }

        SchedulingData IChoiceGenerator.GetBacktrackData()
        {
            return SchedulingData.Clone();
        }
    }
}
