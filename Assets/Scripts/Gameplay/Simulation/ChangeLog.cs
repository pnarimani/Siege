using System.Collections.Generic;

namespace Siege.Gameplay.Simulation
{
    public class ChangeLog
    {
        readonly List<StateChange> _current = new();
        readonly List<StateChange> _lastDay = new();
        readonly List<StateChange> _reusableBuffer = new();

        public IReadOnlyList<StateChange> CurrentChanges => _current;
        public IReadOnlyList<StateChange> LastDayChanges => _lastDay;

        public void Record(StateChange change)
        {
            _current.Add(change);
        }

        public void Record(string field, double amount, string source)
        {
            _current.Add(new StateChange(field, amount, source));
        }

        /// <summary>
        /// Called at the start of each day to snapshot yesterday's changes and clear the buffer.
        /// </summary>
        public void FlushDay()
        {
            _lastDay.Clear();
            _lastDay.AddRange(_current);
            _current.Clear();
        }

        /// <summary>
        /// Returns all current changes for a specific field into a reusable buffer.
        /// Warning: buffer is reused — do not cache the returned list.
        /// </summary>
        public List<StateChange> GetChangesForField(string field)
        {
            _reusableBuffer.Clear();
            foreach (var c in _current)
                if (c.Field == field)
                    _reusableBuffer.Add(c);
            return _reusableBuffer;
        }

        /// <summary>
        /// Returns the net change for a field this day so far.
        /// </summary>
        public double GetNetChange(string field)
        {
            double total = 0;
            foreach (var c in _current)
                if (c.Field == field)
                    total += c.Amount;
            return total;
        }

        /// <summary>
        /// Returns all changes recorded since the given index into a reusable buffer.
        /// Warning: buffer is reused — do not cache the returned list.
        /// </summary>
        public List<StateChange> SliceSince(int fromIndex)
        {
            _reusableBuffer.Clear();
            for (int i = fromIndex; i < _current.Count; i++)
                _reusableBuffer.Add(_current[i]);
            return _reusableBuffer;
        }
    }
}
