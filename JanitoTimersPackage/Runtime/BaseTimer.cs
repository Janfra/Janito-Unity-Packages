using System;
using UnityEngine;

namespace Janito.Timers
{
    /// <summary>
    /// Base class for a timer that can be started, stopped, paused, resumed, reset, and ticked. 
    /// </summary>
    /// </remarks>Ticking is managed by the TimerManager automatically, registering and unregistering respectively with </c>StartTimer<c> and </c>StopTimer<c> methods.</remarks>
    public abstract class BaseTimer : IReadOnlyTimer, IDisposable
    {
        /// <summary>
        /// Time left until the timer finishes.
        /// </summary>
        public float RemainingTime { get; protected set; }

        /// <summary>
        /// Amount of time ticked since the timer started.
        /// </summary>
        public float ElapsedTime { get => Mathf.Abs(_InitialTime - RemainingTime); }

        /// <summary>
        /// Is the timer currently ticking
        /// </summary>
        public bool IsActive { get => !IsPaused && IsRegistered; }

        /// <summary>
        /// Indicates whether the timer is currently paused.
        /// </summary>
        /// </remarks>This is used in addition to </c>IsActive<c> to avoid registering multiple times the timer if paused and then started.</remarks>
        public bool IsPaused { get; private set; } = false;

        /// <summary>
        /// Indicates whether the timer is registered with the TimerManager for ticking.
        /// </summary>
        public bool IsRegistered { get; private set; } = false;

        /// <summary>
        /// Progress of the timer from 1 (start) to 0 (end)
        /// </summary>
        public float InverseProgress => Mathf.Clamp01(RemainingTime / _InitialTime);

        /// <summary>
        /// Progress of the timer from 0 (start) to 1 (end)
        /// </summary>
        public float Progress => 1.0f - InverseProgress;

        public Action OnTimerStart { get; set; } = delegate { };
        public Action OnTimerStop { get; set; } = delegate { };

        /// <summary>
        /// Duration of the timer
        /// </summary>
        protected float _InitialTime;

        /// <summary>
        /// Indicates whether the timer has been disposed or is the process of being disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Constructor for Timer that can be started, stopped, reset, and ticked. 
        /// </summary>
        /// <param name="initialTime"></param>
        protected BaseTimer(float initialTime = 0.0f)
        {
            _InitialTime = Mathf.Clamp(initialTime, 0.0f, Mathf.Infinity);
            RemainingTime = 0.0f;
        }

        /// <summary>
        /// Destructor to ensure Dispose is called.
        /// </summary>
        ~BaseTimer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Registers the timer with the TimerManager and starts ticking it.
        /// </summary>
        public void StartTimer()
        {
            RemainingTime = _InitialTime;
            if (IsPaused)
            {
                Resume();
            }

            if (!IsRegistered)
            {
                IsRegistered = true;
                TimerManager.RegisterTimer(this);
                OnTimerStart.Invoke();
                OnStart();
            }
        }

        protected virtual void OnStart() { }

        /// <summary>
        /// Unregisters the timer from the TimerManager and stops ticking it. 
        /// </summary>
        public void StopTimer()
        {
            if (IsRegistered)
            {
                IsRegistered = false;
                TimerManager.UnregisterTimer(this);
                OnTimerStop.Invoke();
                OnStop();
            }
        }

        protected virtual void OnStop() { }

        /// <summary>
        /// Advances the state of the implementing timer by one unit of time or step.
        /// </summary>
        /// <remarks>The specific behavior of this method depends on the implementation. It is typically
        /// used to update the state of time-dependent or step-based systems. Must only be executed internally through the TimerManager.</remarks>
        public abstract void Tick();

        /// <summary>
        /// Defines whether the timer has completed its duration or reached its end state.
        /// </summary>
        public abstract bool IsFinished { get; }

        /// <summary>
        /// Resumes the ticking of the timer if it was previously paused.
        /// </summary>
        /// </remarks>This does not start the timer if it has not been started yet. It needs to be registered first.</remarks>
        public void Resume()
        {
            IsPaused = false;
            OnResume();
        }

        protected virtual void OnResume() { }

        /// <summary>
        /// Stops the ticking of the timer until resumed.
        /// </summary>
        /// </remarks>This does not unregister the timer from the TimerManager. It only pauses its ticking.</remarks>
        public void Pause() 
        {
            IsPaused = true;
            OnPaused();
        }

        protected virtual void OnPaused() { }

        /// <summary>
        /// Resets the timer to its initial time value.
        /// </summary>
        /// </remarks>This does not start the timer if it has not been started yet. It only resets the current time value.</remarks>
        public virtual void Reset() => RemainingTime = _InitialTime;

        /// <summary>
        /// Resets the timer to a new initial time value.
        /// </summary>
        /// <param name="newTime">New target time for the timer</param>
        /// </remarks>This does not start the timer if it has not been started yet. It only resets the current time value and updates the initial time.</remarks>
        public virtual void Reset(float newTime)
        {
            _InitialTime = newTime;
            Reset();
        }

        /// <summary>
        /// Call Dispose to ensure unregistration of the timer from the TimerManager when the consumer is done with the timer or being destroyed.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) 
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                TimerManager.UnregisterTimer(this);
            }

            _isDisposed = true;
        }
    }
}
