using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Janito.Prototyping.Input
{
    /// <summary>
    /// Base class for input broadcasters that can be used to broadcast input events from an <c>InputAction</c> to <c>UnityEvent</c>s and expose relevant properties to the inspector.
    /// </summary>
    [Serializable]
    public abstract class BaseInputBroadcaster : IDisposable
    {
        /// <summary>
        /// Name of the element displayed in an array or collection.
        /// </summary>
        /// <remarks>
        /// Usually reflects the name of the intended input action of this broadcaster.
        /// </remarks>
        [SerializeField]
        [HideInInspector]
        private string _name;

        [Header("Configuration")]
        [SerializeField]
        [Tooltip("When active, it will enable and disable associated input action when this broadcaster is disabled and enabled.")]
        private bool _canDisableInputAction;

        [SerializeField]
        [Tooltip("When active, it will match the enable state set before disabling when re-enabling.")]
        private bool _shouldRespectActiveEnableState;

        /// <summary>
        /// Input Action that determines when to execute events of this broadcaster
        /// </summary>
        private InputAction _inputAction;

        /// <summary>
        /// Determines the enable state of the broadcaster when the owning GameObject is enabled and active. Only considered if <c>_shouldRespectActiveEnableState</c> is true.
        /// </summary>
        private bool _isActiveEnabled;

        /// <summary>
        /// Contains whether this broadcaster is currently enabled or disabled.
        /// </summary>
        private bool _isEnable;

        /// <summary>
        /// Property for setting the expected enable state to return to this broadcaster when GameObject is enabled and active.
        /// </summary>
        /// <remarks>
        /// This is only true when <c>_shouldRespectActiveEnableState</c> is true. Otherwise enable state will match exactly the GameObject.
        /// </remarks>
        public bool IsActiveEnabled { 
            get { return _isActiveEnabled; } 
            set { SetIsActiveEnableState(value); }
        }

        /// <summary>
        /// Name of the input action associated with this broadcaster. If the input action is not set, it will fallback to the name of the broadcaster.
        /// </summary>
        public string InputActionName { 
            get
            {
                return _inputAction != null ? _inputAction.name : _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Assigns the input action to this broadcaster and subscribes to its events. This method must be called before using the broadcaster.
        /// </summary>
        /// <param name="inputAction">The input action to associate with this broadcaster.</param>
        public virtual void Initialise(InputAction inputAction)
        {
            _inputAction = inputAction;
            if (_inputAction == null)
            {
                Debug.LogError($"[{GetType().Name}] Initialisation requires a valid {nameof(InputAction)}. Ignoring initialisation.");
                return;
            }

            _inputAction.started += OnInputActionStarted;
            _inputAction.performed += OnInputActionPerformed;
            _inputAction.canceled += OnInputActionCancelled;
        }

        /// <summary>
        /// Execute when owning GameObject is disabled.
        /// </summary>
        public virtual void OnDisable()
        {
            if (_canDisableInputAction)
            {
                _inputAction.Disable();
            }

            _isEnable = false;
        }


        /// <summary>
        /// Execute when owning GameObject is enabled.
        /// </summary>
        public virtual void OnEnable()
        {
            if (_canDisableInputAction)
            {
                if (_shouldRespectActiveEnableState)
                {
                    if (_isActiveEnabled)
                    {
                        _inputAction.Enable();
                    }
                }
                else
                {
                    _inputAction.Enable();
                }
            }

            _isEnable = true;
        }

        /// <summary>
        /// Execute when this object will no longer be used.
        /// </summary>
        public virtual void Dispose()
        {
            if (_inputAction != null)
            {
                _inputAction.started -= OnInputActionStarted;
                _inputAction.performed -= OnInputActionPerformed;
                _inputAction.canceled -= OnInputActionCancelled;
                _inputAction = null;
            }
        }

        protected abstract void OnInputActionStarted(InputAction.CallbackContext context);
        protected abstract void OnInputActionPerformed(InputAction.CallbackContext context);
        protected abstract void OnInputActionCancelled(InputAction.CallbackContext context);

        /// <summary>
        /// Updates <c>_isActiveEnabled</c> state and executes the <c>OnEnable</c> and <c>OnDisable</c> events as relevant.
        /// </summary>
        /// <param name="isEnabled"></param>
        private void SetIsActiveEnableState(bool isEnabled)
        {
            _isActiveEnabled = isEnabled;
            // If we are currently disabled, and we just got to set enable, enable or vice versa.
            if (!_isEnable && isEnabled)
            {
                OnEnable();
            }
            else if (_isEnable && !isEnabled) 
            {
                OnDisable();
            }
        }
    }

    /// <summary>
    /// Generic input broadcaster that can be used to broadcast input events from an <c>InputAction</c> to <c>UnityEvent</c>s. The generic type parameter <c>TEventType</c> determines the type of value that will be read from the <c>InputAction</c> and passed to the <c>UnityEvent</c>s.
    /// </summary>
    /// <typeparam name="TEventType">The type of value that will be read from the <c>InputAction</c> and passed to the <c>UnityEvent</c>s.</typeparam>
    [Serializable]
    public class InputBroadcaster<TEventType> : BaseInputBroadcaster
        where TEventType : struct
    {
        [Header("Events")]
        [Space(5.0f)]
        [SerializeField]
        public UnityEvent<TEventType> InputStarted;
        [SerializeField]
        public UnityEvent<TEventType> InputPerformed;
        [SerializeField]
        public UnityEvent<TEventType> InputCancelled;

        protected override void OnInputActionStarted(InputAction.CallbackContext context)
        {
            // Cant perform at init since the value type is not set until the action is performed for the first time
            if (context.valueType != typeof(TEventType))
            {
                Debug.LogError($"InputAction {context.action.name} has an active value type of {context.valueType}, which does not match the expected type of {typeof(TEventType)}.");
                return;
            }

            TEventType value = context.ReadValue<TEventType>();
            InputStarted.Invoke(value);
        }

        protected override void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            TEventType value = context.ReadValue<TEventType>();
            InputPerformed.Invoke(value);
        }

        protected override void OnInputActionCancelled(InputAction.CallbackContext context)
        {
            TEventType value = context.ReadValue<TEventType>();
            InputCancelled.Invoke(value);
        }
    }

    /// <summary>
    /// Parameterless input broadcaster that can be used to broadcast input events from an <c>InputAction</c> to <c>UnityEvent</c>s. This class will read no value from the <c>InputAction</c> and directly invoke the <c>UnityEvent</c>s.
    /// </summary>
    [Serializable]
    public class InputBroadcaster : BaseInputBroadcaster
    {
        [Header("Events")]
        [Space(5.0f)]
        [SerializeField]
        public UnityEvent InputStarted;
        [SerializeField]
        public UnityEvent InputPerformed;
        [SerializeField]
        public UnityEvent InputCancelled;

        protected override void OnInputActionStarted(InputAction.CallbackContext context)
        {
            InputStarted.Invoke();
        }

        protected override void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            InputPerformed.Invoke();
        }

        protected override void OnInputActionCancelled(InputAction.CallbackContext context)
        {
            InputCancelled.Invoke();
        }
    }
}
