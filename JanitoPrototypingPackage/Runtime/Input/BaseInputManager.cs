using Janito.EditorExtras;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Janito.Prototyping.Input
{
    /// TODO: Add events for device changes?

    /// <summary>
    /// Alternative to <c>PlayerInput</c> that includes cursor options and settings for handling enabling and disabling.
    /// </summary>
    /// <remarks>
    /// The main drawback is that it does not include listening for device changes. However since it utilises the generated C# class, it avoids the need for string lookups when directly accessing the instanced <c>Input System</c>.
    /// </remarks>
    /// <typeparam name="TInputSystem">Generated C# Input Action Asset wrapper class</typeparam>
    [DisallowMultipleComponent]
    public class BaseInputManager<TInputSystem> : MonoBehaviour
        where TInputSystem : IInputActionCollection2, new() // Generated C# class for Unity Input System currently implements this interface. It will eventually be merged with IInputActionCollection. Other types like InputActionAsset also implement it, but this is designed with the generated C# class in mind.
    {

        [Header("Cursor Awake Settings")]
        [SerializeField]
        private CursorLockMode _cursorLockMode;
        [SerializeField]
        private bool _isVisible = true;

        [Header("Input System Settings")]
        [SerializeField]
        [Tooltip("When active, it will automatically match the Input System enabled state to the GameObject enabled state. This is performed before input broadcasters update of state.")]
        private bool _canAutomaticallyMatchEnable = true;

        [Header("Generated Events")]
        /// <summary>
        /// Input Action Asset used to automatically generate the <c>InputBroadcaster</c> references.
        /// </summary>
        [SerializeField]
        [Tooltip("Sets the Input Actions from which to generate the exposed Input Broadcasters")]
        private InputActionAsset _assetSource;
        [SerializeReference]
        private BaseInputBroadcaster[] _events;

        private TInputSystem _inputSystem;
        private Dictionary<InputAction, BaseInputBroadcaster> _inputBroadcasters = new();

        /// <summary>
        /// Instance of the generated C# Input Action Asset wrapper class.
        /// </summary>
        /// <remarks>
        /// Lazy load the Input System in case this is accessed before it is awaken, avoiding issues with script execution order
        /// </remarks>
        public TInputSystem InputSystemInstance {
            get
            {
                if (_inputSystem == null)
                {
                    _inputSystem = new();
                }
                return _inputSystem;
            }
        }

        protected virtual void Awake()
        {
            Cursor.lockState = _cursorLockMode;
            Cursor.visible = _isVisible;
            RegisterGeneratedEventsToSourceInputAction();
        }

        /// <summary>
        /// Retrieves the instanced <c>InputAction</c> from the generated C# Input Action Asset wrapper class based on the provided static <c>InputAction</c> reference. Returns true if the instanced action was found, false otherwise.
        /// </summary>
        /// <param name="staticAction">The static <c>InputAction</c> reference</param>
        /// <param name="instancedAction">The instanced <c>InputAction</c> if found</param>
        /// <returns>True if the instanced action was found, false otherwise</returns>
        /// <remarks>
        /// <c>staticAction</c> can be a reference to an <c>InputAction</c> that matches the name of an action in the generated C# Input Action Asset wrapper class.
        /// </remarks>
        public bool TryGetInstancedActionFromStaticAction(InputAction staticAction, out InputAction instancedAction)
        {
            instancedAction = InputSystemInstance.FindAction(staticAction.name);
            return instancedAction != null;
        }

        /// <summary>
        /// Extracts the relevant events from the <c>_sourceAsset</c> and exposes them to <c>_events</c>.
        /// </summary>
        [Button("Regenerate Input Action Events", ButtonExecutionModes.EditorMode, "Regenerates the exposed events based on the existing input actions in the provided asset source. This does not replace existing events, it only adds missing events.")]
        protected void GenerateEventsForInputBroadcasters()
        {
            if (Application.isPlaying) return;

            InputActionAsset targetAsset = _assetSource ? _assetSource : InputSystem.actions;
            if (targetAsset == null)
            {
                this.LogErrorInDevelopment($"Unable to find an Input Action Asset. Will not generate input events. Provide a valid Input Action Asset in the inspector or ensure the Input System is properly initialized and the project-wide actions are available.");
                return;
            }

            List<InputAction> actions = new();

            foreach (var item in targetAsset)
            {
                if (TryGetInstancedActionFromStaticAction(item, out var instance))
                {
                    // Sometimes button control types have null expected control type, so still allow if it has bindings
                    if (!string.IsNullOrEmpty(instance.expectedControlType) || instance.bindings.Count > 0)
                    {
                        actions.Add(instance);
                    }
                }
            }

            CreateInputActionsInputBroadcasters(actions.ToArray());
        }

        /// <summary>
        /// Adds or replaces an <c>InputBroadcaster</c> for the provided <c>InputAction</c>. If an existing broadcaster is found, it will be disposed and replaced with the new one.
        /// </summary>
        /// <typeparam name="TBroadcaster">The type of the broadcaster</typeparam>
        /// <param name="inputAction">The input action to associate with the broadcaster</param>
        /// <param name="broadcaster">The broadcaster instance</param>
        protected void RegisterInputBroadcaster<TBroadcaster>(InputAction inputAction, TBroadcaster broadcaster)
            where TBroadcaster : BaseInputBroadcaster
        {
            if (_inputBroadcasters.ContainsKey(inputAction))
            {
                var oldBroadcaster = _inputBroadcasters[inputAction];
                oldBroadcaster.Dispose();
                _inputBroadcasters[inputAction] = broadcaster;

                this.LogWarningInDevelopment($"Overriding broadcaster for {nameof(InputAction)} {inputAction.name} from {oldBroadcaster.GetType().Name} to {nameof(TBroadcaster)}");
            }
            else
            {
                _inputBroadcasters.Add(inputAction, broadcaster);
            }

            broadcaster.Initialise(inputAction);
            
            // If we are currently disabled, set to disable.
            if (!gameObject.activeSelf)
            {
                broadcaster.OnDisable();
            }
        }

        /// <summary>
        /// Attempts to retrieve the registered <c>InputBroadcaster</c> for the provided <c>InputAction</c>. Returns true if found, false otherwise.
        /// </summary>
        /// <typeparam name="TBroadcaster">The type of the broadcaster</typeparam>
        /// <param name="inputAction">The input action associated with the broadcaster</param>
        /// <param name="broadcaster">The retrieved broadcaster instance if found</param>
        /// <returns>True if the broadcaster is found, false otherwise</returns>
        protected bool TryGetInputBroadcaster<TBroadcaster>(InputAction inputAction, out TBroadcaster broadcaster)
            where TBroadcaster : BaseInputBroadcaster
        {
            if (_inputBroadcasters.TryGetValue(inputAction, out BaseInputBroadcaster value) && value is TBroadcaster typedValue)
            {
                broadcaster = typedValue;
                return true;
            }

            broadcaster = null;
            return false;
        }

        /// <summary>
        /// Removes the registered <c>InputBroadcaster</c> for the provided <c>InputAction</c> and disposes of it. Returns true if the broadcaster was found and removed, false otherwise.
        /// </summary>
        /// <param name="inputAction">The input action associated with the broadcaster to remove</param>
        /// <returns>True if the broadcaster was found and removed, false otherwise</returns>
        protected bool RemoveInputBroadcaster(InputAction inputAction)
        {
            if (_inputBroadcasters.Remove(inputAction, out var value))
            {
                value.Dispose();
                return true;
            }
            return false;
        }

        protected virtual void OnEnable()
        {
            if (_canAutomaticallyMatchEnable)
            {
                _inputSystem.Enable();
            }

            SetRegisteredInputBroadcastersEnabledTo(true);
        }

        protected virtual void OnDisable()
        {
            if (_canAutomaticallyMatchEnable)
            {
                _inputSystem.Disable();
            }

            SetRegisteredInputBroadcastersEnabledTo(false);
        }

        protected virtual void OnDestroy()
        {
            DisposeOfRegisteredInputBroadcasters();
        }

        protected virtual void OnValidate()
        {
            GenerateEventsForInputBroadcasters();
        }

        /// <summary>
        /// Subscribes the generated events to the input system instance input action events. This is called during Awake to ensure that the events are registered with the correct input actions.
        /// </summary>
        private void RegisterGeneratedEventsToSourceInputAction()
        {
            foreach (var broadcaster in _events)
            {
                var inputAction = InputSystemInstance.FindAction(broadcaster.InputActionName);
                if (inputAction != null)
                {
                    RegisterInputBroadcaster(inputAction, broadcaster);
                }
            }
        }

        /// <summary>
        /// Unsubscribes and disposes of all registered input broadcasters. This is called during OnDestroy to ensure that all broadcasters are properly cleaned up.
        /// </summary>
        private void DisposeOfRegisteredInputBroadcasters()
        {
            foreach (var broadcaster in _inputBroadcasters.Values)
            {
                broadcaster.Dispose();
            }
        }

        /// <summary>
        /// Enables or disables all registered input broadcasters.
        /// </summary>
        /// <param name="isEnabled">true to enable broadcasters; false to disable them.</param>
        private void SetRegisteredInputBroadcastersEnabledTo(bool isEnabled)
        {
            // Does not lock or make copy of values, may change later if this becomes an issue
            foreach (var broadcaster in _inputBroadcasters.Values)
            {
                if (isEnabled)
                {
                    broadcaster.OnEnable();
                }
                else
                {
                    broadcaster.OnDisable();
                }
            }
        }

        /// <summary>
        /// Generates or updates the <c>InputBroadcaster</c> instances for the provided <c>InputAction</c> array. If an existing broadcaster is found for an action, it will be reused to preserve its state; otherwise, a new broadcaster will be created based on the expected control type of the action.
        /// </summary>
        /// <param name="actions">The array of <c>InputAction</c> instances for which to generate or update broadcasters.</param>
        private void CreateInputActionsInputBroadcasters(InputAction[] actions)
        {
            if (actions == null || actions.Length == 0) _events = null;

            // Create a temporary array to check for existing events before updating
            var temp = new BaseInputBroadcaster[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                BaseInputBroadcaster broadcaster = null;
                if (!IsInputBroadcasterAlreadyGenerated(action, out broadcaster)) 
                {
                    broadcaster = GetInputBroadcasterForExpectedControlType(action);
                    broadcaster.InputActionName = action.name;
                }
                temp[i] = broadcaster;
            }

            _events = new BaseInputBroadcaster[actions.Length];
            Array.Copy(temp, _events, actions.Length);
        }

        /// <summary>
        /// Returns a new instance of an <c>InputBroadcaster</c> based on the expected control type of the provided <c>InputAction</c>. If the expected control type is not recognized, a default <c>InputBroadcaster</c> is returned.
        /// </summary>
        /// <param name="action">The <c>InputAction</c> for which to create a broadcaster.</param>
        /// <returns>A new instance of an <c>InputBroadcaster</c> appropriate for the expected control type of the action.</returns>
        private BaseInputBroadcaster GetInputBroadcasterForExpectedControlType(InputAction action)
        {
            switch (action.expectedControlType)
            {
                case "Quaternion":
                    return new InputBroadcaster<Quaternion>();
                case "Vector2":
                    return new InputBroadcaster<Vector2>();
                case "Vector3":
                    return new InputBroadcaster<Vector3>();
                case "Button":
                case "Key":
                case null: // Sometimes actions expected control type is null, treat it as button.
                    return new InputBroadcaster();
                default:
                    return GetInputBroadcasterForExpectedControlType(action.expectedControlType);
            }
        }

        /// <summary>
        /// Use this method to provide a custom <c>InputBroadcaster</c> for unrecognized expected control types.
        /// </summary>
        /// <param name="expectedControlType">The expected control type string for which to create a broadcaster.</param>
        /// <returns>A new instance of an <c>InputBroadcaster</c> appropriate for the expected control type.</returns>
        /// <remarks>
        /// Expected control types that are already supported by the default implementation will not call this method. This method is only called for unrecognized expected control types. Override this method in derived classes to provide specific broadcaster types based on expected control types.
        /// </remarks>
        protected virtual BaseInputBroadcaster GetInputBroadcasterForExpectedControlType(string expectedControlType)
        {
            // Default implementation returns a parameterless InputBroadcaster to still provide events to unrecognized control types. 
            return new InputBroadcaster();
        }

        /// <summary>
        /// Determines if an <c>InputBroadcaster</c> has already been generated for the provided <c>InputAction</c>. If found, the existing broadcaster is returned via the out parameter.
        /// </summary>
        /// <param name="action">The <c>InputAction</c> to check for an existing broadcaster.</param>
        /// <param name="foundBroadcaster">The existing <c>InputBroadcaster</c> if found; otherwise, null.</param>
        /// <returns>True if an existing broadcaster is found; otherwise, false.</returns>
        private bool IsInputBroadcasterAlreadyGenerated(InputAction action, out BaseInputBroadcaster foundBroadcaster)
        {
            foreach (var broadcaster in _events)
            {
                if (broadcaster.InputActionName == action.name)
                {
                    // For now don't check if value type also matches, otherwise it would need either reflection or storing string. Just delete the event and regenerate
                    foundBroadcaster = broadcaster;
                    return true;
                }
            }

            foundBroadcaster = null;
            return false;
        }
    }
}