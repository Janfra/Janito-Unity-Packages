using System.Collections.Generic;

namespace Janito.Timers
{
    public static class TimerManager
    {
        /// <summary>
        /// List of registered timers to be updated.
        /// </summary>
        private static readonly List<BaseTimer> _timers = new();

        /// <summary>
        /// List of timers to be swept and updated this frame.
        /// </summary>
        private static readonly List<BaseTimer> _sweep = new();

        /// <summary>
        /// Registers a timer to be updated by the TimerManager.
        /// </summary>
        /// <param name="timer">Timer to tick</param>
        public static void RegisterTimer(BaseTimer timer) => _timers.Add(timer);

        /// <summary>
        /// Unregisters a timer so it is no longer updated by the TimerManager.
        /// </summary>
        /// <param name="timer">Timer to remove and stop ticking</param>
        public static void UnregisterTimer(BaseTimer timer) => _timers.Remove(timer);

        /// <summary>
        /// Function called every frame to update all registered timers.
        /// </summary>
        public static void UpdateTimers()
        {
            if (_timers.Count == 0) return;

            RefreshSweep();
            foreach (var timer in _sweep)
            {
                timer.Tick();
            }
        }

        /// <summary>
        /// Removes all registered timers from the TimerManager.
        /// </summary>
        public static void ClearTimers()
        {
            RefreshSweep();
            foreach (var timer in _sweep)
            {
                timer.Dispose();
            }

            _timers.Clear();
            _sweep.Clear();
        }

        /// <summary>
        /// Refreshes the sweep list to match the current registered timers.
        /// </summary>
        private static void RefreshSweep()
        {
            _sweep.Clear();
            _sweep.AddRange(_timers);
        }
    }
}
