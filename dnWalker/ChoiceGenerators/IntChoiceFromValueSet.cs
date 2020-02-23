using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMC.State;

namespace dnWalker.ChoiceGenerators
{
    public class IntChoiceFromValueSet : IChoiceGenerator
    {
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly List<int> _numbers;
        private SchedulingData _schedulingData;

        public IntChoiceFromValueSet(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;

            _numbers = Enumerable.Range(minValue, maxValue).ToList();
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
            _schedulingData = activeState.Collapse(activeState.StateStorage);
        }

        SchedulingData IChoiceGenerator.Advance()
        {
            return _schedulingData;
        }
    }
}
