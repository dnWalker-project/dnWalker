using System;
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
            System.Diagnostics.Debug.WriteLine($"[{GetHashCode()}] {this} next={nextChoice} ({_numbers.Count()})");
            return nextChoice;
        }

        public bool HasMoreChoices => _numbers.Count != 0;

        IChoiceGenerator IChoiceGenerator.Previous { get; set; }

        //private Collapser stateCollapser;
        private ThreadState _threadState;

        void IChoiceGenerator.SetContext(ExplicitActiveState activeState)
        {
            _threadState = activeState.CurrentThread;
            _schedulingData = activeState.Collapse/*Only*/().Clone();

            if (_schedulingData.Delta.Threads.DeltaVal == -20)
            {
                _schedulingData = activeState.Collapse/*Only*/().Clone();
            }
            
            if (!_schedulingData.Working.Any())
            {
                //_schedulingData.Enabled.Enqueue(activeState.CurrentThread.Id);
                //_schedulingData.Working.Enqueue(activeState.CurrentThread.Id);
            }

            //var sd = activeState.Collapse(new Collapser(activeState.StateCollapser.PoolData), activeState.StateStorage).Clone();
            //var sd = stateConvertor.SchedulingData;

            //var poolData = activeState.StateCollapser.PoolData.Clone();
            //stateCollapser = new Collapser(poolData, activeState);

            //_schedulingData = activeState.Collapse(stateCollapser, activeState.StateStorage);

            //var s = activeState.CollapseOnly();
            //_delta = _schedulingData.Delta.Clone();
            //_state = _schedulingData.State;
            //_schedulingData.Working.Enqueue(activeState.CurrentThread.Id);
            //_schedulingData.Enqueue(activeState.CurrentThread.Id);
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

        SchedulingData IChoiceGenerator.GetBacktrackData()
        {
            /*var x = false;
            if (x)
            {
                stateCollapser.DecollapseByDelta(_schedulingData.Delta);
            }*/
            //_schedulingData.Delta = _state.GetDelta();
            return _schedulingData.Clone();
        }
    }
}
